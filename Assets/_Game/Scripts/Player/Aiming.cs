using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : MonoBehaviour
{
    public List<LimitedIndicator<float>> CurrentEnergyIndicators 
    {
        private get => _currentEnergyIndicators; 
        set => _currentEnergyIndicators = value;
    }

    [Header("Aim")]
    [SerializeField] private bool _isAiming;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private bool _ignoreHeight;
    [SerializeField] private Transform _aimedTransform;

    [Header("Laser")]
    [SerializeField] private LineRenderer _laserRenderer;
    [SerializeField] private LayerMask _laserMask;
    [SerializeField] private float _laserLength;

    [Header("Projectile")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _prefabSpawn;
    [SerializeField] private float _shotDelay = 1f;

    [Header("Gizmos")]
    [SerializeField] private bool _gizmo_cameraRay = false;
    [SerializeField] private bool _gizmo_ground = false;
    [SerializeField] private bool _gizmo_target = false;
    [SerializeField] private bool _gizmo_ignoredHeightTarget = false;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCameraBase _followCamera;
    [SerializeField] private CinemachineVirtualCameraBase _aimCamera;
    [SerializeField] private InputActionReference _xyCameraMove;
    [SerializeField] private float _xSpeed = 2.0f;
    [SerializeField] private float _ySpeed = 1.0f;
    [SerializeField] private GameObject _parent;

    private Camera _mainCamera;
    private bool _shootingMode = false;
    private List<LimitedIndicator<float>> _currentEnergyIndicators;
    private float _currentDelay = 0;

    private InputController.WizardActions _playerActions;
    private Action<InputAction.CallbackContext> _chargeAtack;
    private Action<InputAction.CallbackContext> _shoot;
    private Action<InputAction.CallbackContext> _cancelShooting;
    private Vector2 _aimInput;
    // private Action<InputAction.CallbackContext> _aimRotating;

    public void Initialize()
    {
        _mainCamera = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (_laserRenderer != null)
        {
            _laserRenderer.SetPositions(
                new Vector3[]{Vector3.zero, Vector3.zero}
            );
        }
    }

    private void Awake()
    {
        _chargeAtack = ctx  => ChargeAtack();
        _shoot = ctx => Shoot();
        _cancelShooting = ctx => SwapShootingMode(false);
        // _aimRotating = ctx => AimRotating(ctx.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        _currentDelay = 0;
        SubscribeInput();
    }

    private void OnDisable()
    {
        UnsubscribeInput();
        SwapShootingMode(false);
    }

    private void Update()
    {
        ReadAimInput();
        RefreshLaser();
    }

    private void FixedUpdate()
    {
        AimRotating(_aimInput);
    }

    private void SubscribeInput()
    {
        _playerActions = ServiceLocator.Current.Get<PlayerDataManager>().InputController.Wizard;
        _playerActions.Atack.started += _chargeAtack;
        _playerActions.Atack.canceled  += _shoot;
        _playerActions.CancelAtack.performed += _cancelShooting;
        // _xyCameraMove.action.performed += _aimRotating;
    }

    private void UnsubscribeInput()
    {
        _playerActions.Atack.started -= _chargeAtack;
        _playerActions.Atack.canceled  -= _shoot;
        _playerActions.CancelAtack.performed -= _cancelShooting;
        // _xyCameraMove.action.performed -= _aimRotating;
    }

    private void ReadAimInput()
    {
        _aimInput = _xyCameraMove.action.ReadValue<Vector2>();
    }

    private void AimRotating(Vector2 aimInput)
    {
        if (!_shootingMode)
            return;

        // var lookInput = context.ReadValue<Vector2>();
        // var lookInput = _xyCameraMove.action.ReadValue<Vector2>();
    
        _aimedTransform.Rotate(new Vector3(-aimInput.y * _ySpeed * Time.deltaTime, 0.0f, 0.0f));
        _parent.transform.Rotate(new Vector3(0.0f, aimInput.x * _xSpeed * Time.deltaTime, 0.0f));

        var xIncrement = -aimInput.y * _ySpeed * Time.deltaTime;
        var localAngles = _aimedTransform.localEulerAngles;

        if (localAngles.x > 180.0f)
        {
            localAngles.x -= 360.0f;
        }

        localAngles.x = Mathf.Clamp(localAngles.x + xIncrement, -85.0f, 85.0f);
        _aimedTransform.localEulerAngles = localAngles;
    }
    
    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
        {
            return;
        }

        var ray = new Ray(_prefabSpawn.position, GetDirection());
        if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, _groundMask, QueryTriggerInteraction.Ignore))
        {
            if (_gizmo_cameraRay)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(ray.origin, hitInfo.point);
                Gizmos.DrawWireSphere(ray.origin, 0.5f);
            }

            var hitPosition = hitInfo.point;
            var hitGroundHeight = Vector3.Scale(hitInfo.point, new Vector3(1, 0, 1)); ;
            var hitPositionIngoredHeight = new Vector3(hitInfo.point.x, _aimedTransform.position.y, hitInfo.point.z);

            if (_gizmo_ground)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(hitGroundHeight, 0.5f);
                Gizmos.DrawLine(hitGroundHeight, hitPosition);
            }

            if (_gizmo_target)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(hitInfo.point, 1.5f);
                Gizmos.DrawLine(_aimedTransform.position, hitPosition);
            }

            if (_gizmo_ignoredHeightTarget)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(hitPositionIngoredHeight, 0.5f);
                Gizmos.DrawLine(_aimedTransform.position, hitPositionIngoredHeight);
            }
        }
    }

    private void ChargeAtack()
    {
        if (_currentEnergyIndicators == null)
        {
            return;
        }

        if (_currentEnergyIndicators[CurrentColourData.CurrentColour].CurrentValue < 10)
        {
            return;
        }

        if (_currentDelay <= 0)
        {
            var lookDirection = _mainCamera.transform.forward;
            _parent.transform.forward = new Vector3(lookDirection.x, _parent.transform.forward.y, lookDirection.z);
            _aimedTransform.forward = lookDirection;
            SwapShootingMode(true);
        }
        else
            Debug.Log(_currentDelay + " remain");
    }

    // private Vector3 GetPoint()
    // {
    //     var ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
    //     if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _groundMask, QueryTriggerInteraction.Ignore))
    //     {
    //         return hitInfo.point;
    //     }
    //     else
    //     {
    //         return ray.GetPoint(1000.0f);
    //     }
    // }

    private Vector3 GetDirection()
    {
        var ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
        
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _groundMask, QueryTriggerInteraction.Ignore))
        {
            return hitInfo.point - _aimedTransform.position;
        }
        else
        {
            return ray.direction * 100.0f;
        }
    }
    
    private void Shoot()
    {
        // if (RaycastUtilities.PointerIsOverUI(Input.mousePosition))
        //     return;

        if (_shootingMode == false)
            return;

        _currentEnergyIndicators[CurrentColourData.CurrentColour].CurrentValue -= 10;
        StartCoroutine(ShotDelay());
        var projectile = Instantiate(_projectilePrefab, _prefabSpawn.position, Quaternion.identity);
        projectile.transform.forward = GetDirection();
        SwapShootingMode(false);
    }

    private IEnumerator ShotDelay()
    {
        _currentDelay = _shotDelay;

        while (_currentDelay > 0)
        {
            yield return _currentDelay -= Time.deltaTime;
        } 
    }

    private void SwapShootingMode(bool mode)
    {
        if (_shootingMode == mode)
            return;

        if (_aimCamera != null)
            _aimCamera.gameObject.SetActive(mode);
        if (_followCamera != null)
            _followCamera.gameObject.SetActive(!mode);
        _shootingMode = mode;
        ServiceLocator.Current.Get<EventBus>().Invoke(new ShootingToggleCallback(mode));
    }

    private void RefreshLaser()
    {
        if (_laserRenderer == null)
        {
            return;
        }

        Vector3 lineEnd;

        if (Physics.Raycast(_prefabSpawn.position, _prefabSpawn.forward, out var hitinfo, _laserLength, _laserMask))
        {
            lineEnd = hitinfo.point;
        }
        else
        {
            lineEnd = _prefabSpawn.position + _aimedTransform.forward * _laserLength;
        }

        _laserRenderer.SetPosition(1, _aimedTransform.InverseTransformPoint(lineEnd));
    }
}
