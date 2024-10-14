/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairClimb : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.91f;
    [SerializeField] float stepSmooth = 2f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, rigidBody.transform.position.y + stepHeight, stepRayUpper.transform.position.z);
    }

    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            stepClimb();
    }
    //void OnDrawGizmosSelected()
    //{
    //    // Draws a 5 unit long red line in front of the object
    //    Gizmos.color = Color.red;
    //    Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
    //    Gizmos.DrawRay(transform.position, direction);
    //}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward)*2.84f);
        Gizmos.DrawRay(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1).normalized* 2.84f);
        Gizmos.DrawRay(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1).normalized * 2.84f);
    }
    void stepClimb()
    {
        //Gizmos.color = Color.red;
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 2.84f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 2.86f))
            {
                rigidBody.position -= new Vector3(0f, -stepSmooth * Time.fixedDeltaTime, 0f);
            }
        }
       // Gizmos.DrawRay(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward)*1.42f);
        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 2.84f))
        {

            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 2.86f))
            {
                rigidBody.position -= new Vector3(0f, -stepSmooth * Time.fixedDeltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 2.84f))
        {

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 2.86f))
            {
                rigidBody.position -= new Vector3(0f, -stepSmooth * Time.fixedDeltaTime, 0f);
            }
        }
    }
}*/
