using UnityEngine;
using RPG.Stats;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private ProgressionData progressionData;

        public float GetStat(Stats stat)
        {
            return progressionData.GetStat(stat, characterClass, startingLevel);
        }
        
    }
}