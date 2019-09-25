using UnityEngine;

namespace DefaultNamespace.SynonymDict
{
    [CreateAssetMenu(fileName = "Synonym Config", menuName = "Synonym Configuration", order = 0)]
    public class SynonymConfig : ScriptableObject
    {
        public SynonymDef[] SynonymDefs;
    }
}