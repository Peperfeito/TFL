using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEventListener
{
    private CustomEvent _customEvent;
    private Action<object[]> _callback;

    public CustomEventListener(CustomEvent customEvent, Action<object[]> callback)
    {
        this._customEvent = customEvent;
        this._callback = callback;

        this._customEvent.RegisterListener(this);
    }

    public void OnEventTriggered(object[] args)
    {
        this._callback.Invoke(args);
    }
}
