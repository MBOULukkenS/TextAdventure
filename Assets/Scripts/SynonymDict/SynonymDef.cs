using System;

namespace SynonymDict
{
    /// <summary>
    /// Dit is een synoniemdefinitie, hierin wordt een synoniem beschreven.
    /// </summary>
    [Serializable]
    public class SynonymDef
    {
        public string Word;
        public string[] Synonyms;
    }
}