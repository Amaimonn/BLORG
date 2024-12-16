using UnityEngine;

public class MagicColourSwap : MonoBehaviour
{
    [SerializeField] private CurrentColourDataSO currentColour;
    [SerializeField] private PartsColourData partsColour;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject left_hand;
    [SerializeField] private GameObject right_hand;
    [SerializeField] private GameObject hood;

    private MeshRenderer headMeshRen;
    private MeshRenderer bodyMeshRen;
    private MeshRenderer left_handMeshRen;
    private MeshRenderer right_MeshRen;
    private MeshRenderer hoodMeshRen;

    private static bool valueReseted = false;
    void Start()
    {
        headMeshRen = head.GetComponent<MeshRenderer>();
        bodyMeshRen = body.GetComponent<MeshRenderer>();
        left_handMeshRen = left_hand.GetComponent<MeshRenderer>();
        right_MeshRen = right_hand.GetComponent<MeshRenderer>();
        hoodMeshRen = hood.GetComponent<MeshRenderer>();

        if (!valueReseted)
        {
            currentColour.SetDefaultColour();
            valueReseted = true;
        }

        headMeshRen.material = partsColour.HeadColourMat[currentColour.CurrentColour];
        bodyMeshRen.material = partsColour.BodyColourMat[currentColour.CurrentColour];
        left_handMeshRen.material = partsColour.Left_handColourMat[currentColour.CurrentColour];
        right_MeshRen.material = partsColour.Right_handColourMat[currentColour.CurrentColour];
        hoodMeshRen.material = partsColour.HoodColourMat[currentColour.CurrentColour];
    }
    private void SwapColour()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            //headMeshRen.material.SetTexture("Blue", partsColour.HeadColourTexture[0]);
            headMeshRen.material = partsColour.HeadColourMat[0];
            bodyMeshRen.material = partsColour.BodyColourMat[0];
            left_handMeshRen.material = partsColour.Left_handColourMat[0];
            right_MeshRen.material = partsColour.Right_handColourMat[0];
            hoodMeshRen.material = partsColour.HoodColourMat[0];
            currentColour.CurrentColour = 0;
        }
        if (Input.GetKey(KeyCode.X))
        {
            headMeshRen.material = partsColour.HeadColourMat[1];
            bodyMeshRen.material = partsColour.BodyColourMat[1];
            left_handMeshRen.material = partsColour.Left_handColourMat[1];
            right_MeshRen.material = partsColour.Right_handColourMat[1];
            hoodMeshRen.material = partsColour.HoodColourMat[1];
            currentColour.CurrentColour = 1;
        }
        if (Input.GetKey(KeyCode.C))
        {
            headMeshRen.material = partsColour.HeadColourMat[2];
            bodyMeshRen.material = partsColour.BodyColourMat[2];
            left_handMeshRen.material = partsColour.Left_handColourMat[2];
            right_MeshRen.material = partsColour.Right_handColourMat[2];
            hoodMeshRen.material = partsColour.HoodColourMat[2];
            currentColour.CurrentColour = 2;
            //   meshRen.material.SetTexture("Green", colour_texture[2]);
            // gameObject.GetComponent<Renderer>().material = colour_mat[2];
        }
    }
    // Update is called once per frame
    void Update()
    {
        SwapColour();
    }
}
