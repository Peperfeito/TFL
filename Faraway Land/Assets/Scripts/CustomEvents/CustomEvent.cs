using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "CustomEvents/Event")]
public class CustomEvent : ScriptableObject
{
    public readonly List<CustomEventListener> _listeners = new List<CustomEventListener>();

    public void RegisterListener(CustomEventListener listener)
    {
        if (this._listeners.Contains(listener)) return;
        this._listeners.Add(listener);
    }

    public void UnregisterListener(CustomEventListener listener)
    {
        if (!this._listeners.Contains(listener)) return;
        this._listeners.Remove(listener);
    }

    public void Trigger(params object[] args)
    {
        for (int i = 0; i < this._listeners.Count; i++)
        {
            this._listeners[i].OnEventTriggered(args);
        }
    }
}
