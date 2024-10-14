using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*private Animator animator;
    private Rigidbody rb;
    public float speed = 2f;
    public float rotationSpeed=10f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(h, 0, v);
        if (moveVector.magnitude > Mathf.Abs(0.05f))
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveVector), Time.deltaTime * 10);
        rb.velocity = Vector3.ClampMagnitude(moveVector, 1) * speed;
        animator.SetFloat("speed", Vector3.ClampMagnitude(moveVector, 1).magnitude);*/
//��� ������� ����������� ��� ��� ������ �� ��������� 
//���� �� ������ ����� ������������� ��������� Rigidbody
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    private float Speed = 16f;
    public float JumpForce = 4f;
    public float gravity = -99999999f;

    private Animator animator;

    //��� �� ��� ���������� �������� �������� ��� "Ground" �� ���� ����������� �����
    private bool _isGrounded;
    private Rigidbody _rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    // �������� �������� ��� ��� �������� � ������� 
    // ���������� ������������ � FixedUpdate, � �� � Update
    void FixedUpdate()
    {
        MovementLogic(animator);
        JumpLogic();  
    }

    private void MovementLogic(Animator animator)
    {
        float moveHorizontal = 0.0f;
        float moveVertical = 0.0f;
        // ����������� �����
        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal-=1.0f;
        }

        // ����������� ������
        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal += 1.0f;
        }

        //����������� �����
        if (Input.GetKey(KeyCode.W))
        {
            moveVertical += 1.0f;
        }       

        // ����������� ����
        if (Input.GetKey(KeyCode.S))
        {
            moveVertical -= 1.0f;
        }
        //if (isMovingUp == false || isMovingDown == false)
        //    moveVertical = Input.GetAxis("Vertical");
        //else
        //    moveVertical = 0.0f;
        //Vector3 movement;
        Vector3 movement2d = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //if (_rb.velocity.y > 0)
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //  if (_isGrounded && _rb.velocity.y < 0)
        // {
        //     movement.y = -2f;
        // }
        //  if (_isGrounded == false)
        //  _rb.AddForce(Vector3.up * -0.05f, ForceMode.Impulse);//movement.y += gravity;// * Time.deltaTime;
        //else
        //Vector3 vel = _rb.velocity;
        //float vel = _rb.velocity.y;
        //  Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // _rb.MovePosition(_rb.position+Vector3.ClampMagnitude(movement, 1) * Speed * Time.fixedDeltaTime);//.AddForce(movement * Speed);
        movement = Vector3.ClampMagnitude(movement, 1) * Speed;
        _rb.linearVelocity = new Vector3(movement.x, _rb.linearVelocity.y, movement.z);
        //_rb.AddForce(Vector3.ClampMagnitude(movement, 1) * Speed);
        animator.SetFloat("speed", Vector3.ClampMagnitude(movement2d, 1).magnitude);
        if (movement2d.magnitude > 0.2f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement2d), Time.deltaTime * 10);
        if(movement2d.magnitude==0.0f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, -1)), Time.deltaTime * 3);
    }
    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                //_rb.velocity = new Vector3(_rb.velocity.x, JumpForce, _rb.velocity.z);
                //_rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                //_rb.MovePosition(_rb.position+Vector3.up * JumpForce*Time.fixedDeltaTime);
                // �������� �������� ��� � ����� �� ������ Vector3.up 
                // � �� �� ������ transform.up. ���� �������� ���� ��� 
                // ���� �������� -- ���, �� ��� ������ "����" ����� 
                // ����� �����������. �����, ������, ����...
                // �� ��� ����� ������ ������ � ���������� �����, 
                // ������ � Vector3.up
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }

    void OnCollisionExit(Collision collision)
    {
        IsGroundedUpate(collision, false);
    }

    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            _isGrounded = value;
        }
    }
}