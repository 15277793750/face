using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    //public static T getInstance()
    //{
    //    if (!_instance)
    //    {
    //        _instance = new (T)this;
    //    }
    //    return _instance;
    //}

    protected void Awake()
    {
        Instance = (T)this;
    }
}