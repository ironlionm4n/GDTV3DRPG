using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;

namespace RPG.Cinematics
{
    public class CinematicsControlInput : MonoBehaviour
    {
        private PlayableDirector _playableDirector;

        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }

        private void OnEnable()
        {
            _playableDirector.played += DisableControl;
            _playableDirector.stopped += EnableControl;
        }

        private void OnDisable()
        {
            _playableDirector.played -= DisableControl;
            _playableDirector.stopped -= EnableControl;
        }

        private void DisableControl(PlayableDirector playableDirector)
        {
            // when cinematic starts disable control
            Debug.Log("Started, disabling control");
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            // when cinematic ends enable control
            Debug.Log("Stopped, enabling control");
        }
    }
}