using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SceneLoadTransition());
        }
    }

    private IEnumerator SceneLoadTransition()
    {
        yield return SceneManager.LoadSceneAsync(sceneIndex);
        Destroy(gameObject);
    }
}
