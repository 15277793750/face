using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MySingleton<T> : MonoBehaviour
	where T : MySingleton<T>
{
	public static T Instance { get; private set; }

	protected void Awake()
	{
		Instance = (T) this;
	}
}
