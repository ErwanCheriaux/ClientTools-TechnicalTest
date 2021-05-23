namespace PartsTrader.ClientTools.Api.Data
{
    /// <summary>
    /// Provides summary details of a part.
    /// </summary>
    public class PartSummary
    {
        /// <summary>
        /// The part number.
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// The description of the part.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Return true if the partNumber has the same partId
        /// else if the partNumber has the same partCode
        /// else if the partCode is contained in the description of this PartSummary
        /// </summary>
        public bool IsCloseEnough(string partNumber)
        {
            string partId = partNumber.Split('-')[0];
            string partCode = partNumber.Split('-')[1].ToLower();

            if (partId == PartNumber.Split('-')[0]) return true;
            if (partCode == PartNumber.Split('-')[1].ToLower()) return true;
            if (Description.ToLower().Contains(partCode)) return true;

            return false;
        }
    }
}