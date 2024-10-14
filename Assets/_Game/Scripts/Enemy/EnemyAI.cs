using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
	[SerializeField] private NavMeshAgent _agent;
	[SerializeField] private float _chaseDelay = 0.1f;
	[SerializeField] private int _atackDamage = 10;
	[SerializeField] private float _attackDistance = 8.0f;
    [SerializeField] private float _attackDelay = 1.5f;
	[SerializeField] private Vector3 _rayOffset = new(0.0f, 5.0f, 0.0f);
    [SerializeField] private LayerMask _possibleRayMask;
    private bool _isAttacking = false;

	/// <summary>
	/// Задаёт <c>this.transform<c> позицию и поворот, которые также присваиваются симуляции агента <c>NavMeshAgent<c>.
	/// Оставляет <c>updatePosition = false<c>
	/// </summary>
	public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
	{
		_agent.updatePosition = true;
		transform.SetPositionAndRotation(position, rotation);	
		_agent.updatePosition = false;	
	}

	private void Start()
	{
		_agent.updatePosition = false;		
	}

	private void OnEnable()
	{
		// _agent.updateRotation = false;
		var target = ServiceLocator.Current.Get<PlayerDataManager>().PlayerObject;

		if (!target)
		{
			Debug.Log($"Make sure your player is in {typeof(PlayerDataManager).Name}");
		}
		else
		{
			StartCoroutine(FightTarget(target));
		}
	}

	private void OnDisable()
	{
		// StopCoroutine($"{nameof(ChaseWithTimer)}");
		StopAllCoroutines();
		_isAttacking = false;
	}

	private void FixedUpdate()
	{
		transform.position = _agent.nextPosition;
	}

	private void OnDrawGizmos()
	{
		for (var i = 0; i < 7; i++)
		{
			Gizmos.DrawLine(transform.position + _rayOffset, 
				transform.position + _rayOffset + Quaternion.Euler(0, i * -30, 0) * transform.right * _attackDistance);
		}
	}

	private IEnumerator FightTarget(GameObject target)
	{
		while (target != null)
		{
            if (!_isAttacking)
            {
                _agent.SetDestination(target.transform.position);
            }
			// Physics.Raycast(transform.position + _rayOffset, transform.forward, 
			//	out var hitInfo, _attackDistance, _possibleRayMask, QueryTriggerInteraction.Ignore)
			if (!_isAttacking && FanRaycast(out var hitInfo))
            {
                if (hitInfo.collider == target.GetComponent<Collider>() && hitInfo.collider.TryGetComponent<CharacteristicsController>(out var controller))
                	StartCoroutine(AtackTarget(controller));
            }

			yield return new WaitForSeconds(_chaseDelay);
		}
	}

	private bool FanRaycast(out RaycastHit hit)
	{
		for (var i = 0; i < 7; i++)
		{
			var ray = new Ray(transform.position + _rayOffset, Quaternion.Euler(0, i * -30, 0) * transform.right);
			if (Physics.Raycast(ray, out var hitInfo, _attackDistance, _possibleRayMask, QueryTriggerInteraction.Ignore))
			{
				hit =  hitInfo;
				return true;	
			}
		}

		hit = default;
        return false;
	}
	
	private IEnumerator AtackTarget(IDamageable target)
    {
        _isAttacking = true;
        _agent.isStopped = true;

        target.TakeDamage(_atackDamage);

        yield return new WaitForSeconds(_attackDelay);

        _agent.isStopped = false;
        _isAttacking = false;
    }
}