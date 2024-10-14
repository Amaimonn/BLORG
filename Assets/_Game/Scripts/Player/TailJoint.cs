using UnityEngine;

public class TailJoint : MonoBehaviour
{
    [SerializeField] private GameObject _connectedTailPiece; // Подключаемый кусочек хвоста
    public float rotationLimit = 10f; // Ограничение вращения в градусах

    void Start()
    {
        if (_connectedTailPiece != null)
        {
            ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
            joint.connectedBody = _connectedTailPiece.GetComponent<Rigidbody>();

            // Добавление настроек ограничения вращения по оси Y
            SoftJointLimit limit = new SoftJointLimit();
            limit.limit = rotationLimit;

            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularYLimit = limit;

            // Остальные настройки joint'а
            // ...

            // Применить изменения
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = Vector3.zero;
            joint.connectedAnchor = Vector3.zero;

            // Применить изменения
            joint.configuredInWorldSpace = true;
        }
        else
        {
            Debug.LogError("Пожалуйста, укажите объект connectedTailPiece в инспекторе!");
        }
    }
}