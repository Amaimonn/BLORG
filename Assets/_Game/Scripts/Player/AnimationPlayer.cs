using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;

    private float _moveSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _moveSpeed = Vector3.ClampMagnitude(
            new Vector3(_rb.linearVelocity.x, 0.0f, _rb.linearVelocity.z), 10.0f).magnitude;
        _animator.SetFloat("speed", _moveSpeed > 0.01f ? _moveSpeed : 0);
    }
}
