using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset playerMoveInputActionAsset;
        [SerializeField] private NavMeshAgentMover navMeshAgentMover;
        private Ray _lastRay;
        private InputAction _leftClickAction;
        private InputActionMap _playerMoveActionMap;

        private void Awake()
        {
            _playerMoveActionMap = playerMoveInputActionAsset.FindActionMap("PlayerPrimaryActions");
            _leftClickAction = _playerMoveActionMap.FindAction("PrimaryButton");
        }
        
        private void OnEnable()
        {
            _leftClickAction.Enable();
        }

        private void Update()
        {
            if (_leftClickAction.phase == InputActionPhase.Performed)
            {
                navMeshAgentMover.MoveToCursor();
            }
        }

        private void OnDisable()
        {
            _leftClickAction.Disable();
        }
    }
}