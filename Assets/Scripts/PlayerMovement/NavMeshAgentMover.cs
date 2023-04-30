using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NavMeshAgentMover : MonoBehaviour
{
    #region Variables

    [SerializeField] private Animator playerAnimator;
    private NavMeshAgent _playerAgent;
    private static readonly int MovementSpeed = Animator.StringToHash("MoveSpeed");

    #endregion

    #region UnityMethods

    private void Awake()
    {
        _playerAgent = GetComponent<NavMeshAgent>();
    }



    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        // Convert from global to local because the NavMeshAgent velocity is in world space,
        // Animator parameter needs the local velocity to blend correctly
        var localVelocity = transform.InverseTransformDirection(_playerAgent.velocity);
        playerAnimator.SetFloat(MovementSpeed, localVelocity.z);
    }



    #endregion

    #region CustomFunctions
    
    public void MoveToCursor()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var raycastHit))
        {
            var point = raycastHit.point;
            SetPlayerDestination(point);
        }
    }

    private void SetPlayerDestination(Vector3 point)
    {
        _playerAgent.SetDestination(point);
    }

    #endregion
}