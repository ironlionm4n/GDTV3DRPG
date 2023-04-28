using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NavMeshAgentMover : MonoBehaviour
{
    #region Variables

    [SerializeField] private InputActionAsset playerMoveInputActionAsset;
    [SerializeField] private Animator playerAnimator;
    private NavMeshAgent _playerAgent;
    private Ray _lastRay;
    private InputAction _leftClickAction;
    private InputActionMap _playerMoveActionMap;
    private static readonly int MovementSpeed = Animator.StringToHash("MoveSpeed");

    #endregion

    #region UnityMethods

    private void Awake()
    {
        _playerMoveActionMap = playerMoveInputActionAsset.FindActionMap("PlayerMovement");
        _leftClickAction = _playerMoveActionMap.FindAction("ClickToMove");
        _playerAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        _leftClickAction.Enable();
    }

    private void Start()
    {
   
    }

    private void Update()
    {
        Debug.Log(_leftClickAction.phase);
        if (_leftClickAction.phase == InputActionPhase.Performed)
        {
            if (Camera.main != null) MoveToCursor();
        }
        
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        // Convert from global to local because the NavMeshAgent velocity is in world space,
        // Animator parameter needs the local velocity to blend correctly
        var localVelocity = transform.InverseTransformDirection(_playerAgent.velocity);
        playerAnimator.SetFloat(MovementSpeed, localVelocity.z);
    }

    private void OnDisable()
    {
        _leftClickAction.Disable();
    }

    #endregion

    #region CustomFunctions
    
    private void MoveToCursor()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit)) _playerAgent.SetDestination(raycastHit.point);
    }

    #endregion
}