using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class LiveObjectAI : MonoBehaviour
{

    [SerializeField, Range(0, 20)] private float _wanderRadius;
    [SerializeField, Range(0, 20)] private float _wanderTimer;
    [SerializeField] private Vector3 _coordPoint;
    [SerializeField] private float _minPos;
    [SerializeField] private float _maxPos;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private List<Light> _lights = new();
    private float _time;

    [SerializeField] private int _energy = 100;
    [SerializeField] private Transform _bed;
    [SerializeField] private Vector3 _sleepOffsetTransform = new(-10, 2f, 0);
    [SerializeField] private Vector3 _sleepOffsetRotation = new(-90, 180, 90);
    [SerializeField] private bool _sleepState = false;
    [SerializeField] private bool _isGoingToSleep = false;
    [SerializeField] private bool _isSleeping = false;

    public delegate void MovementLogic();
    public event MovementLogic ExecuteMovement;

    public delegate void OnCollisionLogic(Collision other);
    public event OnCollisionLogic ExecuteCollision;

    // Use this for initialization
    private void OnEnable()
    {
        if (_agent.enabled && !_agent.isOnNavMesh)
        {
            var position = transform.position;
            NavMesh.SamplePosition(position, out NavMeshHit hit, 10.0f, -1);
            position = hit.position; // usually this barely changes, if at all
            _agent.Warp(position);
        }
    }
    private void OnDisable()
    {
        ExecuteMovement = null;
        ExecuteCollision = null;
    }

    private void Start()
    {
        _time = _wanderTimer;
        ExecuteMovement += SetWalkDestination;
        ExecuteCollision += EatItem;
    }
    // Update is called once per frame
    private void Update()
    {
        _time += Time.deltaTime;
        ExecuteMovement?.Invoke();
        SetState();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ExecuteCollision?.Invoke(collision);
    }    

    private void SetState()
    {
        if (!_sleepState)
            if (_energy <= 0 || _energy > 100)
            {
                _sleepState = true;
                ExecuteMovement -= SetWalkDestination;
                ExecuteCollision -= EatItem;

                ExecuteMovement += GoSleep;
                ExecuteCollision += GetOnTheBed;
            }
    }

    private void SetWalkDestination()
    {
        if (_time >= _wanderTimer)
        {
            _energy -= 5;
            Vector3 newPos = RandomNavSphere(transform.position, _wanderRadius, -1);
            _agent.SetDestination(newPos);
            _time = 0;
        }
    }

    private void EatItem(Collision other)
    {
        if(other.gameObject.TryGetComponent(out ChargingItem item))
        {
            // Destroy(other.gameObject);
            item.TakeItem();
            _energy += 10;
        }
    }

    private void GoSleep()
    {
        if (!_isGoingToSleep)
        {
            _agent.SetDestination(_bed.position);
            _isGoingToSleep = true;
        }
    }

    private void GetOnTheBed(Collision other)
    {
        if (!_isSleeping)
            if (other.transform == _bed)
            {
                _agent.enabled = false;
                GetComponent<Collider>().enabled = false;
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.useGravity = false;
                foreach(var light in _lights)
                    light.enabled = false;
                transform.SetPositionAndRotation(_bed.position + _sleepOffsetTransform,
                    Quaternion.Euler(_bed.rotation.eulerAngles + _sleepOffsetRotation));
                rb.linearVelocity = Vector3.zero;
                _isSleeping = true;
            }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = _coordPoint +
            new Vector3(UnityEngine.Random.Range(_minPos, _maxPos),
                        origin.y, UnityEngine.Random.Range(_minPos, _maxPos));

        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask);

        return navHit.position;
    }
}