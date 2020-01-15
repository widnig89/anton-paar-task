using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AntonPaarTask
{
    [Serializable()]
    public class WordOccurence : ISerializable
    {
        public string word;
        public int occurence;

        public WordOccurence()
        {
            word = null;
            occurence = 0;
        }

        public WordOccurence(SerializationInfo info, StreamingContext ctxt)
        {
            word = (String)info.GetValue("Word", typeof(string));
            occurence = (int)info.GetValue("Occurence", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Word", word);
            info.AddValue("Occurence", occurence);
        }
    }


    public class DataLayer
    {
        private IDictionary<string, int> wordCountDict = new Dictionary<string, int>();

        private static DataLayer instance;

        public static DataLayer dataLayer {
            get
            {
                if (DataLayer.instance == null)
                {
                    DataLayer.instance = new DataLayer();
                    return DataLayer.instance;
                } else
                {
                    return DataLayer.instance;
                }
            }
        }

        private DataLayer()
        {
            
        }

        public void ClearWordOccurrence()
        {
            this.wordCountDict.Clear();
        }

        public void AddWord(string word)
        {
            if (this.wordCountDict.TryGetValue(word, out int occurences))
            {
                this.wordCountDict[word] = occurences + 1;
            }
            else
            {
                this.wordCountDict[word] = 1;
            }
        }

        public IList GetOrderedWordCountList() {
            return (from row in this.wordCountDict orderby row.Value descending select new {row.Key, row.Value }).ToList();
        }

        public IEnumerable<KeyValuePair<string, int>> GetOrderedWordOccurrencePairs()
        {
            return from pair in this.wordCountDict orderby pair.Value descending select pair;
        }
    }
}
