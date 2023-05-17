using Newtonsoft.Json.Linq;
using RPG.SaveSystem;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, IJsonSavable
    {
        [SerializeField] private float experiencePoints = 0f;
        public float GetExperiencePoints => experiencePoints;
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(experiencePoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            experiencePoints = state.ToObject<float>();
        }
    }
}