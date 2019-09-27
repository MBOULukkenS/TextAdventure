using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SynonymDict
{
    /// <summary>
    /// Dit is de implementatie van een synoniemenboek.
    /// </summary>
    public class SynonymDict : MonoBehaviour
    {
        [SerializeField]
        private SynonymConfig[] _synonymConfigs;

        /// <summary>
        /// Deze functie zoekt alle synoniemen voor 'word'
        /// </summary>
        /// <param name="word">Het woord waar synoniemen voor gevonden moeten worden</param>
        /// <returns>alle gevonden synoniemen voor 'word'</returns>
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

        /// <summary>
        /// Deze functie controleerd of dat 'synonym' een synoniem is voor 'word'.
        /// </summary>
        /// <param name="synonym">Het mogelijke synoniem</param>
        /// <param name="word">Het woord waar dat 'synonym' een synoniem voor moet zijn</param>
        /// <returns>Of een woord een synoniem is voor 'word'</returns>
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