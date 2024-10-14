using System.Collections.Generic;
using UnityEngine;

public class MutationManager : IService
{
    private readonly Dictionary<string, GameObject> _activeMutations = new();

    private readonly MutationListSO _availableMutationsSO;

    public MutationManager(MutationListSO mutationListSO)
    {
        _availableMutationsSO = mutationListSO;
        ServiceLocator.Current.Get<EventBus>().Subscribe<PlayerInitializationCallback>(PlayerInitializationHandler);
        ServiceLocator.Current.Get<EventBus>().Subscribe<GameExitCallback>(GameExitHandler);
    }

    ~MutationManager()
    {
        ServiceLocator.Current.Get<EventBus>().Unsubscribe<PlayerInitializationCallback>(PlayerInitializationHandler);
        ServiceLocator.Current.Get<EventBus>().Unsubscribe<GameExitCallback>(GameExitHandler);
    }

    public void AddMutation(string mutationName, Vector3 basePosition)
    {
        var mutationData = _availableMutationsSO.mutations.Find(m => m.mutationName == mutationName);

        if (mutationData != null && !_activeMutations.ContainsKey(mutationName))
        {
            var playerTransform = ServiceLocator.Current.Get<PlayerDataManager>().PlayerTransform;

            var mutationInstance = Object.Instantiate(mutationData.mutationPrefab, basePosition + mutationData.offset,
                playerTransform.rotation * Quaternion.Euler(mutationData.rotationAngles), playerTransform);

            if (mutationData.mutationPrefab.TryGetComponent(out IMutation mutationActivation))
                mutationActivation.StartInstruction();
            _activeMutations.Add(mutationData.mutationName, mutationInstance);
        }
    }

    public void EndMutation(string mutationName)
    {
        if (_activeMutations.TryGetValue(mutationName, out GameObject mutationToRemove))
        {
            if(mutationToRemove.TryGetComponent(out IMutation mutationDeactivation))
                mutationDeactivation.EndInstruction();
            _activeMutations.Remove(mutationName);
            Object.Destroy(mutationToRemove);
        }
    }

    public void EndAllMutations()
    {
        foreach (var mutationPair in _activeMutations)
        {
            EndMutation(mutationPair.Key);
        }

        _activeMutations.Clear();
    }

    private void PlayerInitializationHandler(PlayerInitializationCallback callback) => InitializeActiveMutations();

    private void GameExitHandler(GameExitCallback callback) => _activeMutations.Clear();

    private void InitializeActiveMutations()
    {
        foreach (var mutationData in _availableMutationsSO.mutations)
        {
            if (_activeMutations.ContainsKey(mutationData.mutationName))
            {
                _activeMutations.Remove(mutationData.mutationName);

                AddMutation(mutationData.mutationName, 
                    ServiceLocator.Current.Get<PlayerDataManager>().PlayerTransform.position);
            }
        }
    }
}

