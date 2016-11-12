using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Xml.Serialization;
using Evodia.Voyager.Common;
using Evodia.Voyager.Domain.Models;
using Evodia.Voyager.Domain.VoyagerObjects;
using Evodia.Voyager.Exceptions;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using File = System.IO.File;

namespace Evodia.Voyager.Domain
{
    public class Voyager
    {
        private readonly IContentService _contentService;

        private readonly List<VacancyFeed> _jobs;

        private readonly VoyagerApi _api;

        private List<SyncFile> _syncFiles;
         
        public Stats Statistics;

        public Voyager(IContentService contentService)
        {
            _contentService = contentService;
            _jobs = new List<VacancyFeed>();
            _api = VoyagerApi.Instance();
            _syncFiles = new List<SyncFile>();

            Statistics = new Stats();
        }

        public void Fetch()
        {
            _syncFiles = _api.GetXmlSyncFiles();

            if (_syncFiles != null)
            {
                var mostRecentFiles = _syncFiles.GroupBy(f => f.JobReferenceNumber).Select(f => f.OrderByDescending(d => d.FileUpDateTime).First()).ToList();

                foreach (var file in mostRecentFiles)
                {
                    ReadAndDeserializeXmlFile(file.FileLocation);
                }
            }
        }

        public Stats GetStatistics()
        {
            return Statistics;
        }

        public void SyncAll()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            CheckVoyagerProperties();
            Fetch();
            CheckAgaintUmbracoNodes(_jobs);

            foreach (var job in _jobs)
            {
                if (job.Delete)
                {
                    DeleteSingle(job);
                }
                else if (job.New)
                {
                    CreateSingle(job);
                }
                else if (job.Dirty)
                {
                    SyncSingle(job);
                }
            }

            _api.DeleteXmlFiles(_syncFiles);
            watch.Stop();

            Statistics.ElapsedSeconds = watch.Elapsed.TotalSeconds;
        }

        private void DeleteSingle(VacancyFeed vacancy)
        {
            try
            {
                var jobsRoot = GetJobsRoot();
                var allDecendants = jobsRoot.Descendants().Where(d => d.HasProperty("jobReference")).ToList();
                var nodeToDelete =
                    allDecendants.SingleOrDefault(d => d.GetValue<string>("jobReference") == vacancy.VacancyPosting.Vacancy.JobReference);

                if (nodeToDelete != null)
                {
                    _contentService.Delete(nodeToDelete);

                    Statistics.Deleted++;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Info(GetType(), "Failed to DETELE vacancy " + vacancy.VacancyPosting.Vacancy.JobReference + " with message: " + ex.Message);
            }
        }

        private void SyncSingle(VacancyFeed vacancy)
        {
            try
            {
                var jobsRoot = GetJobsRoot();
                var allDecendants = jobsRoot.Descendants().Where(d => d.HasProperty("jobReference")).ToList();
                var nodeToSync =
                    allDecendants.SingleOrDefault(d => d.GetValue<string>("jobReference") == vacancy.VacancyPosting.Vacancy.JobReference);

                if (nodeToSync == null) return;

                SetVoyagerProperties(nodeToSync, vacancy);

                _contentService.SaveAndPublishWithStatus(nodeToSync);

                Statistics.Updated++;
            }
            catch (Exception ex)
            {
                LogHelper.Info(GetType(), "Failed to SYNC vacancy " + vacancy.VacancyPosting.Vacancy.JobReference + " with message: " + ex.Message);
            }
        }

        public void CreateSingle(VacancyFeed vacancy)
        {
            try
            {
                var newNode = _contentService.CreateContent(vacancy.VacancyPosting.Vacancy.JobTitle.Trim(), Data.Constants.JobsFolderId, "job");

                SetVoyagerProperties(newNode, vacancy);

                _contentService.SaveAndPublishWithStatus(newNode);

                Statistics.Created++;
            }
            catch (Exception ex)
            {
                LogHelper.Info(GetType(), "Failed to CREATE vacancy " + vacancy.VacancyPosting.Vacancy.JobReference + " with message: " + ex.Message);
            }
        }

        private void CheckAgaintUmbracoNodes(IEnumerable<VacancyFeed> vacancies)
        {
            try
            {
                var jobsRoot = GetJobsRoot();
                var allDecendants = jobsRoot.Descendants().Where(d => d.HasProperty("jobReference")).ToList();

                foreach (var vacancy in vacancies.Where(v => v.VacancyPosting.Vacancy.JobReference != null))
                {
                    if (vacancy.VacancyPosting.Vacancy.AdvertStatus != null && vacancy.VacancyPosting.Vacancy.AdvertStatus.Equals("Delete", StringComparison.OrdinalIgnoreCase))
                    {
                        vacancy.Delete = true;
                    }
                    else
                    {
                        vacancy.Delete = false;

                        var vacancyNode = allDecendants.SingleOrDefault(d => d.GetValue<string>("jobReference") == vacancy.VacancyPosting.Vacancy.JobReference);

                        if (vacancyNode == null)
                        {
                            vacancy.New = true;
                        }
                        else
                        {
                            vacancy.New = false;
                            vacancy.Dirty = vacancy.FingerPrint != vacancyNode.GetValue<string>("fingerprint");
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                throw new DuplicateNameException("Erro: " + ex.Message);
            }
        }

        private static void CheckVoyagerProperties()
        {
            var cs = ApplicationContext.Current.Services.ContentTypeService;

            if (!cs.GetContentType("Job").PropertyTypeExists("jobReference"))
            {
                throw new Exception("The Job document type does not have the jobReference property");
            }

            if (!cs.GetContentType("Job").PropertyTypeExists("fingerprint"))
            {
                throw new Exception("The Job document type does not have the fingerprint property");
            }
        }

        private IContent GetJobsRoot()
        {
            var content = (IContent)ApplicationContext.Current.ApplicationCache.RuntimeCache.GetCacheItem(Data.Constants.JobsRootCacheKey);

            if (content != null) return content;

            content = _contentService.GetById(Data.Constants.JobsRootId).Descendants().SingleOrDefault(n => n.ContentType.Name == "Jobs");

            if (content == null)
            {
                throw new NoRootNodeException();
            }

            ApplicationContext.Current.ApplicationCache.RuntimeCache.InsertCacheItem(Data.Constants.JobsRootCacheKey, () => content);

            return content;
        }

        private static void SetVoyagerProperties(IContent newNode, VacancyFeed vacancy)
        {
            SetUmbracoProperty(newNode, "fingerprint", vacancy.FingerPrint);

            var vacancyPosting = vacancy.VacancyPosting;

            if (vacancyPosting == null) return;

            var vacancyElement = vacancyPosting.Vacancy;

            if (vacancyElement != null)
            {
                newNode.Name = vacancyElement.JobTitle;

                var attributes = vacancyElement.Attributes;

                if (attributes != null)
                {
                    SetNestedContentAttributes(newNode, "attributes", attributes);
                }

                SetUmbracoProperty(newNode, "jobReference", vacancyElement.JobReference);
                SetUmbracoProperty(newNode, "clientJobTitle", vacancyElement.ClientJobTitle);
                SetUmbracoProperty(newNode, "jobType", vacancyElement.JobType);
                SetUmbracoProperty(newNode, "sector", vacancyElement.Sector);
                SetUmbracoProperty(newNode, "company", vacancyElement.Company);
                SetUmbracoProperty(newNode, "contact", vacancyElement.Contact);
                SetUmbracoProperty(newNode, "class1", vacancyElement.Class1);
                SetUmbracoProperty(newNode, "class2", vacancyElement.Class2);
                SetUmbracoProperty(newNode, "class3", vacancyElement.Class3);
                //SetUmbracoProperty(newNode, "jobDescription", vacancyElement.JobDescription);

                var jobLocation = vacancyElement.JobLocation;

                if (jobLocation != null)
                {
                    SetUmbracoProperty(newNode, "location", jobLocation.Location);
                    SetUmbracoProperty(newNode, "addressLine1", jobLocation.AddressLine1);
                    SetUmbracoProperty(newNode, "addressLine2", jobLocation.AddressLine2);
                    SetUmbracoProperty(newNode, "addressLine3", jobLocation.AddressLine3);
                    SetUmbracoProperty(newNode, "town", jobLocation.Town);
                    SetUmbracoProperty(newNode, "county", jobLocation.County);
                    SetUmbracoProperty(newNode, "postcode", jobLocation.Postcode);
                    SetUmbracoProperty(newNode, "country", jobLocation.Country);
                    SetUmbracoProperty(newNode, "countryCode", jobLocation.CountryCode);
                }

                var compensation = vacancyElement.Compensation;

                if (compensation != null)
                {
                    var salaryDescription = compensation.SalaryDescription;

                    if (salaryDescription != null)
                    {
                        var salaryRange = salaryDescription.SalaryRange;

                        if (salaryRange != null)
                        {
                            SetUmbracoProperty(newNode, "from", salaryRange.From);
                            SetUmbracoProperty(newNode, "to", salaryRange.To);
                            SetUmbracoProperty(newNode, "packageMin", salaryRange.PackageMin);
                            SetUmbracoProperty(newNode, "packageMax", salaryRange.PackageMax);
                            SetUmbracoProperty(newNode, "isoCurrency", salaryRange.IsoCurrency);
                            SetUmbracoProperty(newNode, "period", salaryRange.Period);
                        }
                    }
                }
            }

            var consultants = vacancyPosting.Consultants;

            if (consultants != null)
            {
                SetNestedContentConsultants(newNode, "consultants", consultants);
            }
        }

        private static void SetNestedContentConsultants(IContent newNode, string propertyAlias, Consultants consultants)
        {
            var ncItems = new List<dynamic>();

            foreach (var consultant in consultants.Consultant)
            {
                if (consultant == null) continue;

                dynamic ncItem = new ExpandoObject();

                ((IDictionary<string, object>)ncItem).Add("ncContentTypeAlias", "consultant");
                ((IDictionary<string, object>)ncItem).Add("emailAddress", consultant.EmailAddress);

                if (consultant.Name != null)
                {
                    ((IDictionary<string, object>)ncItem).Add("firstName", consultant.Name.First);
                    ((IDictionary<string, object>)ncItem).Add("lastName", consultant.Name.Last);
                }

                var phoneNumbers = consultant.PhoneNumbers;

                if (phoneNumbers != null)
                {
                    var voice = phoneNumbers.Voice;
                    var fax = phoneNumbers.Fax;
                    var mobile = phoneNumbers.ConsultantMobile;

                    if (voice != null)
                    {
                        ((IDictionary<string, object>)ncItem).Add("voice", voice.TelNumber);
                    }

                    if (fax != null)
                    {
                        ((IDictionary<string, object>)ncItem).Add("fax", fax.TelNumber);
                    }

                    if (mobile != null)
                    {
                        ((IDictionary<string, object>)ncItem).Add("consultantMobile", mobile.TelNumber);
                    }
                }

                ncItems.Add(ncItem);
            }

            var ncItemString = Newtonsoft.Json.JsonConvert.SerializeObject(ncItems);

            newNode.SetValue(propertyAlias, ncItemString);
        }

        private static void SetNestedContentAttributes(IContent newNode, string propertyAlias, Attributes attributes)
        {
            var ncItems = new List<dynamic>();

            foreach (var attribute in attributes.Attribute)
            {
                if (attribute == null) continue;

                dynamic ncItem = new ExpandoObject();

                ((IDictionary<string, object>)ncItem).Add("ncContentTypeAlias", "attribute");
                ((IDictionary<string, object>)ncItem).Add("name", attribute.Name);
                ((IDictionary<string, object>)ncItem).Add("essential", attribute.Essential);

                ncItems.Add(ncItem);
            }

            var ncItemString = Newtonsoft.Json.JsonConvert.SerializeObject(ncItems);

            newNode.SetValue(propertyAlias, ncItemString);
        }

        private static void SetUmbracoProperty(IContent node, string propertyAlias, string value)
        {
            if (node.HasProperty(propertyAlias) && !string.IsNullOrEmpty(value))
            {
                node.SetValue(propertyAlias, value);
            }
        }

        public void ReadAndDeserializeXmlFile(string path)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(VacancyFeed));
                VacancyFeed job;

                using (var file = File.OpenText(path))
                {
                    job = (VacancyFeed)serializer.Deserialize(file);
                }

                if (job == null) return;

                if (_jobs.SingleOrDefault(j => j.VacancyPosting.Vacancy.JobReference == job.VacancyPosting.Vacancy.JobReference) == null)
                {
                    job.FingerPrint = Hash.CreateMd5(XmlUtils.SerializeToString(job));

                    _jobs.Add(job);
                }
                else
                {
                    LogHelper.Info(GetType(), "Job with reference " + job.VacancyPosting.Vacancy.JobReference + " already exists.");
                }

            }
            catch (Exception ex)
            {
                LogHelper.Warn(GetType(), ex.Message);
            }
        }
    }
}
