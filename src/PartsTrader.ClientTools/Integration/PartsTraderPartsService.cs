using Newtonsoft.Json;
using PartsTrader.ClientTools.Api.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace PartsTrader.ClientTools.Integration
{
    internal class PartsTraderPartsService : IPartsTraderPartsService
    {
        public IEnumerable<PartSummary> FindAllCompatibleParts(string partNumber)
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data.json");
            string jsonText = File.ReadAllText(jsonPath);

            List<PartSummary> partSummaryList = new List<PartSummary> { };
            IEnumerable<PartSummary> partSummaryDataList = JsonConvert.DeserializeObject<IEnumerable<PartSummary>>(jsonText);

            foreach (PartSummary partSummary in partSummaryDataList)
            {
                if (partSummary.IsCloseEnough(partNumber)) partSummaryList.Add(partSummary);
            }

            return partSummaryList;
        }
    }
}