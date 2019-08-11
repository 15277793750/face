using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test55 : MonoBehaviour
{
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        int a = 1;
        print(a);
        print(a << 16);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        print(screenPos);
    }
}
