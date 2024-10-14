using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventBus: IService
{
    private Dictionary<string, List<PriorityCallback>> _callbacks = new();

    public void Subscribe<T>(Action<T> callback, int priority=0)
    {
        string key = typeof(T).Name;

        if(!_callbacks.TryGetValue(key, out var callbackList))
        {
            callbackList = new List<PriorityCallback>();
            _callbacks.Add(key, callbackList);
        }

        callbackList.Add(new PriorityCallback(priority, callback));
        callbackList.OrderByDescending(x => x.Priority).ToList();
    }

    public void Unsubscribe<T>(Action<T> callback)
    {
        string key = typeof(T).Name;

        if(_callbacks.TryGetValue(key, out var callbackList))
        {
            var callbackToDelete = callbackList.FirstOrDefault(x => x.Callback.Equals(callback));
            if(callbackList.Remove(callbackToDelete))
            {
                if(callbackList.Count == 0)
                {
                    _callbacks.Remove(key);
                }
            }
        }
        else
        {
            Debug.Log($"key {key} for unsubscribe not found");
        }
    }

    public void Invoke<T>(T signal)
    {
        string key = typeof(T).Name;

        if(_callbacks.TryGetValue(key, out var callbackList))
        {
            foreach(var obj in callbackList)
            {
                var callback = obj.Callback as Action<T>;
                callback?.Invoke(signal);
            }
        }
    }
}
