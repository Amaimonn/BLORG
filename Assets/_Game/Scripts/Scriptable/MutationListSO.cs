using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new MutationList", menuName = "ScriptableObjects/MutationList")]
public class MutationListSO : ScriptableObject

{
    public List<MutationDataSO> mutations = new List<MutationDataSO>();
    //[SerializeField] private Dictionary <string, MutationDataSO> _allMutationsDict;
    //[SerializeField] private List<MutationDataSO> _allMutationsList;
    //private List<MutationDataSO> _currentMutationsList;
    //public List<MutationDataSO> AllMutationsList
    //{
    //    get { return _allMutationsList; }
    //}
    //public List<MutationDataSO> CurrentMutationsList
    //{
    //    get { return _currentMutationsList; }
    //}
    //public MutationDataSO this[int index]
    //{
    //    get { return _currentMutationsList[index]; }
    //}
    //public void SetDefaultMutations()
    //{
    //    _currentMutationsList.Clear();
    //}
    //public void RemoveMut(GameObject mutation)
    //{
    //    if (mutation != null)
    //    {
    //        if (_currentMutationsList.Contains(mutation))
    //        {
    //            _mutationsList.Remove(mutation);
    //            Destroy(mutation);
    //        }
    //    }
    //}
    //public List<GameObject> MutationsList
    //{
    //    get { return _mutationsList; }
    //}
    //public GameObject this[int index]
    //{
    //    get { return _mutationsList[index]; }
    //}
    //public void SetDefaultMutations()
    //{
    //    _mutationsList.Clear();
    //}
}

