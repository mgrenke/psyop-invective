using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
    private Transform _target;
    private NavMeshAgent _agent;
    private ThirdPersonCharacter _character;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _character = gameObject.GetComponent<ThirdPersonCharacter>();
    }

    // Can be optimized with a coroutine
    private void Update()
    {
        if (_target != null)
        {
            _agent.SetDestination(_target.position);
            FaceTarget();
        }

        if (_agent.remainingDistance > _agent.stoppingDistance)
            _character.Move(_agent.desiredVelocity, false, false);
        else
            _character.Move(Vector3.zero, false, false);
    }

    private void FaceTarget()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        // Refactor hardcode
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void MoveToPoint(Vector3 point)
    {
        _agent.SetDestination(point);
    }
    
    public void FollowTarget(Interactable newTarget)
    {
        _agent.stoppingDistance = newTarget.radius * .5f;
        _agent.updateRotation = false;
        
        _target = newTarget.interactionTransform;
    }

    public void StopFollowingTarget()
    {
        _agent.stoppingDistance = .2f;
        _agent.updateRotation = true;
        
        _target = null;
    }
}