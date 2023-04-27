using System;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentMover : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform target;

    private NavMeshAgent _playerAgent;
    #endregion
    
    #region UnityMethods

    private void Start()
    {
        _playerAgent = GetComponent<NavMeshAgent>();
        _playerAgent.SetDestination(target.position);
    }

    #endregion
}
