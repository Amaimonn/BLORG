using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAttenuation : MonoBehaviour
{
    [SerializeField] private Transform listenerTransform;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minDist = 1;
    [SerializeField] private float maxDist = 400;

    void Update()
    {
        float dist = Vector3.Distance(transform.position, listenerTransform.position);

        if (dist < minDist)
        {
            audioSource.volume = 1;
        }
        else if (dist > maxDist)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = 1 - ((dist - minDist) / (maxDist - minDist));
        }
    }
}
