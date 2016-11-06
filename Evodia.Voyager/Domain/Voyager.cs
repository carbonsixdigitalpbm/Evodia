using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using Evodia.Voyager.Domain.VoyagerObjects;
using Evodia.Voyager.Exceptions;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
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
            var fileNames = _api.GetXmlFileNames();

            foreach (var fileName in fileNames)
            {
                ReadAndDeserializeXmlFile(fileName);
            }
        }

        public void SyncAll()
        {
            var jobsRoot = GetPublishedJobsRoot();

            CheckVoyagerProperties(jobsRoot);
            Fetch();
            CheckAgaintUmbracoNodes(_jobs);

            foreach (var job in _jobs)
            {
                if (job.New)
                {
                    CreateSingle(job);
                }
                else if (job.Dirty)
                {
                    SyncSingle(job);
                }
                else if (job.Delete)
                {
                    DeleteSingle(job);
                }
            }
        }

        private void CreateSingle(VacancyFeed job)
        {
            LogHelper.Info(GetType(), "Create job: " + job.VacancyPosting.Vacancy.JobReference);
        }

        private void DeleteSingle(VacancyFeed job)
        {
            LogHelper.Info(GetType(), "Delete job: " + job.VacancyPosting.Vacancy.JobReference);
        }

        private void SyncSingle(VacancyFeed job)
        {
            LogHelper.Info(GetType(), "Sync job: " + job.VacancyPosting.Vacancy.JobReference);
        }

        private void CheckAgaintUmbracoNodes(IEnumerable<VacancyFeed> vacancies)
        {
            try
            {
                var eventsRoot = GetJobsRoot();
                var allDecendants = eventsRoot.Descendants().Where(d => d.HasProperty("jobReference")).ToList();

                foreach (var vacancy in vacancies)
                {
                    var productionNode = allDecendants.SingleOrDefault(d => d.GetValue<string>("jobReference") == vacancy.VacancyPosting.Vacancy.JobReference);

                    if (productionNode == null)
                    {
                        vacancy.New = true;
                    }
                    else
                    {
                        vacancy.New = false;
                        //vacancy.Published = productionNode.Published;
                        //vacancy.UmbracoEventId = productionNode.Id;
                        vacancy.Dirty = vacancy.FingerPrint != productionNode.GetValue<string>("fingerprint");
                        vacancy.Delete = vacancy.VacancyPosting.Vacancy.AdvertStatus.Equals("Delete",
                            StringComparison.OrdinalIgnoreCase);
                    }
                }
            }
            catch (Exception)
            {
                throw new DuplicateNameException("Two productions have the same Voyager Job Reference. Please check your productions.");
            }
        }

        private static void CheckVoyagerProperties(IPublishedContent jobsRoot)
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

        //public IContent CreateSingle(VacancyFeed vacancy)
        //{
        //    var newNode = _contentService.CreateContent(vacancy.VacancyPosting.Vacancy.JobTitle.Trim(), Data.Constants.JobsRootId, "job");

        //    SetVoyagerProperties(newNode, vacancy);

        //   _contentService.SaveAndPublishWithStatus(newNode);

        //    return newNode;
        //}

        private static void SetVoyagerProperties(IContent newNode, VacancyFeed vacancy)
        {

            newNode.Name = vacancy.VacancyPosting.Vacancy.JobTitle.Trim();
            newNode.SetValue("title", vacancy.VacancyPosting.Vacancy.JobTitle.Trim());
            //newNode.SetValue("duration", production.Duration.ToString());
            //newNode.SetValue("prices", production.Pricing.FormattedPrices);
            //newNode.SetValue("venue", venues.Trim());
            //newNode.SetValue("multidate", json);
            //newNode.SetValue("patronBaseId", production.ProductionId);
            //newNode.SetValue("fingerprint", production.FingerPrint);
        }

        private static IPublishedContent GetPublishedJobsRoot()
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            IPublishedContent eventsRoot;

            try
            {
                eventsRoot = umbracoHelper.TypedContent(Data.Constants.JobsRootId).Descendant("job");
            }
            catch (Exception)
            {
                throw new Exception("Could not find an Jobs node in root");
            }

            if (eventsRoot == null)
            {
                throw new Exception("Could not find an Jobs node in root");
            }

            return eventsRoot;
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

                if (job != null)
                {
                    _jobs.Add(job);
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
