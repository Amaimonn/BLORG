using UnityEngine;

public class TailJoint : MonoBehaviour
{
    [SerializeField] private GameObject _connectedTailPiece; // ������������ ������� ������
    public float rotationLimit = 10f; // ����������� �������� � ��������

    void Start()
    {
        if (_connectedTailPiece != null)
        {
            ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
            joint.connectedBody = _connectedTailPiece.GetComponent<Rigidbody>();

            // ���������� �������� ����������� �������� �� ��� Y
            SoftJointLimit limit = new SoftJointLimit();
            limit.limit = rotationLimit;

            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularYLimit = limit;

            // ��������� ��������� joint'�
            // ...

            // ��������� ���������
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = Vector3.zero;
            joint.connectedAnchor = Vector3.zero;

            // ��������� ���������
            joint.configuredInWorldSpace = true;
        }
        else
        {
            Debug.LogError("����������, ������� ������ connectedTailPiece � ����������!");
        }
    }
}