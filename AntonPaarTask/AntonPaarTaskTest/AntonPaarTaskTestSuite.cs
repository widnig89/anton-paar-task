using AntonPaarTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AntonPaarTaskTest
{
    [TestClass]
    public class AntonPaarTaskTest
    {
        [TestMethod]
        public void TestWordOccurences()
        {
            DataLayer.dataLayer.AddWord("hello");
            DataLayer.dataLayer.AddWord("world");
            DataLayer.dataLayer.AddWord("hallo");
            DataLayer.dataLayer.AddWord("world");
            DataLayer.dataLayer.AddWord("world");
            DataLayer.dataLayer.AddWord("hallo");

            var wordOccurenceList = DataLayer.dataLayer.GetOrderedWordOccurrencePairs();
            Assert.AreEqual(("world", 3), (wordOccurenceList.ElementAt(0).Key, wordOccurenceList.ElementAt(0).Value));
            Assert.AreEqual(("hallo", 2), (wordOccurenceList.ElementAt(1).Key, wordOccurenceList.ElementAt(1).Value));
            Assert.AreEqual(("hello", 1), (wordOccurenceList.ElementAt(2).Key, wordOccurenceList.ElementAt(2).Value));
        }
    }
}
