using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "new MutationData", menuName = "ScriptableObjects/MutationData")]
public class MutationDataSO : ScriptableObject
{
    public string mutationName;
    public GameObject mutationPrefab;
    public Vector3 offset;
    public Vector3 rotationAngles;
}
