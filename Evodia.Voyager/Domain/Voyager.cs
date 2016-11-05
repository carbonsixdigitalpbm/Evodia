using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Evodia.Voyager.Domain.VoyagerObjects;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;

namespace Evodia.Voyager.Domain
{
    public class Voyager
    {
        private readonly IContentService _contentService;

        private List<VacancyFeed> _jobs;

        private readonly VoyagerApi _api;

        public Voyager(IContentService contentService)
        {
            _contentService = contentService;
            _jobs = new List<VacancyFeed>();
            _api = VoyagerApi.Instance();
        }

        public void Fetch()
        {
            var timer = Stopwatch.StartNew();

            var fileNames = _api.GetXmlFileNames();

            foreach (var fileName in fileNames)
            {
                ReadAndDeserializeXmlFile(fileName);
            }

            timer.Stop();
            var timespan = timer.Elapsed;

            LogHelper.Info(GetType(), "TIME TO READ AND DESERIALIZE " + _jobs.Count + " : " + timespan.Milliseconds + " ms");
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
