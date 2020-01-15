using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseLibrary;

namespace StringParseLibraryTest
{
    [TestClass]
    public class StringParseTest
    {
        [TestMethod]
        public void TestWhitespaceLine()
        {
            string line = " \t ";
            string[] words = StringParseLibrary.GetWordsInString(line);
            Assert.IsNotNull(words);
            Assert.AreEqual(0, words.Length);
        }

        [TestMethod]
        public void TestSingleWord()
        {
            string line = "hello";
            string[] words = StringParseLibrary.GetWordsInString(line);
            Assert.AreEqual(1, words.Length);
            Assert.AreEqual(line, words[0]);
        }

        [TestMethod]
        public void TestManyWords()
        {
            string line = "hello world, here I am";
            string[] words = StringParseLibrary.GetWordsInString(line);
            Assert.AreEqual(5, words.Length);
            Assert.AreEqual("hello", words[0]);
            Assert.AreEqual("world,", words[1]);
            Assert.AreEqual("here", words[2]);
            Assert.AreEqual("I", words[3]);
            Assert.AreEqual("am", words[4]);
        }
    }
}
