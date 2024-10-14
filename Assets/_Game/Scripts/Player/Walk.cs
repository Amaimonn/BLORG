using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class movePlayer : MonoBehaviour
{
    private Animator animator;
    //private Rigidbody rb;
    private GameObject MainCamera;
    public float speed = 2f;
    public float rotationSpeed=10f;
    private float JumpForce = 8f;
    private bool _isGrounded;
    private Rigidbody _rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    // обратите внимание что все действия с физикой 
    // необходимо обрабатывать в FixedUpdate, а не в Update
    void FixedUpdate()
    {
        Jump();
    }

    void Update()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        

        //Vector3 moveVector = new Vector3(h, 0, v);
        //if (moveVector.magnitude > Mathf.Abs(0.05f))
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveVector), Time.deltaTime * 10);
        //rb.velocity = Vector3.ClampMagnitude(moveVector, 1) * speed;
        //rb.velocity = MainCamera.transform.forward * speed * Time.deltaTime;
        //animator.SetFloat("speed", Vector3.ClampMagnitude(moveVector, 1).magnitude);
       // rb.MovePosition(rb.position + moveVector * speed * Time.deltaTime);
    }
    void Jump()
    {
        if (Input.GetAxis("Jump") > 0)
            if (_isGrounded)
            _rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
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