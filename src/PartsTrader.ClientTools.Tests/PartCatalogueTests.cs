using NUnit.Framework;
using PartsTrader.ClientTools.Api;
using PartsTrader.ClientTools.Api.Data;
using System.Collections.Generic;
using System.Linq;

namespace PartsTrader.ClientTools.Tests
{
    /// <summary>
    /// Tests for <see cref="PartCatalogue" />.
    /// </summary>
    [TestFixture]
    public class PartCatalogueTests
    {
        [TestCase]
        public void GetCompatibleParts_CorrectPartNumber_Pass()
        {
            PartCatalogue partCatalogue = new PartCatalogue();
            partCatalogue.GetCompatibleParts("1234-test");
            Assert.Pass();
        }

        [TestCase]
        public void GetCompatibleParts_CorrectPartNumberLongPartCode_Pass()
        {
            PartCatalogue partCatalogue = new PartCatalogue();
            partCatalogue.GetCompatibleParts("1234-aBcDeE12345");
            Assert.Pass();
        }

        [TestCase]
        public void GetCompatibleParts_PartIdTooShort_ThrowsInvalidPartException()
        {
            PartCatalogue partCatalogue = new PartCatalogue();
            Assert.Throws<InvalidPartException>(() =>
            {
                partCatalogue.GetCompatibleParts("123-abcd");
            });
        }

        [TestCase]
        public void GetCompatibleParts_PartCodeTooShort_ThrowsInvalidPartException()
        {
            PartCatalogue partCatalogue = new PartCatalogue();
            Assert.Throws<InvalidPartException>(() =>
            {
                partCatalogue.GetCompatibleParts("123-abc");
            });
        }

        [TestCase]
        public void GetCompatibleParts_PartIdNonDigit_ThrowsInvalidPartException()
        {
            PartCatalogue partCatalogue = new PartCatalogue();
            Assert.Throws<InvalidPartException>(() =>
            {
                partCatalogue.GetCompatibleParts("abcd-abcd");
            });
        }

        [TestCase]
        public void GetCompatibleParts_PartCodeNonAlphanumeric_ThrowsInvalidPartException()
        {
            PartCatalogue partCatalogue = new PartCatalogue();
            Assert.Throws<InvalidPartException>(() =>
            {
                partCatalogue.GetCompatibleParts("abcd-a+b*");
            });
        }

        [TestCase]
        public void GetCompatibleParts_PartNumberWithoutDash_ThrowsInvalidPartException()
        {
            PartCatalogue partCatalogue = new PartCatalogue();
            Assert.Throws<InvalidPartException>(() =>
            {
                partCatalogue.GetCompatibleParts("1234abcd");
            });
        }

        [TestCase]
        public void GetCompatibleParts_PartNumberInTheExclusionListWithDifferentCase_Empty()
        {
            PartCatalogue partCatalogue = new PartCatalogue();
            Assert.IsEmpty(partCatalogue.GetCompatibleParts("1234-ABCD"));
        }

        [TestCase]
        public void GetCompatibleParts_PartNumberInTheDataList_FindCompatibleParts()
        {
            //Arrange
            string partNumber = "5772-MetaDATA";
            PartCatalogue partCatalogue = new PartCatalogue();

            //Act
            IEnumerable<PartSummary> compatibleParts = partCatalogue.GetCompatibleParts(partNumber);
            PartSummary compatiblePart = compatibleParts.First(x => x.PartNumber == partNumber);

            //Assert
            Assert.AreEqual(partNumber, compatiblePart.PartNumber);
        }

        [TestCase]
        public void GetCompatibleParts_PartNumberInTheDataList_FindFourCompatibleParts()
        {
            //Arrange
            string partNumber = "5772-MetaDATA";
            PartCatalogue partCatalogue = new PartCatalogue();

            //Act
            IEnumerable<PartSummary> compatibleParts = partCatalogue.GetCompatibleParts(partNumber);

            //Assert
            Assert.AreEqual(4, compatibleParts.Count<PartSummary>());
        }
    }
}