using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Items/Stats/Progression", order = 0)]
    public class ProgressionData : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] progressionCharacterClass;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            var returnedHealth = 0f;
            foreach (var @class in progressionCharacterClass)
            {
                if (@class.CharacterClass == characterClass)
                {
                    returnedHealth = @class.HealthPointsAtLevelX(level - 1);
                }
            }

            return returnedHealth;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass characterClass;
            public CharacterClass CharacterClass => characterClass;

            [SerializeField] private float[] healthPoints;

            public float HealthPointsAtLevelX(int x)
            {
                return healthPoints[x];
            }
        }
    }
}