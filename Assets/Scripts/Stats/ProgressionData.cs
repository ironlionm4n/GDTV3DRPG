using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Items/Stats/Progression", order = 0)]
    public class ProgressionData : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] progressionCharacterClass;

        Dictionary<CharacterClass, Dictionary<Stats, float[]>> _lookupTable;

 
        public float GetStat(Stats stat, CharacterClass characterClass, int level)
        {
            BuildLookUpTable();

            var statLevels = _lookupTable[characterClass][stat];
            if (statLevels.Length < level)
            {
                return 0;
            }

            return statLevels[level - 1];
        }

        public int GetLevels(Stats stat, CharacterClass characterClass)
        {
            return _lookupTable[characterClass][stat].Length;

        }
        
        private void BuildLookUpTable()
        {
            if (_lookupTable != null) return;

            _lookupTable = new();
            foreach (var characterClass in progressionCharacterClass)
            {
                var statLookupTable = new Dictionary<Stats, float[]>();

                foreach (var progressionStat in characterClass.GetStats)
                {
                    statLookupTable[progressionStat.GetProgressionStat] = progressionStat.GetProgressionLevels;
                }
                
                _lookupTable[characterClass.CharacterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass characterClass;
            public CharacterClass CharacterClass => characterClass;

            [SerializeField] private ProgressionStat[] stats;
            public ProgressionStat[] GetStats => stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            [SerializeField] private Stats stat;
            public Stats GetProgressionStat => stat;
            [SerializeField] float[] levels;
            public float[] GetProgressionLevels => levels;
        }
    }
}