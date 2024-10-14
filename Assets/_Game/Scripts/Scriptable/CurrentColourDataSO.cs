using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new CurrentColourData", menuName = "ScriptableObjects/CurrentColourData")]
public class CurrentColourDataSO : ScriptableObject
{
    [SerializeField] private int currentColour = 0;
    [SerializeField] private int defaultColour = 0;
    public int CurrentColour
    {
        get { return currentColour; }
        set { currentColour = value; }
    }
    public void SetDefaultColour()
    {
        currentColour= defaultColour;
    }
}
