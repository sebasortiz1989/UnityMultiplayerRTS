using Mirror;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] NavMeshAgent _agent = null;
    [SerializeField] Targeter _targeter = null;
    [SerializeField] float chaseRange = 10f;

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    [ServerCallback]
    private void Update()
    {
        Targetable target = _targeter.GetTarget();

        if (target != null)
        {
            if ((target.transform.position - transform.position).sqrMagnitude > chaseRange * chaseRange) // This is more efficient than distance
            {
                _agent.SetDestination(target.transform.position);
            }
            else if (_agent.hasPath)
            {
                _agent.ResetPath();
            }

            return;
        }

        if (!_agent.hasPath) return;

        if (_agent.remainingDistance > _agent.stoppingDistance) return;

        _agent.ResetPath();
    }

    [Command]
    public void CmdMove(Vector3 _destination)
    {
        ServerMove(_destination);
    }

    [Server]
    private void ServerHandleGameOver()
    {
        _agent.ResetPath();
    }

    [Server]
    public void ServerMove(Vector3 _destination)
    {
        _targeter.ClearTarget();

        if (!NavMesh.SamplePosition(_destination, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; } //If is not a valid position

        _agent.SetDestination(hit.position);
    }
}
