using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private ProgressionData progressionData;

        public float GetHealth()
        {
            return progressionData.GetHealth(characterClass, startingLevel);
        }

        public float GetExperienceReward()
        {
            return 10f;
        }
    }
}