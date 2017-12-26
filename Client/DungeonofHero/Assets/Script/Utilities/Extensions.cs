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

    public static bool HasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type)
    {
        if (name == null || name == "") { return false; }
        AnimatorControllerParameter[] parameters = self.parameters;
        foreach (AnimatorControllerParameter currParam in parameters)
        {
            if (currParam.type == type && currParam.name == name)
            {
                return true;
            }
        }
        return false;
    }
}
