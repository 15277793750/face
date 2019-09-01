using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public delegate void NoParamsDelegate();
    public static NoParamsDelegate noParamsDelegate;
    public static Y GetDicValueByKey<Y>(int key, Dictionary<int, Y> dic)
    {

        foreach (var item in dic)
        {
            if (item.Key == key)
            {
                return item.Value;
            }
        }
        return default(Y);
    }

    public static Y GetDicValueByKey<Y>(string key, Dictionary<string, Y> dic)
    {

        foreach (var item in dic)
        {
            if (item.Key == key)
            {
                return item.Value;
            }
        }
        return default(Y);
    }
}
