using CryptExApi.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptExTests.Utilities
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class StringUtilitiesTests
    {
        [TestMethod]
        public void Random()
        {
            var rand = StringUtilities.Random(2048, StringUtilities.AllowedChars.AllSpaces);

            Assert.IsTrue(rand.Any(c => StringUtilities.AlphabetMaj.Contains(c)));
            Assert.IsTrue(rand.Any(c => StringUtilities.AlphabetMin.Contains(c)));
            Assert.IsTrue(rand.Any(c => StringUtilities.Numbers.Contains(c)));
            Assert.IsTrue(rand.Any(c => StringUtilities.Space.Contains(c)));
            Assert.IsTrue(rand.Any(c => StringUtilities.SpecialChars.Contains(c)));
        }

        [TestMethod]
        public void ComputeHash()
        {
            var origString = "abcdef123";
            var hashedString = "9f4c121d60cf553ad8e1b73f6c02ad2689b454075712d1665f72685bc93044c2";

            Assert.AreEqual(hashedString, StringUtilities.ComputeHash(origString, System.Security.Cryptography.HashAlgorithmName.SHA256));
        }
    }
}
