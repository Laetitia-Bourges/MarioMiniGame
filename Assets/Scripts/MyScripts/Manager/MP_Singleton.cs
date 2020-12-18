using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MP_Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance = default(T);
    public static T Instance => instance;
    protected virtual void Awake()
    {
        if(instance && instance!= this)
        {
            Destroy(this);
            return;
        }
        instance = this as T;
    }
}
