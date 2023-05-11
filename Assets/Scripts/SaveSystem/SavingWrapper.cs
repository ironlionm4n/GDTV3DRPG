using System;
using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SaveSystem
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string currentSaveKey = "currentSaveName";
        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] int firstLevelBuildIndex = 1;
        [SerializeField] int menuLevelBuildIndex = 0;

        private void OnEnable()
        {
            StartCoroutine(LoadLastSceneImmediate());
        }

        public void ContinueGame() 
        {
            StartCoroutine(LoadLastScene());
        }

        public void NewGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }

        public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            var saveKey = PlayerPrefs.GetString(currentSaveKey);
            Debug.Log("Save Key " + saveKey);
            return saveKey;
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeIn(fadeInTime);
            yield return GetComponent<JsonSavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeOut(fadeOutTime);
        }
        
        private IEnumerator LoadLastSceneImmediate()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeInImmediate();
            var saveSystem = GetComponent<JsonSavingSystem>();
            Debug.Log(saveSystem);
            yield return saveSystem.LoadLastScene(GetCurrentSave());
            yield return fader.FadeOut(fadeOutTime);
        }

        private IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        private IEnumerator LoadMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(menuLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Load()
        {
            GetComponent<JsonSavingSystem>().Load(GetCurrentSave());
        }

        public void Save()
        {
            GetComponent<JsonSavingSystem>().Save(GetCurrentSave());
        }

        public void Delete()
        {
            GetComponent<JsonSavingSystem>().Delete(GetCurrentSave());
        }

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<JsonSavingSystem>().ListSaves();
        }

    }
}