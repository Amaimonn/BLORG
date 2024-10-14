using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailMutation : MonoBehaviour, IMutation
{
    [SerializeField] private Mesh _meshWithTail;
    [SerializeField] private Mesh _meshWithoutTail;

    public void StartInstruction()
    {
        ServiceLocator.Current.Get<PlayerDataManager>().PlayerTransform.transform.Find("Body").Find("dBody")
            .TryGetComponent(out MeshFilter PlayerBodyMeshFilter);

        if (PlayerBodyMeshFilter != null && _meshWithTail != null 
            && _meshWithoutTail != null)
            PlayerBodyMeshFilter.mesh = _meshWithTail;
    }
    
    public void EndInstruction()
    {
        ServiceLocator.Current.Get<PlayerDataManager>().PlayerTransform.transform.Find("Body").Find("dBody")
            .TryGetComponent(out MeshFilter PlayerBodyMeshFilter);

        if (PlayerBodyMeshFilter != null && _meshWithTail != null 
            && _meshWithoutTail != null)
            PlayerBodyMeshFilter.mesh = _meshWithoutTail;
    }
}
