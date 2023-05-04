using System;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace RPG.Cinematics
{
    public class CinematicsControlInput : MonoBehaviour
    {
        [SerializeField] private InputActionAsset playerActionAsset;
        private PlayableDirector _playableDirector;
        private InputAction _playerInput;
        private GameObject _player;
        private const string PrimaryButton = "PrimaryButton";

        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            _playerInput = playerActionAsset.FindAction(PrimaryButton);
            _player = GameObject.FindGameObjectWithTag("Player");
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
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _playerInput.Disable();
            
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            // when cinematic ends enable control
            Debug.Log("Stopped, enabling control");
            _playerInput.Enable();
        }
    }
}