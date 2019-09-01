using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winbase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // private Tween move_up_anim;
    //    move_up_anim = transform.DOMoveY(transform.position.y + move_up_distance, move_up_time);
    //        move_up_anim.SetAutoKill(false);
    //        move_up_anim.SetEase(Ease.OutQuad);
    //        move_up_anim.Play();
    //        move_up_anim.OnComplete(
    //            delegate ()
    //            {
    //                D();
    //}
    //        );
    public object winParams;
    public virtual void Init()
    {

    }
    public virtual void Open(object data)
    {
        winParams = data;
        print("base Open");
        this.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        this.gameObject.SetActive(false);
    }
}
