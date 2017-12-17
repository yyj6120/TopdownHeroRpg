using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
public static class Extensions
{
    public static void SetListener(this UnityEvent unityEvent, UnityAction call)
    {
        unityEvent.RemoveAllListeners();
        unityEvent.AddListener(call);
    }
}
