using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPrefabSO", menuName = "ScriptableObjects/Spawn Prefab ")]
public class SpawnPrefabSO : ScriptableObject
{
   // public Cube cubePrefab;
    [SerializeField] private GameObject _spawnPrefab;
    public GameObject SpawnPrefab {  get { return _spawnPrefab; } }
}
