using System;
using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.PlayerMovement;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset playerMoveInputActionAsset;

        private Fighter _fighter;
        private NavMeshAgentMover _navMeshAgentMover;
        private Ray _lastRay;
        private InputAction _leftClickAction;
        private InputActionMap _playerMoveActionMap;
        private Health _health;

        private void Awake()
        {
            _playerMoveActionMap = playerMoveInputActionAsset.FindActionMap("PlayerPrimaryActions");
            _leftClickAction = _playerMoveActionMap.FindAction("PrimaryButton");
            _navMeshAgentMover = GetComponent<NavMeshAgentMover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }
        
        private void OnEnable()
        {
            _leftClickAction.Enable();
        }

        private void Update()
        {
            if (_health.HasDied) return;
            
            if (_leftClickAction.phase == InputActionPhase.Performed)
            {
                PerformRaycasts();
            }
        }

        private void PerformRaycasts()
        {
            if (CheckForCombatTarget())
            {
                return;
            }

            if (CanMoveToCursor())
            {
                return;
            }
            
            print("Nothing to do");
        }

        private bool CanMoveToCursor()
        {
            if (Physics.Raycast(GetMouseRay(), out var hit))
            {
                // 1 is not a magic number, always want the player to move at max speed
                _navMeshAgentMover.StartMoveAction(hit.point, 1f);
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        }

        private bool CheckForCombatTarget()
        {
            var raycastHits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in raycastHits)
            {
                var combatTarget = hit.transform.GetComponent<CombatTarget>();
                
                if(combatTarget == null) continue;
                
                if (!_fighter.CanAttack(combatTarget.gameObject))
                {
                    continue;
                }

                _fighter.Attack(combatTarget.gameObject);
                return true;
            }

            return false;
        }

        private void OnDisable()
        {
            _leftClickAction.Disable();
        }
    }
}