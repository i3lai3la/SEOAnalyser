using K.SEOAnalyser.Web.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace K.SEOAnalyser.Test
{
    [TestClass]
    public class CommonFunctionsTests
    {
        [TestMethod]
        public void IsValidUrlTest()
        {
            Assert.IsTrue(CommonFunctions.IsValidUrl("www.yahoo.com"));
            Assert.IsTrue(CommonFunctions.IsValidUrl("http://www.yahoo.com"));
            Assert.IsTrue(CommonFunctions.IsValidUrl("https://www.yahoo.com"));
            Assert.IsTrue(CommonFunctions.IsValidUrl("https://yahoo.com"));
            Assert.IsTrue(CommonFunctions.IsValidUrl("www.yahoo.com?param=test1"));
            Assert.IsTrue(CommonFunctions.IsValidUrl("https://www.yahoo.com/test/path"));
            Assert.IsFalse(CommonFunctions.IsValidUrl("https://www.yahoo.com/test/path asd"));
            Assert.IsFalse(CommonFunctions.IsValidUrl("113https://www.yahoo.com/test/path asd"));
        }

        [TestMethod]
        public void IsUrlResponsesTest()
        {
            Assert.IsTrue(CommonFunctions.IsUrlResponses("https://www.thestar.com.my/business/business-news/2018/09/24/a-gamechanger-model-for-proton/"));
            Assert.IsFalse(CommonFunctions.IsUrlResponses("www.https://www.testsdfsdfsdf.com.com"));
        }

        [TestMethod]
        public void GetPageWordsFromUrlTest()
        {
            Assert.IsNotNull("https://www.thestar.com.my/business/business-news/2018/09/24/a-gamechanger-model-for-proton/");
        }
    }
}
