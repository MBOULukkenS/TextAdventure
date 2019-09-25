using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.SynonymDict
{
    public class SynonymDict : MonoBehaviour
    {
        [SerializeField]
        private SynonymConfig[] _synonymConfigs;

        public IEnumerable<string> GetSynonyms(string word)
        {
            List<string> synonyms = new List<string>();
            foreach (SynonymConfig config in _synonymConfigs)
            {
                try
                {
                    synonyms.AddRange(config.SynonymDefs
                                          .First(sDef => word
                                              .Equals(sDef.Word, StringComparison.CurrentCultureIgnoreCase))
                                          .Synonyms);
                }
                catch
                {
                    // ignored
                }
            }

            return synonyms.ToArray();
        }

        public bool IsSynonymFor(string synonym, string word)
        {
            try
            {
                return synonym.Equals(word, StringComparison.CurrentCultureIgnoreCase) 
                       || GetSynonyms(word)
                           .Select(s => s.ToLower())
                           .Contains(synonym.ToLower());
            }
            catch
            {
                return false;
            }
        }
    }
}