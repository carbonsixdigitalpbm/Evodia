using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using Evodia.Voyager.Common;
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

        public Voyager(IContentService contentService)
        {
            _contentService = contentService;
            _jobs = new List<VacancyFeed>();
            _api = VoyagerApi.Instance();
        }

        public void Fetch()
        {
            var xmlFilePaths = _api.GetXmlFilePaths();

            foreach (var filePath in xmlFilePaths)
            {
                ReadAndDeserializeXmlFile(filePath);
            }
        }

        public void SyncAll()
        {
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

            _api.DeleteXmlFiles();
        }

        private void DeleteSingle(VacancyFeed vacancy)
        {
            try
            {
                var jobsRoot = GetJobsRoot();
                var allDecendants = jobsRoot.Descendants().Where(d => d.HasProperty("jobReference")).ToList();
                var nodeToDelete =
                    allDecendants.SingleOrDefault(
                        d => d.GetValue<string>("jobReference") == vacancy.VacancyPosting.Vacancy.JobReference);

                if (nodeToDelete != null)
                {
                    _contentService.Delete(nodeToDelete);
                }

            }
            catch (Exception ex)
            {
                LogHelper.Info(GetType(),
                    "Failed to DETELE vacancy " + vacancy.VacancyPosting.Vacancy.JobReference + " with message: " + ex.Message);
            }
        }

        private void SyncSingle(VacancyFeed vacancy)
        {
            try
            {
                var jobsRoot = GetJobsRoot();
                var allDecendants = jobsRoot.Descendants().Where(d => d.HasProperty("jobReference")).ToList();
                var nodeToSync =
                    allDecendants.SingleOrDefault(
                        d => d.GetValue<string>("jobReference") == vacancy.VacancyPosting.Vacancy.JobReference);

                if (nodeToSync == null) return;

                SetVoyagerProperties(nodeToSync, vacancy);

                _contentService.SaveAndPublishWithStatus(nodeToSync);
            }
            catch (Exception ex)
            {
                LogHelper.Info(GetType(),
                    "Failed to SYNC vacancy " + vacancy.VacancyPosting.Vacancy.JobReference + " with message: " + ex.Message);
            }
        }

        public void CreateSingle(VacancyFeed vacancy)
        {
            try
            {
                var newNode = _contentService.CreateContent(vacancy.VacancyPosting.Vacancy.JobTitle.Trim(), Data.Constants.JobsFolderId, "vacancy");

                SetVoyagerProperties(newNode, vacancy);

                _contentService.SaveAndPublishWithStatus(newNode);
            }
            catch (Exception ex)
            {
                LogHelper.Info(GetType(),
                    "Failed to SYNC vacancy " + vacancy.VacancyPosting.Vacancy.JobReference + " with message: " + ex.Message);
            }
        }

        private void CheckAgaintUmbracoNodes(IEnumerable<VacancyFeed> vacancies)
        {
            try
            {
                var jobsRoot = GetJobsRoot();
                var allDecendants = jobsRoot.Descendants().Where(d => d.HasProperty("jobReference")).ToList();

                foreach (var vacancy in vacancies)
                {
                    if (vacancy.VacancyPosting.Vacancy.AdvertStatus.Equals("Delete", StringComparison.OrdinalIgnoreCase))
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
            catch (Exception)
            {
                throw new DuplicateNameException("Two vacancies have the same Voyager Job Reference. Please check your vacancies.");
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
            newNode.Name = vacancy.VacancyPosting.Vacancy.JobTitle.Trim();

            newNode.SetValue("jobReference", vacancy.VacancyPosting.Vacancy.JobReference);
            newNode.SetValue("fingerprint", vacancy.FingerPrint);
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

                throw;
            }
        }
    }
}
