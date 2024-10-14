using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform target;
    //   public float y_dist = 30.0f;
    //  public float z_dist = -25.0f;
   // [SerializeField] private float damping = 5.0f;
  //  [SerializeField] private bool smoothRotation = true;
   // [SerializeField] private bool followBehind = true;
   // [SerializeField] private float rotationDamping = 10.0f;
    [SerializeField] private float smooth = 5.0f;
    [SerializeField] private float addUpAngle = -4.0f;
    [SerializeField] private Vector3 offset = new(0, 40, -52);
    //private void Awake()
    //{
    //    Aiming._mainCamera = this.gameObject.GetComponent<Camera>();
    //}
    public void Initialize()
    {
        target = ServiceLocator.Current.Get<PlayerDataManager>().PlayerTransform;//GameObject.FindGameObjectWithTag("Player").transform;
        //transform.LookAt(target, target.up);
        transform.SetPositionAndRotation(target.position + offset/2, 
            Quaternion.Euler(90.0f + addUpAngle - (Mathf.Atan(Mathf.Abs((offset.z - 4.0f) 
            / offset.y)) / Mathf.PI * 180.0f), 0f, 0f));
      //  offset = new Vector3(0, y_dist+4.0f, z_dist);
    }

    private void Update()
    {
       // offset = new Vector3(0, y_dist + 4.0f, z_dist);
       // transform.rotation = Quaternion.Euler(90.0f+ addUpAngle - Mathf.Atan(Mathf.Abs(z_dist / y_dist)) / Mathf.PI * 180.0f, 0f, 0f);
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * smooth);
        //offset - ������ ������� ������ ������������ ������.������ ����� ������ ����� � ���� �� �������-�� ������.

        //if (smoothRotation)
        //{
        //    Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        //}
        //else transform.LookAt(target, target.up);
        //Vector3 wantedPosition;
        //if (followBehind)
        //    wantedPosition = target.TransformPoint(0, height, -distance);
        //else
        //  wantedPosition = target.TransformPoint(0, height, distance);

    //transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

    //if (smoothRotation)
    //{
    //    Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
    //}
    //else transform.LookAt(target, target.up);
}
}
