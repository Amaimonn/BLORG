using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class NavAnimating : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    private float _moveSpeed;


    private void Update()
    {
        _moveSpeed = Vector3.ClampMagnitude(
            new Vector3(_agent.velocity.x, 0.0f, _agent.velocity.z), 10.0f).magnitude;
        _animator.SetFloat("speed", _moveSpeed > 0.01f ? _moveSpeed : 0);
    }
}
