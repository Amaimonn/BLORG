/*using System;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidLandController : MonoBehaviour
{
    public Transform CameraFollow;

    [SerializeField] HumanoidLandInput _input;
   // [SerializeField] CameraController _cameraController;
    [SerializeField] GameObject _playerObject; // Added for animations

    Vector3 _playerObjectOriginalLocalPosition = Vector3.zero; // Added for animations

    Rigidbody _rigidbody = null;
    CapsuleCollider _capsuleCollider = null;

    [SerializeField] Vector3 _playerMoveInput = Vector3.zero;

    Vector3 _playerLookInput = Vector3.zero;
    Vector3 _previousPlayerLookInput = Vector3.zero;
    [SerializeField] float _cameraPitch = 0.0f;
    [SerializeField] float _playerLookInputLerpTime = 0.35f;

    [Header("Movement")]
    [SerializeField] float _movementMultiplier = 30.0f;
    [SerializeField] float _notGroundedMovementMultiplier = 1.25f;
    [SerializeField] float _rotationSpeedMultiplier = 180.0f;
    [SerializeField] float _pitchSpeedMultiplier = 180.0f;
    [SerializeField] float _crouchSpeedMultiplier = 0.5f;
    [SerializeField] float _runMultiplier = 2.5f;

    [Header("Ground Check")]
    [SerializeField][Range(0.0f, 1.8f)] float _groundCheckRadiusMultiplier = 0.9f;
    [SerializeField][Range(-0.95f, 1.05f)] float _groundCheckDistanceTolerance = 0.05f;
    [SerializeField] float _playerCenterToGroundDistance = 0.0f;
    public bool _playerIsGrounded { get; private set; } = true; // Modified for animations

    RaycastHit _groundCheckHit = new RaycastHit();

    [Header("Gravity")]
    [SerializeField] float _gravityFallCurrent = 0.0f;
    [SerializeField] float _gravityFallMin = 0.0f;
    [SerializeField] float _gravityFallIncrementTime = 0.05f;
    [SerializeField] float _playerFallTimer = 0.0f;
    [SerializeField] float _gravityGrounded = -1.0f;
    [SerializeField] float _maxSlopeAngle = 47.5f;

    [Header("Stairs")]
    [SerializeField] [Range(0.0f, 1.0f)] float _maxStepHeight = 0.5f;
    [SerializeField] [Range(0.0f, 1.0f)] float _minStepDepth = 0.3f;
    [SerializeField] float _stairHeightPaddingMultiplier = 1.5f;
    [SerializeField] bool _isFirstStep = true;
    [SerializeField] float _firstStepVelocityDistanceMultiplier = 0.1f;
    [SerializeField] float _ascendingStairsMovementMultiplier = 0.35f;
    [SerializeField] float _descendingStairsMovementMultiplier = 0.7f;
    [SerializeField] float _maximumAngleOfApproachToAscend = 45.0f;
    public bool _playerIsAscendingStairs { get; private set; } = false; // Modified for animations
    public bool _playerIsDescendingStairs { get; private set; } = false; // Modified for animations
    float _playerHalfHeightToGround = 0.0f;
    float _maxAscendRayDistance = 0.0f; // This gets calculated in Awake() and is based off _maxStepHeight and a max approach angle of _maximumAngleOfApproachToAscend
    float _maxDescendRayDistance = 0.0f; // This gets calculated in Awake() and is based off _maxStepHeight and a max departure angle of 80*
    int _numberOfStepDetectRays = 0;
    float _rayIncrementAmount = 0.0f;

    [Header("Jumping")]
    [SerializeField] float _initialJumpForceMultiplier = 750.0f;
    [SerializeField] float _continualJumpForceMultiplier = 0.1f;
    [SerializeField] float _jumpTime = 0.175f;
    [SerializeField] float _jumpTimeCounter = 0.0f;
    [SerializeField] float _coyoteTime = 0.15f;
    [SerializeField] float _coyoteTimeCounter = 0.0f;
    [SerializeField] float _jumpBufferTime = 0.2f;
    [SerializeField] float _jumpBufferTimeCounter = 0.0f; 
    [SerializeField] bool _jumpWasPressedLastFrame = false;
    public bool _playerIsJumping { get; private set; } = false; // Modified for animations

    [Header("Just For Fun")]
    [SerializeField] float _jumpReactionForceMultiplier = 0.75f;
    RaycastHit _lastGroundCheckHit = new RaycastHit();
    Vector3 _playerMoveInputAtLastGroundCheckHit = Vector3.zero;

    [Header("Crouching")]
    [SerializeField] [Range(0.0f, 1.8f)] float _headCheckRadiusMultiplier = 0.9f;
    [SerializeField] float _crouchTimeMulitplier = 10.0f;
    [SerializeField] float _playercrouchedHeightTolerance = 0.05f;
    public bool _playerIsCrouching { get; private set; } = false; // Modified for animations
    float _crouchAmount = 1.0f;
    float _playerFullHeight = 0.0f; // Note: Gets set in Awake()
    float _playerCrouchedHeight = 0.0f;  // Note: Gets set in Awake()
    Vector3 _playerCenterPoint = Vector3.zero;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        _maxAscendRayDistance = _maxStepHeight / Mathf.Cos(_maximumAngleOfApproachToAscend * Mathf.Deg2Rad);
        _maxDescendRayDistance = _maxStepHeight / Mathf.Cos(80.0f * Mathf.Deg2Rad);

        _numberOfStepDetectRays = Mathf.RoundToInt(((_maxStepHeight * 100.0f) * 0.5f) + 1.0f);
        _rayIncrementAmount = _maxStepHeight / _numberOfStepDetectRays;

        _playerFullHeight = _capsuleCollider.height;
        _playerCrouchedHeight = _playerFullHeight - _crouchAmount;

        _playerObjectOriginalLocalPosition = _playerObject.transform.localPosition; // Added for animations
    }

    private void FixedUpdate()
    {
        //if (!_cameraController.UsingOrbitalCamera)
        //{
        //    _playerLookInput = GetLookInput();
        //    PlayerLook();
        //    PitchCamera();
        //}
        
        _playerMoveInput = GetMoveInput();
        PlayerVariables();
        _playerIsGrounded = PlayerGroundCheck();

        _playerMoveInput = PlayerMove();
        _playerMoveInput = PlayerStairs();
        _playerMoveInput = PlayerSlope();
        _playerMoveInput = PlayerCrouch();
        _playerMoveInput = PlayerRun();

        PlayerInfoCapture();

        _playerMoveInput.y = PlayerFallGravity();
        _playerMoveInput.y = PlayerJump();

        Debug.DrawRay(_playerCenterPoint, _rigidbody.transform.TransformDirection(_playerMoveInput), Color.red, 0.5f);
        _playerMoveInput *= _rigidbody.mass; // NOTE: For dev purposes.

        _rigidbody.AddRelativeForce(_playerMoveInput, ForceMode.Force);
        Debug.Log(_playerMoveInput.x);
    }

    private Vector3 GetLookInput()
    {
        _previousPlayerLookInput = _playerLookInput;
        _playerLookInput = new Vector3(_input.LookInput.x, (_input.InvertMouseY ? -_input.LookInput.y : _input.LookInput.y), 0.0f);
        return Vector3.Lerp(_previousPlayerLookInput, _playerLookInput * Time.deltaTime, _playerLookInputLerpTime);
    }

    private void PlayerLook()
    {
        _rigidbody.rotation = Quaternion.Euler(0.0f, _rigidbody.rotation.eulerAngles.y + (_playerLookInput.x * _rotationSpeedMultiplier), 0.0f);
    }

    private void PitchCamera()
    {
        _cameraPitch += _playerLookInput.y * _pitchSpeedMultiplier;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -89.9f, 89.9f);

        CameraFollow.rotation = Quaternion.Euler(_cameraPitch, CameraFollow.rotation.eulerAngles.y, CameraFollow.rotation.eulerAngles.z);
    }

    private Vector3 GetMoveInput()
    {
        return new Vector3(_input.MoveInput.x, 0.0f, _input.MoveInput.y);
    }

    private void PlayerVariables()
    {
        _playerCenterPoint = _rigidbody.position + _capsuleCollider.center;
    }

    private Vector3 PlayerMove()
    {
        return ((_playerIsGrounded) ? (_playerMoveInput * _movementMultiplier) : (_playerMoveInput * _movementMultiplier * _notGroundedMovementMultiplier));
    }

    private bool PlayerGroundCheck()
    {
        float sphereCastRadius = _capsuleCollider.radius * _groundCheckRadiusMultiplier;
        Physics.SphereCast(_playerCenterPoint, sphereCastRadius, Vector3.down, out _groundCheckHit);
        _playerCenterToGroundDistance = _groundCheckHit.distance + sphereCastRadius;
        return ((_playerCenterToGroundDistance >= _capsuleCollider.bounds.extents.y - _groundCheckDistanceTolerance) &&
                (_playerCenterToGroundDistance <= _capsuleCollider.bounds.extents.y + _groundCheckDistanceTolerance));
    }

    private Vector3 PlayerStairs()
    {
        Vector3 calculatedStepInput = _playerMoveInput;

        _playerHalfHeightToGround = _capsuleCollider.bounds.extents.y;
        if (_playerCenterToGroundDistance < _capsuleCollider.bounds.extents.y)
        {
            _playerHalfHeightToGround = _playerCenterToGroundDistance;
        }

        calculatedStepInput = AscendStairs(calculatedStepInput);
        if (!(_playerIsAscendingStairs))
        {
            calculatedStepInput = DescendStairs(calculatedStepInput);
        }
        return calculatedStepInput;
    }

    private Vector3 AscendStairs(Vector3 calculatedStepInput)
    {
        if (_input.MoveIsPressed)
        {
            float calculatedVelDistance = _isFirstStep ? (_rigidbody.velocity.magnitude * _firstStepVelocityDistanceMultiplier) + _capsuleCollider.radius : _capsuleCollider.radius;

            float ray = 0.0f;
            List<RaycastHit> raysThatHit = new List<RaycastHit>();
            for (int x = 1;
                 x <= _numberOfStepDetectRays;
                 x++, ray += _rayIncrementAmount)
            {
                Vector3 rayLower = new Vector3(_playerCenterPoint.x,((_playerCenterPoint.y - _playerHalfHeightToGround) + ray), _playerCenterPoint.z);
                RaycastHit hitLower;
                if (Physics.Raycast(rayLower, _rigidbody.transform.TransformDirection(_playerMoveInput), out hitLower, calculatedVelDistance + _maxAscendRayDistance))
                {
                    float stairSlopeAngle = Vector3.Angle(hitLower.normal, _rigidbody.transform.up);
                    if (stairSlopeAngle == 90.0f)
                    {
                        raysThatHit.Add(hitLower);
                    }
                }
            }
            if (raysThatHit.Count > 0)
            {
                Vector3 rayUpper = new Vector3(_playerCenterPoint.x, (((_playerCenterPoint.y - _playerHalfHeightToGround) + _maxStepHeight) + _rayIncrementAmount), _playerCenterPoint.z);
                RaycastHit hitUpper;
                Physics.Raycast(rayUpper, _rigidbody.transform.TransformDirection(_playerMoveInput), out hitUpper, calculatedVelDistance + (_maxAscendRayDistance * 2.0f));
                if (!(hitUpper.collider) ||
                     (hitUpper.distance - raysThatHit[0].distance) > _minStepDepth)
                {
                    if (Vector3.Angle(raysThatHit[0].normal, _rigidbody.transform.TransformDirection(-_playerMoveInput)) <= _maximumAngleOfApproachToAscend)
                    {
                        Debug.DrawRay(rayUpper, _rigidbody.transform.TransformDirection(_playerMoveInput), Color.yellow, 5.0f);

                        _playerIsAscendingStairs = true;
                        Vector3 playerRelX = Vector3.Cross(_playerMoveInput, Vector3.up);

                        if (_isFirstStep)
                        {
                            calculatedStepInput = Quaternion.AngleAxis(45.0f, playerRelX) * calculatedStepInput;
                            _isFirstStep = false;
                        }
                        else
                        {
                            float stairHeight = raysThatHit.Count * _rayIncrementAmount * _stairHeightPaddingMultiplier;

                            float avgDistance = 0.0f;
                            foreach (RaycastHit r in raysThatHit)
                            {
                                avgDistance += r.distance;
                            }
                            avgDistance /= raysThatHit.Count;

                            float tanAngle = Mathf.Atan2(stairHeight, avgDistance) * Mathf.Rad2Deg;
                            calculatedStepInput = Quaternion.AngleAxis(tanAngle, playerRelX) * calculatedStepInput;
                            calculatedStepInput *= _ascendingStairsMovementMultiplier;
                        }
                    }
                    else
                    {   // more than 45* approach
                        _playerIsAscendingStairs = false;
                        _isFirstStep = true;
                    }
                }
                else
                {   // top ray hit something
                    _playerIsAscendingStairs = false;
                    _isFirstStep = true;
                }
            }
            else
            {   // no rays hit
                _playerIsAscendingStairs = false;
                _isFirstStep = true;
            }
        }
        else
        {   // move is not pressed
            _playerIsAscendingStairs = false;
            _isFirstStep = true;
        }
        return calculatedStepInput;
    }

    private Vector3 DescendStairs(Vector3 calculatedStepInput)
    {
        if (_input.MoveIsPressed)
        {
            float ray = 0.0f;
            List<RaycastHit> raysThatHit = new List<RaycastHit>();
            for (int x = 1;
                 x <= _numberOfStepDetectRays;
                 x++, ray += _rayIncrementAmount)
            {
                Vector3 rayLower = new Vector3(_playerCenterPoint.x, ((_playerCenterPoint.y - _playerHalfHeightToGround) + ray), _playerCenterPoint.z);
                RaycastHit hitLower;
                if (Physics.Raycast(rayLower, _rigidbody.transform.TransformDirection(-_playerMoveInput), out hitLower, _capsuleCollider.radius + _maxDescendRayDistance))
                {
                    float stairSlopeAngle = Vector3.Angle(hitLower.normal, _rigidbody.transform.up);
                    if (stairSlopeAngle == 90.0f)
                    {
                        raysThatHit.Add(hitLower);
                    }
                }
            }
            if (raysThatHit.Count > 0)
            {
                Vector3 rayUpper = new Vector3(_playerCenterPoint.x, (((_playerCenterPoint.y - _playerHalfHeightToGround) + _maxStepHeight) + _rayIncrementAmount), _playerCenterPoint.z);
                RaycastHit hitUpper;
                Physics.Raycast(rayUpper, _rigidbody.transform.TransformDirection(-_playerMoveInput), out hitUpper, _capsuleCollider.radius + (_maxDescendRayDistance * 2.0f));
                if (!(hitUpper.collider) ||
                     (hitUpper.distance - raysThatHit[0].distance) > _minStepDepth)
                {
                    if (!(_playerIsGrounded) && hitUpper.distance < _capsuleCollider.radius + (_maxDescendRayDistance * 2.0f))
                    {
                        Debug.DrawRay(rayUpper, _rigidbody.transform.TransformDirection(-_playerMoveInput), Color.yellow, 5.0f);

                        _playerIsDescendingStairs = true;
                        Vector3 playerRelX = Vector3.Cross(_playerMoveInput, Vector3.up);

                        float stairHeight = raysThatHit.Count * _rayIncrementAmount * _stairHeightPaddingMultiplier;

                        float avgDistance = 0.0f;
                        foreach (RaycastHit r in raysThatHit)
                        {
                            avgDistance += r.distance;
                        }
                        avgDistance /= raysThatHit.Count;

                        float tanAngle = Mathf.Atan2(stairHeight, avgDistance) * Mathf.Rad2Deg;
                        calculatedStepInput = Quaternion.AngleAxis(tanAngle - 90.0f, playerRelX) * calculatedStepInput;
                        calculatedStepInput *= _descendingStairsMovementMultiplier;
                    }
                    else
                    {   // more than 45* approach
                        _playerIsDescendingStairs = false;
                    }
                }
                else
                {   // top ray hit something
                    _playerIsDescendingStairs = false;
                }
            }
            else
            {   // no rays hit
                _playerIsDescendingStairs = false;
            }
        }
        else
        {   // move is not pressed
            _playerIsDescendingStairs = false;
        }
        return calculatedStepInput;
    }

    private Vector3 PlayerSlope()
    {
        Vector3 calculatedPlayerMovement = _playerMoveInput;

        if (_playerIsGrounded && !_playerIsAscendingStairs && !_playerIsDescendingStairs)
        {
            Vector3 localGroundCheckHitNormal = _rigidbody.transform.InverseTransformDirection(_groundCheckHit.normal);

            float groundSlopeAngle = Vector3.Angle(localGroundCheckHitNormal, _rigidbody.transform.up);
            if (groundSlopeAngle == 0.0f)
            {
                if (_input.MoveIsPressed)
                {
                    RaycastHit rayHit;
                    float rayCalculatedRayHeight = _playerCenterPoint.y - _playerCenterToGroundDistance + _groundCheckDistanceTolerance;
                    Vector3 rayOrigin = new Vector3(_playerCenterPoint.x, rayCalculatedRayHeight, _playerCenterPoint.z);
                    if (Physics.Raycast(rayOrigin, _rigidbody.transform.TransformDirection(calculatedPlayerMovement), out rayHit, 0.75f))
                    {
                        if (Vector3.Angle(rayHit.normal, _rigidbody.transform.up) > _maxSlopeAngle)
                        {
                            Debug.Log(_playerMoveInput.y);
                            calculatedPlayerMovement.y = -_movementMultiplier;
                        }
                    }
                    //Debug.DrawRay(rayOrigin, _rigidbody.transform.TransformDirection(calculatedPlayerMovement), Color.green, 1.0f);
                }

                if (calculatedPlayerMovement.y == 0.0f)
                {
                    calculatedPlayerMovement.y = _gravityGrounded;
                }
            }
            else
            {
                Quaternion slopeAngleRotation = Quaternion.FromToRotation(_rigidbody.transform.up, localGroundCheckHitNormal);
                calculatedPlayerMovement = slopeAngleRotation * calculatedPlayerMovement;

                float relativeSlopeAngle = Vector3.Angle(calculatedPlayerMovement, _rigidbody.transform.up) - 90.0f;
                calculatedPlayerMovement += calculatedPlayerMovement * (relativeSlopeAngle / 90.0f);

                if (groundSlopeAngle < _maxSlopeAngle)
                {
                    if (_input.MoveIsPressed)
                    {
                        calculatedPlayerMovement.y += _gravityGrounded;
                    }
                }
                else
                {
                    float calculatedSlopeGravity = groundSlopeAngle * -0.2f;
                    if (calculatedSlopeGravity < calculatedPlayerMovement.y)
                    {
                        calculatedPlayerMovement.y = calculatedSlopeGravity;
                    }
                }
            }
//#if UNITY_EDITOR
//            Debug.DrawRay(_rigidbody.position, _rigidbody.transform.TransformDirection(calculatedPlayerMovement), Color.red, 0.5f);
//#endif
        }

        return calculatedPlayerMovement;   
    }

    private Vector3 PlayerCrouch()
    {
        Vector3 calculatedPlayerCrouchSpeed = _playerMoveInput;
        if (_input.CrouchIsPressed)
        {
            Crouch();
        }
        else if (_playerIsCrouching)
        {
            Uncrouch();
        }
        if (_playerIsCrouching)
        {
            calculatedPlayerCrouchSpeed *= _crouchSpeedMultiplier;
        }
        return calculatedPlayerCrouchSpeed;
    }

    private void Crouch()
    {
        if (_capsuleCollider.height >= _playerCrouchedHeight + _playercrouchedHeightTolerance)
        {
            float time = Time.fixedDeltaTime * _crouchTimeMulitplier;
            float amount = Mathf.Lerp(0.0f, _crouchAmount, time);

            _capsuleCollider.height -= amount;
            _capsuleCollider.center = new Vector3(_capsuleCollider.center.x, _capsuleCollider.center.y + (amount * 0.5f), _capsuleCollider.center.z);
            _rigidbody.position = new Vector3(_rigidbody.position.x, _rigidbody.position.y - amount, _rigidbody.position.z);

            _playerObject.transform.localPosition += Vector3.up * amount;  // Added for animations

            _playerIsCrouching = true;
        }
        else
        {
            EnforceExactCharHeight();
        }
    }

    private void Uncrouch()
    {
        if(_capsuleCollider.height < _playerFullHeight - _playercrouchedHeightTolerance)
        {
            float sphereCastRadius = _capsuleCollider.radius * _headCheckRadiusMultiplier;
            float headroomBufferDistance = 0.05f;
            float sphereCastTravelDistance = (_capsuleCollider.bounds.extents.y + headroomBufferDistance) - sphereCastRadius;
            if (!(Physics.SphereCast(_playerCenterPoint, sphereCastRadius, _rigidbody.transform.up, out _, sphereCastTravelDistance)))
            {
                float time = Time.fixedDeltaTime * _crouchTimeMulitplier;
                float amount = Mathf.Lerp(0.0f, _crouchAmount, time);

                _capsuleCollider.height += amount;
                _capsuleCollider.center = new Vector3(_capsuleCollider.center.x, _capsuleCollider.center.y - (amount * 0.5f), _capsuleCollider.center.z);
                _rigidbody.position = new Vector3(_rigidbody.position.x, _rigidbody.position.y + amount, _rigidbody.position.z);

                _playerObject.transform.localPosition -= Vector3.up * amount; // Added for animations
            }
        }
        else
        {
            _playerIsCrouching = false;
            EnforceExactCharHeight();
        }
    }

    private void EnforceExactCharHeight()
    {
        if (_playerIsCrouching)
        {
            _capsuleCollider.height = _playerCrouchedHeight;
            _capsuleCollider.center = new Vector3(0.0f, _crouchAmount * 0.5f, 0.0f);
            _playerObject.transform.localPosition = _playerObjectOriginalLocalPosition + Vector3.up * _crouchAmount; // Added for animations
        }
        else
        {
            _capsuleCollider.height = _playerFullHeight;
            _capsuleCollider.center = Vector3.zero;
            _playerObject.transform.localPosition = _playerObjectOriginalLocalPosition; // Added for animations
        }
    }

    private Vector3 PlayerRun()
    {
        Vector3 calculatedPlayerRunSpeed = _playerMoveInput;
        if (_input.MoveIsPressed && _input.RunIsPressed && !_playerIsCrouching)
        {
            calculatedPlayerRunSpeed *= _runMultiplier;
        }
        return calculatedPlayerRunSpeed;
    }

    private void PlayerInfoCapture()
    {
        if (_playerIsGrounded && _groundCheckHit.collider)
        {
            _lastGroundCheckHit = _groundCheckHit;
            _playerMoveInputAtLastGroundCheckHit = _playerMoveInput;
        }
    }

    private float PlayerFallGravity()
    {
        float gravity = _playerMoveInput.y;
        if(_playerIsGrounded || _playerIsAscendingStairs || _playerIsDescendingStairs)
        {
            _gravityFallCurrent = _gravityFallMin; // Reset
        }
        else
        {
            _playerFallTimer -= Time.fixedDeltaTime;
            if (_playerFallTimer < 0.0f)
            {
                float gravityFallMax = _movementMultiplier * _runMultiplier * 5.0f;
                float gravityFallIncrementAmount = (gravityFallMax - _gravityFallMin) * 0.1f;
                if (_gravityFallCurrent < gravityFallMax)
                {
                    _gravityFallCurrent += gravityFallIncrementAmount;
                }
                _playerFallTimer = _gravityFallIncrementTime;
            }
            gravity = -_gravityFallCurrent;
        }
        return gravity;
    }

    private float PlayerJump()
    {
        float calculatedJumpInput = _playerMoveInput.y;

        SetJumpTimeCounter();
        SetCoyoteTimeCounter();
        SetJumpBufferTimeCounter();

        if (_jumpBufferTimeCounter > 0.0f && !_playerIsJumping && _coyoteTimeCounter > 0.0f)
        {
            //if (Vector3.Angle(_rigidbody.transform.up, _groundCheckHit.normal) < _maxSlopeAngle)
            //{
            KickStuffOutFromUnder(); // Just For Fun

                calculatedJumpInput = _initialJumpForceMultiplier;
                _playerIsJumping = true;
                _jumpBufferTimeCounter = 0.0f;
                _coyoteTimeCounter = 0.0f;
            //}
        }
        else if (_input.JumpIsPressed && _playerIsJumping && !_playerIsGrounded && _jumpTimeCounter > 0.0f)
        {
            calculatedJumpInput = _initialJumpForceMultiplier * _continualJumpForceMultiplier;
        }
        else if (_playerIsJumping && _playerIsGrounded)
        {
            _playerIsJumping = false;
        }
        return calculatedJumpInput;
    }

    private void SetJumpTimeCounter()
    {
        if (_playerIsJumping && !_playerIsGrounded)
        {
            _jumpTimeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            _jumpTimeCounter = _jumpTime;
        }
    }

    private void SetCoyoteTimeCounter()
    {
        if (_playerIsGrounded)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.fixedDeltaTime;
        }
    }

    private void SetJumpBufferTimeCounter()
    {
        if (!_jumpWasPressedLastFrame && _input.JumpIsPressed)
        {
            _jumpBufferTimeCounter = _jumpBufferTime;
        }
        else if (_jumpBufferTimeCounter > 0.0f)
        {
            _jumpBufferTimeCounter -= Time.fixedDeltaTime;
        }
        _jumpWasPressedLastFrame = _input.JumpIsPressed;
    }

    private void KickStuffOutFromUnder()
    {
        if (_lastGroundCheckHit.collider.attachedRigidbody)
        {
            Vector3 force = (_rigidbody.transform.TransformDirection(_playerMoveInputAtLastGroundCheckHit) * 
                                                                     _lastGroundCheckHit.collider.attachedRigidbody.mass *
                                                                     _jumpReactionForceMultiplier);
            _lastGroundCheckHit.collider.attachedRigidbody.AddForceAtPosition(-force, _lastGroundCheckHit.point, ForceMode.Impulse);
        }
    }
}*/
