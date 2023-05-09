using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    enum DestinationIdentifier
    {
        A, B, C, D, E, F, G, H, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }

    [SerializeField] DestinationIdentifier destination;
    [SerializeField] private int sceneIndex;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fadeInTIme;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float fadeWaitTime;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SceneLoadTransition());
        }
    }

    private IEnumerator SceneLoadTransition()
    {
        DontDestroyOnLoad(gameObject);
        var fader = FindObjectOfType<Fader>();
        yield return fader.FadeIn(fadeInTIme);
        yield return SceneManager.LoadSceneAsync(sceneIndex);
        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);
        yield return new WaitForSeconds(fadeWaitTime);
        yield return fader.FadeOut(fadeOutTime);
        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        /*var otherSpawnPointTransform = otherPortal.spawnPoint.transform;
        player.transform.position = otherSpawnPointTransform.position;*/
        player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
        player.transform.rotation = otherPortal.spawnPoint.rotation;
    }

    private Portal GetOtherPortal()
    {
        foreach (var portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            if (portal.destination != destination) continue;
            
            return portal.GetComponent<Portal>();
        }

        return null;
    }
}
