using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptExApi.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptExTests.Validators
{
    [TestClass]
    public class IbanValidatorTests
    {
        private IbanValidator validator;

        [TestInitialize]
        public void Init()
        {
            validator = new IbanValidator();
        }

        [TestMethod]
        public void IsValid()
        {
            var ibans = new List<string>
            {
                "DE11 2222 2222 3333 3333 33",
                "SA11 2233 3333 3333 3333 33",
                "AT11 2222 2333 3333 3333",
                "BA11 2225 5533 3333 3311",
                "HR11 2222 2223 3333 3333 3",
                "DK11 2222 3333 3333 33",
                "ES11 2222 4444 1133 3333 3333",
                "FI11 2222 2233 3333 31",
                "FR11 2222 2444 4433 3333 3333 311", //France
                "GB11 2222 5555 5533 3333 33", // Great Britain
                "GR11 222 2222 3333 3333 3333 3333",
                "HU11 2224 4441 3333 3333 3333 3331",
                "IE11 6666 2222 2233 3333 33",
                "LI11 2222 2333 3333 3333 3",
                "LU11 2223 3333 3333 3333",
                "NO11 2222 33 33333",
                "PL11 2222 2221 3333 3333 3333 3333",
                "CZ11 2222 5555 5533 3333 3333",
                "RO11 2222 3333 3333 3333 3333",
                "SM11 A222 2233 333N 7777 7777 777",
                "SE11 2222 3333 3333 3333 3333",
                "CH90 1234 5678 1234 5678 1", //Switzerland
                "TN11 2222 2333 3333 3333 3333"
            };

            foreach (var iban in ibans)
                Assert.IsTrue(validator.IsValid(iban));
        }

        [TestMethod]
        public void IsInvalid()
        {
            var ibans = new List<string>
            {
                "GB96 BARC 2020 1", //Invalid length
                "0134 1234 5678 9012 3456 11", //Invalid IBAN
                "Invalid"
            };

            foreach (var iban in ibans)
                Assert.IsFalse(validator.IsValid(iban));
        }
    }
}
