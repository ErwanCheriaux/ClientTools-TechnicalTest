PartsTrader.ClientTools
===================================================================================================

Task Outline
---------------------------------------------------------------------------------------------------

The PartsTrader team are developing a set of tools for use in third party repairer estimating software. These tools will provide the ability for repairers to lookup a given part and find all compatible parts that could be used instead. These tools are to be provided to the clients in the form of a simple API that they can reference within their own solutions.

Background
---------------------------------------------------------------------------------------------------

PartsTrader maintains a large catalogue of vehicle parts providing mappings between various national and international standards. When repairers submit part information to PartsTrader, either when quoting or when simply looking up a part, PartsTrader will cache the part information (if it doesn't already know of the part) so as to be able to provide as full and complete a list of parts information as possible to other integrations.

When creating work estimates repairers create a list of the parts need to repair a vehicle; for example, a car may require a new front wheel and a side mirror in order to be road worthy again. In many scenarios car parts are interchangeable; for example, a rear tail light for a 2014 Chevrolet Camaro might be the officially required part for a repair however a Ford Focus 2008 tail light may be either the exact same part or a close enough replacement. In order to maximise the response to part quotes, and thus hopefully reduce costs, repairers want to be able to lookup the central PartsTrader catalogue of parts and retrieve a list of all parts that could be used instead.

In some estimating software, there is no distinction made between a part and a line item, as such an estimate may contain pseudo parts which incur a charge but should not be included in quote requests for parts. For example a repairer may include 1111-OilCheck on their list of parts to indicate that they will be charging for an oil check, however this should not be submitted to PartsTrader as it either contains repair shop operational specifics that PartsTrader shouldn't know about, or it is data that PartsTrader should not be storing in the central parts repository (as this is available to all integrated repairers). In order to prevent non-standard parts being provided to PartsTrader each repairer can maintain their own exclusions list which contains a set of parts that should not be sent through to PartsTrader; it is important that our client tools use this list to exclude parts that shouldn't be uploaded.

Task
---------------------------------------------------------------------------------------------------

A skeleton of the client tools project has been created containing the API exposed to the client and a stub implementation. Full requirements for the integrated PartsTrader Parts Service are not known at present so a simple interface has been provided to abstract the parts lookup. A blank unit test class has been provided for you to expand upon. Your task is to complete the implementation such that it meets the following requirements.

### Requirement 1 - Validate Part Number

When given a part number the client tools should validate it to ensure that it conforms to the following specification:

    partNumber = partId "-" partCode;
    partId     = {4 * digit};
    partCode   = {4 * alphanumeric}, {alphanumeric};

That is a part id comprising of 4 digits, followed by a dash (-), followed by a part code consisting of 4 or more alphanumeric characters. So, 1234-abcd, 1234-a1b2c3d4 would be valid, a234-abcd, 123-abcd would be invalid. Where an invalid number is found an invalid part exception should be thrown.

### Requirement 2 - Check Exclusions List

Valid part numbers should be checked against the local exclusions list to determine whether the part should be supplied to PartsTrader or not. If the given part number is found in the list then the part should not be sent to PartsTrader; in this scenario, the lookup should return an empty collection.

### Requirement 3 - Lookup Compatible Parts

If a valid part is supplied that is not on the exclusions list then it should be looked up via the PartsTrader Parts Service (represented by the IPartsTraderPartsService interface) in order to retrieve any compatible parts. The results of this lookup should be returned.

Assumptions
---------------------------------------------------------------------------------------------------

The following assumptions have been made in the provided framework - you are free to countermand these as you feel is appropriate to your design.

1. The exclusions list is a JSON file currently stored in the file /Exclusions.json (relative to the current execution directory).
2. Part numbers are not case sensitive.
3. It is sufficient to mock IPartsTraderPartsService for the purpose of testing.

Constraints
---------------------------------------------------------------------------------------------------

There are no constraints to your implemented solution, we have provided a skeleton framework to extend from however you are free to modify, remove, or ignore these according to what you perceive to be the best solution.
Use whatever tools you are comfortable with, if you don't have access to anything we recommend you use Visual Studio 2019 Community Edition (https://visualstudio.microsoft.com/vs/). It's free and will allow you to complete the exercise.
We expect this exercise should take around 4 hours.

Comments
---------------------------------------------------------------------------------------------------

### Environment
__IDE__: Visual Studio 2017  
__Framwork__: .NET 4.5  
__Git repo__: https://Erwan_Cheriaux@bitbucket.org/Erwan_Cheriaux/clienttools-technicaltest.git  

### Tools
NUnit 3 Test Adapter v3.17.0  
CodeMaid v11.2  
GhostDoc v2020.2.20270  
Markdown Editor v1.12.253  

### Packages
NUnit v3.13.2  
Newtonsoft.Json v13.0.1  
Moq v4.16.1  
Linq v4.3.0  

### Requirement 1 - Validate Part Number

I created a method __ValidPartNumber(string value)__ in the class __PartCatalogue__. This private method is only used by the method __GetCompatibleParts(string partNumber)__ to throw an __InvalidPartException__ when the partNumber does not respect the specification.
If subsequently, this function need to be used by other classes to check a PartNumber, then the function could be moved into a helper class and set as public.
Since this methode is private, it can be tested by NUnit only through the public method __GetCompatibleParts(string partNumber)__ with __Assert.Throws<InvalidPartException>__ when the partNumber is not valid.

### Requirement 2 - Check Exclusions List

Once the partNumber is valid, the JSON file Exclusion is Deserialize with __Newtonsoft.Json__ and convert to a <IEnumerable<PartSummary\>\> object.
Note that I'm assuming Exclusion.json file exists and has valid content.
Then an empty PartSummary list is returned if the partNumber is found in the exclusion list, regardless of the partNumber case.

### Requirement 3 - Lookup Compatible Parts

It is straightforward to mock the interface IPartsTraderPartsService and use the method FindAllCompatibleParts with the __Moq__ library as follows:

```Csharp
using Moq;

Mock<IPartsTraderPartsService> _partsTraderpartsService = new Mock<IPartsTraderPartsService>();
_partsTraderpartsService.Setup(x => x.FindAllCompatibleParts(partNumber))
    .Returns(() => new List<PartSummary> { partSummaryDto });
```