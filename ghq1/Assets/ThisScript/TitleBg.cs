using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBg : MonoBehaviour
{
    public Sprite[] sprite;
    public Image img;

    private float timer;
    public float time;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= time)
        {
            timer = 0;
            index += 1;
            if (index >= sprite.Length)
            {
                index = 0;
            }
            img.sprite = sprite[index];
        }
    }
}
