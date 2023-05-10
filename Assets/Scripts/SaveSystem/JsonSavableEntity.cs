using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.SaveSystem
{
    [ExecuteAlways]
    public class JsonSavableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";

        private static Dictionary<string, JsonSavableEntity> globalLookup = new();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public JToken CaptureAsJToken()
        {
            JObject state = new();
            IDictionary<string, JToken> stateDictionary = state;
            foreach (var jsonSavable in GetComponents<IJsonSavable>())
            {
                JToken token = jsonSavable.CaptureAsJToken();
                string component = jsonSavable.GetType().ToString();
                Debug.Log($"{name} Capture {component} = {token.ToString()}");
                stateDictionary[jsonSavable.GetType().ToString()] = token;
            }

            return state;
        }

        public void RestoreFromJToken(JToken token)
        {
            JObject state = token.ToObject<JObject>();
            IDictionary<string, JToken> stateDictionary = state;
            foreach (var jsonSavable in GetComponents<IJsonSavable>())
            {
                var component = jsonSavable.GetType().ToString();
                if (stateDictionary.ContainsKey(component))
                {
                    Debug.Log($"{name} Restore {component} => {stateDictionary[component].ToString()}");
                    jsonSavable.RestoreFromJToken(stateDictionary[component]);
                }
                
            }
        }
        
#if UNITY_EDITOR
        private void Update() {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate)) return true;

            if (globalLookup[candidate] == this) return true;

            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }

    }
}