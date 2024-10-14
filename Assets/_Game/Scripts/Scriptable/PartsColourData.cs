using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PartsColourData", menuName = "ScriptableObjects/PartsColourData")]
public class PartsColourData : ScriptableObject
{
    [SerializeField] private Texture[] headColourTexture = new Texture[3];
    [SerializeField] private Material[] headColourMat = new Material[3];
    [SerializeField] private Texture[] bodyColourTexture = new Texture[3];
    [SerializeField] private Material[] bodyColourMat = new Material[3];
    [SerializeField] private Texture[] left_handColourTexture = new Texture[3];
    [SerializeField] private Material[] left_handColourMat = new Material[3];
    [SerializeField] private Texture[] right_handColourTexture = new Texture[3];
    [SerializeField] private Material[] right_handColourMat = new Material[3];
    [SerializeField] private Texture[] hoodColourTexture = new Texture[3];
    [SerializeField] private Material[] hoodColourMat = new Material[3];
    public Texture[] HeadColourTexture
    {
        get { return headColourTexture; }
    }
    public Material[] HeadColourMat
    {
        get { return headColourMat; }
    }

    public Texture[] BodyColourTexture
    {
        get { return bodyColourTexture; }
    }
    public Material[] BodyColourMat
    {
        get { return bodyColourMat; }
    }

    public Texture[] Left_handColourTexture
    {
        get { return left_handColourTexture; }
    }
    public Material[] Left_handColourMat
    {
        get { return left_handColourMat; }
    }

    public Texture[] Right_handColourTexture
    {
        get { return right_handColourTexture; }
    }
    public Material[] Right_handColourMat
    {
        get { return right_handColourMat; }
    }

    public Texture[] HoodColourTexture
    {
        get { return hoodColourTexture; }
    }
    public Material[] HoodColourMat
    {
        get { return hoodColourMat; }
    }

}
