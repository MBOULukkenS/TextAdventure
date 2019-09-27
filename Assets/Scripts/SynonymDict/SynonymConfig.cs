using UnityEngine;

namespace SynonymDict
{
    /// <summary>
    /// Dit is het synoniem configuratie bestand.
    /// </summary>
    [CreateAssetMenu(fileName = "Synonym Config", menuName = "Synonym Configuration", order = 0)]
    public class SynonymConfig : ScriptableObject
    {
        public SynonymDef[] SynonymDefs;
    }
}