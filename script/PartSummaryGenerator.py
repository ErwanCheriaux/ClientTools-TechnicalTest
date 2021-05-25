#!/usr/bin/python
import sys
import random
from essential_generators import DocumentGenerator

gen = DocumentGenerator()
N = int(sys.argv[1]) if (len(sys.argv) > 1 and sys.argv[1].isdigit()) else 10

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
documents = gen.documents(N)

print(str(documents))
