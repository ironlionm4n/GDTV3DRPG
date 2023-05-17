using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private ProgressionData progressionData;

        public float GetStat(Stats stat)
        {
            return progressionData.GetStat(stat, characterClass, GetLevel());
        }

        private void Update()
        {
            if(gameObject.CompareTag("Player"))
                Debug.Log(GetLevel().ToString());
        }

        public int GetLevel()
        {
            var experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;
            
            var currentExp = experience.GetExperiencePoints;
            var penultimateLevel = progressionData.GetLevels(Stats.ExperienceToLevelUp, characterClass);
            for (int i = 1; i <= penultimateLevel; i++)
            {
                var expToLevelUp = progressionData.GetStat(Stats.ExperienceToLevelUp, characterClass, i);
                if (expToLevelUp > currentExp)
                {
                    return i;
                }
            }

            return penultimateLevel + 1;
        }
        
    }
}