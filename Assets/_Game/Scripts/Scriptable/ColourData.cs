using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ColourData", menuName = "ScriptableObjects/ColourData")]
public class ColourDataSO : ScriptableObject
{
    [SerializeField]
    private Color[] magicColour = new Color[3];
    public Color[] MagicColour
    {
        get { return magicColour; }
    }
}
