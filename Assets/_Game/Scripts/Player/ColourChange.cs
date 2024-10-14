using System.Collections.Generic;
using UnityEngine;

public class ColourChange : MonoBehaviour
{
    [SerializeField] private Material[] _heroMaterials = new Material[3];
    [SerializeField] private List<GameObject> _playerParts;
    //[SerializeField] private List<GameObject> _playerMutations;
   // [SerializeField] private CurrentColourDataSO _currentColour;
    private readonly List<Renderer> _partRenderers = new();

    // private static bool valueReseted = false;

    private void Start()
    {
        for (var iterator = 0; iterator < _playerParts.Count; iterator++)
        {
            _partRenderers.Add(_playerParts[iterator].GetComponent<Renderer>());
        }

        // if (!valueReseted)
        // {
        //     CurrentColourData.SetDefaultColour();
        //     valueReseted = true;
        // }

        ChangeMaterials(_heroMaterials[CurrentColourData.CurrentColour]);
    }

    private void OnEnable()
    {
        ServiceLocator.Current.Get<EventBus>().Subscribe<ColourSwapCallback>(ColourSwapHandler, 10);
    }

    private void OnDisable()
    {
        ServiceLocator.Current.Get<EventBus>().Unsubscribe<ColourSwapCallback>(ColourSwapHandler);
    }

    private void ColourSwapHandler(ColourSwapCallback callback)
    {
        ChangeMaterials(_heroMaterials[callback.ColourNum]);
        CurrentColourData.CurrentColour = callback.ColourNum;
    }

    private void ChangeMaterials(Material newMaterial)
    {
        foreach (var renderer in _partRenderers)
        {
            if (renderer != null)
            {
                renderer.material = newMaterial;
            }
        }
    }
}
