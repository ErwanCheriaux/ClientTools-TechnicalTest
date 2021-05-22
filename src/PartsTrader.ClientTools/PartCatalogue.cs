using PartsTrader.ClientTools.Api;
using PartsTrader.ClientTools.Api.Data;
using System;
using System.Collections.Generic;

namespace PartsTrader.ClientTools
{
    public class PartCatalogue : IPartCatalogue
    {
        public IEnumerable<PartSummary> GetCompatibleParts(string partNumber)
        {
            ValidPartNumber(partNumber);

            throw new NotImplementedException();
        }

        /// <summary>Determines whether the partNumber valid the specification {digit * 4}-{alphanumeric * 4}{alphanumeric*}.</summary>
        /// <param name="value">partNumber value to check.</param>
        /// <exception cref="InvalidPartException">
        /// The specified value hasn't one dash only: '{partNumber}'.
        /// The specified value doesn't contain 4 digits before the dash: '{partNumber}'.
        /// The specified value doesn't contains digits only before the dash: '{partNumber}'.
        /// The specified value contains less than 4 alphanumeric char after the dash: '{partNumber}'.
        /// The specified value doesn't contains alphanumeric char only after the dash: '{partNumber}'.
        /// </exception>
        private void ValidPartNumber(string value)
        {
            string[] partNumber = value.Split('-');

            // check if there is only one dash between partId and partCode
            if (partNumber.Length != 2) throw new InvalidPartException($"The specified value hasn't one dash only: '{partNumber}'.");

            string partId = partNumber[0];
            string partCode = partNumber[1];

            //partId check
            if (partId.Length != 4) throw new InvalidPartException($"The specified value doesn't contain 4 digits before the dash: '{partNumber}'.");
            foreach (char c in partId)
            {
                if (!char.IsDigit(c)) throw new InvalidPartException($"The specified value doesn't contains digits only before the dash: '{partNumber}'.");
            }

            //partCode check
            if (partCode.Length < 4) throw new InvalidPartException($"The specified value contains less than 4 alphanumeric char after the dash: '{partNumber}'.");
            foreach (char c in partCode)
            {
                if (!char.IsLetterOrDigit(c)) throw new InvalidPartException($"The specified value doesn't contains alphanumeric char only after the dash: '{partNumber}'.");
            }
        }
    }
}