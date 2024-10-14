using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase, RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // public event Action<int> OnColourChange;
    
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _rotationSpeed = 8f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform _groundCheckerTransform;
    [SerializeField] private LayerMask _notPlayerMask;
    //[SerializeField] private GameObject[] _ignoreCollisionWith;
    //private bool _isGrounded;
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private float _groundCheckRadius = 2.7f;
    [SerializeField] private float _groundCheckDistance = 4f;
    [SerializeField] private float _smoothMoveValue = 0.975f;

    private InputController _inputController;
    private Action<InputAction.CallbackContext> _jump;
    // private Action<InputAction.CallbackContext> _move;
    private Action<InputAction.CallbackContext>  _changeToBlue;
    private Action<InputAction.CallbackContext> _changeToOrange;
    private Action<InputAction.CallbackContext> _changeToGreen;
    private Action<InputAction.CallbackContext>  _addTail; // test

    // private Action<InputAction.CallbackContext>  _rotating;
    private bool _isAiming = false;
    private bool _isRotationDelay = false;
    private Vector2 _moveInput;

    public void Initialize()
    {
        SavePosition.SetPos(gameObject);
        ServiceLocator.Current.Get<PlayerDataManager>().SetPlayerData(this);
        ServiceLocator.Current.Get<EventBus>().Invoke(new PlayerInitializationCallback());
    }

    private void OnEnable()
    {
        _inputController = ServiceLocator.Current.Get<PlayerDataManager>().InputController;
        
        _jump  = ctx  => Jump();
        // _move = ctx => Walk(ctx.ReadValue<Vector2>());
        _changeToBlue = ctx => ServiceLocator.Current.Get<EventBus>().Invoke(new ColourSwapCallback(0));
        _changeToOrange = ctx => ServiceLocator.Current.Get<EventBus>().Invoke(new ColourSwapCallback(1));
        _changeToGreen = ctx => ServiceLocator.Current.Get<EventBus>().Invoke(new ColourSwapCallback(2));
        _inputController.Wizard.Enable();
        SubscribeInput();

        _addTail = ctx => ServiceLocator.Current.Get<MutationManager>().AddMutation("Tail", transform.position); // test
        _inputController.Wizard.Jump.performed  += _addTail; // test
    
        ServiceLocator.Current.Get<EventBus>().Subscribe<ShootingToggleCallback>(ShootingToggleHandler);
    }

    private void OnDisable()
    {
        UnsubscribeInput();
        _inputController.Wizard.Jump.performed  -= _addTail; // test
        _inputController.Wizard.Disable();

        ServiceLocator.Current.Get<EventBus>().Unsubscribe<ShootingToggleCallback>(ShootingToggleHandler);
    }
    
    private void SubscribeInput()
    {
        _inputController.Wizard.Jump.performed += _jump;
        // _inputController.Player.Move.performed += _move;
        _inputController.Wizard.ChangeToBlue.performed += _changeToBlue;
        _inputController.Wizard.ChangeToOrange.performed += _changeToOrange;
        _inputController.Wizard.ChangeToGreen.performed += _changeToGreen;
    }

    private void UnsubscribeInput()
    {
        _inputController.Wizard.Jump.performed -= _jump;
        // _inputController.Player.Move.performed -= _move;
        _inputController.Wizard.ChangeToBlue.performed -= _changeToBlue;
        _inputController.Wizard.ChangeToOrange.performed -= _changeToOrange;
        _inputController.Wizard.ChangeToGreen.performed -= _changeToGreen;
    }
    private void Update()
    {
        ReadMoveInput();
    }

    private void FixedUpdate()
    {
        Walk(_moveInput);
        // Rotating();
    }

    private void ReadMoveInput()
    {
        _moveInput = _inputController.Wizard.Move.ReadValue<Vector2>();
    }

    private void Walk(Vector2 moveInput) 
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;

        if (moveInput.magnitude > Mathf.Abs(0.05f) && !_isAiming && !_isRotationDelay)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection),
                Time.fixedDeltaTime * _rotationSpeed);
        }

        _rigidBody.linearVelocity += moveDirection * _speed;
        _rigidBody.linearVelocity = new Vector3(_rigidBody.linearVelocity.x * _smoothMoveValue, _rigidBody.linearVelocity.y, _rigidBody.linearVelocity.z * _smoothMoveValue);
        _rigidBody.angularVelocity = Vector3.zero;

        Debug.DrawRay(_groundCheckerTransform.position, Vector3.down * (_groundCheckDistance + _groundCheckRadius - 0.08f), Color.red, 2);
        Debug.DrawRay(_groundCheckerTransform.position, Vector3.down * (_groundCheckDistance + _groundCheckRadius - 0.1f), Color.green, 2);

        if (!Physics.SphereCast(_groundCheckerTransform.position, _groundCheckRadius, Vector3.down, out _, _groundCheckDistance, _notPlayerMask,
                QueryTriggerInteraction.Ignore))
        {
            _rigidBody.AddForce(Vector3.up * -0.9f, ForceMode.VelocityChange);
            //position.y = obstaclePoint.y;
        }
        else
        {
            if (_rigidBody.linearVelocity.y < 0)
                _rigidBody.linearVelocity = new Vector3(_rigidBody.linearVelocity.x, 0.0f, _rigidBody.linearVelocity.z);
            if (Physics.SphereCast(_groundCheckerTransform.position, _groundCheckRadius, Vector3.down, out RaycastHit hitGround,
                _groundCheckDistance - 0.1f, _notPlayerMask, QueryTriggerInteraction.Ignore))
            {
                _rigidBody.MovePosition(new Vector3(_rigidBody.position.x, _rigidBody.position.y + (_groundCheckDistance - 0.1f - hitGround.distance) + 0.01f, 
                    _rigidBody.position.z));
            }
        }
    }

    // private void Rotating()
    // {
    //     if (!_isAiming)
    //         return;

    //     Vector3 camForward = Camera.main.transform.forward;
    //     //Vector3 camRight = Camera.main.transform.right;
    //     camForward.y = 0f;
    //     //camRight.y = 0f;

    //     camForward.Normalize();
    //     //camRight.Normalize();

    //     // Vector3 dirVector3 = camForward + camRight;
    //     transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(camForward),
    //         Time.fixedDeltaTime * _rotationSpeed);

    // }

    private void Jump()
    {
        if (Physics.SphereCast(_groundCheckerTransform.position, _groundCheckRadius, Vector3.down, out _, _groundCheckDistance + 0.1f, 
            _notPlayerMask, QueryTriggerInteraction.Ignore))
        {
            // animator.SetTrigger("Jump");
            _rigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("Did not find ground layer");
        }
    }

    private void ShootingToggleHandler(ShootingToggleCallback callback)
    {
        _isAiming = callback.IsShooting;
        if(callback.IsShooting)
        {
            _isRotationDelay = true;
        }
        else
        {
            if(gameObject.activeSelf)
                StartCoroutine(RotationDelay());
            else
                _isRotationDelay = false;
        }
    }

    private IEnumerator RotationDelay()
    {
        yield return new WaitForSeconds(0.3f);
        _isRotationDelay = false;
    }
}