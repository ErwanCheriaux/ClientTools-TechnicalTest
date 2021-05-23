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
__Github__: https://github.com/ErwanCheriaux/ClientTools-TechnicalTest  

Note that I initially created a git repo on __Bitbucket__ which I then migrated to __Github__ to be able to set up a CI / CD.
Indeed, I faced trouble to setup and restore .net dependencies with the __Bitbucket pipeline__.
The __Github action__ builds and runs all NUnit tests every push or merge on the master branch.

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
ReferenceAssemblies.net45 v1.0.2  

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

### Requirement 4 - Bonus

I created an new __Issue__ on __Github__ to generate user data to provide with the method FindAllCompatibleParts.
Create an inheriting class from IPartsTraderPartsService to look up via the PartsTrader Parts Service.
Then FindAllCompatibleParts will return all partSummary with the same partId, partCode or description then the given partNumber.

I used a Python script and the __Essential Document Generator__ library to generate user data in Json format.
Note that this data could be organize in a simple database table to make recovery efficient.
```python
#!/usr/bin/python
import random
from essential_generators import DocumentGenerator

gen = DocumentGenerator()

def gen_partNumber():
    partId = random.randrange(1000, 9999)
    partCode = ''
    while not (len(partCode) >= 4 and partCode.isalnum()):
        partCode = gen.word()
    partNumber = str(partId) + str('-') + str(partCode)
    return str(partNumber)

template = {
    'PartNumber': gen_partNumber,
    'Description': 'sentence'
}

gen.set_template(template)
documents = gen.documents(100)

print(documents)
```

Then the Data.json can easily be generated with the following bash command:
```bash
python PartSummaryGenerator.py >> Data.json
```

Finally, I created a class PartsTraderPartsService inheriting from IPartsTraderPartsService looking for close enough data in the system regarding the partNumber input.


### Improvement

The PartsTrader.ClientTools library could be used by a windows form app or a web app to have a human interface device.
The PartsTrader.ClientTools library could provide Rest API tools.
I started a __Swagger__ project to document this __Rest API__, you can find this public project with the below link:
https://app.swaggerhub.com/apis/erwanCheriaux/PartsTrader.ClientTools.Api/1.0.0#/PartSummary

Example
https://virtserver.swaggerhub.com/erwanCheriaux/PartsTrader.ClientTools.Api/1.0.0/PartNumber?PartNumber=1234-example

Returns
```json
[
  {
    "PartNumber": "1234-example",
    "Description": "This is a description"
  }
]
```

#### Thank you for reading, Erwan.