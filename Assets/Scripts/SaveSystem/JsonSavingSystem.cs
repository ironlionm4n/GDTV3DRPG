using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SaveSystem
{
    public class JsonSavingSystem : MonoBehaviour
    {
        private const string extension = ".json";
        
        /// <summary>
        /// Will load the last scene that was saved and restore the state. This
        /// must be run as a coroutine.
        /// </summary>
        /// <param name="saveFile">The save file to consult for loading.</param>
        public IEnumerable<AsyncOperation> LoadLastScene(string saveFile)
        {
            JObject state = LoadJsonFromFile(saveFile);
            IDictionary<string, JToken> stateDict = state; 
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            var found = stateDict.ContainsKey("lastSceneBuildIndex");
            Debug.Log("Found "+found);
            if (found)
            {
                buildIndex = (int)stateDict["lastSceneBuildIndex"];
            }
            Debug.Log("Build Index "+buildIndex);
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreFromToken(state);
        }

        /// <summary>
        /// Save the current scene to the provided save file.
        /// </summary>
        public void Save(string saveFile)
        {
            JObject state = LoadJsonFromFile(saveFile);
            CaptureAsToken(state);
            SaveFileAsJSon(saveFile, state);
        }

        /// <summary>
        /// Delete the state in the given save file.
        /// </summary>
        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        public void Load(string saveFile)
        {
            RestoreFromToken(LoadJsonFromFile(saveFile));
        }

        public IEnumerable<string> ListSaves()
        {
            foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if (Path.GetExtension(path) == extension)
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }

        // PRIVATE

        private JObject LoadJsonFromFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new JObject();
            }
            
            using (var textReader = File.OpenText(path))
            {
                using (var reader = new JsonTextReader(textReader))
                {
                    reader.FloatParseHandling = FloatParseHandling.Double;

                    return JObject.Load(reader);
                }
            }

        }

        private void SaveFileAsJSon(string saveFile, JObject state)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (var textWriter = File.CreateText(path))
            {
                using (var writer = new JsonTextWriter(textWriter))
                {
                    writer.Formatting = Formatting.Indented;
                    state.WriteTo(writer);
                }
            }
        }


        private void CaptureAsToken(JObject state)
        {
            IDictionary<string, JToken> stateDict = state;
            foreach (JsonSavableEntity saveable in FindObjectsOfType<JsonSavableEntity>())
            {
                stateDict[saveable.GetUniqueIdentifier()] = saveable.CaptureAsJToken();
            }

            stateDict["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }


        private void RestoreFromToken(JObject state)
        {
            IDictionary<string, JToken> stateDict = state;
            foreach (JsonSavableEntity savable in FindObjectsOfType<JsonSavableEntity>())
            {
                string id = savable.GetUniqueIdentifier();
                if (stateDict.ContainsKey(id))
                {
                    savable.RestoreFromJToken(stateDict[id]);
                }
            }
        }


        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + extension);
        }

    }
}