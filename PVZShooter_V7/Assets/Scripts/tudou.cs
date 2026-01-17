using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class tudou : MonoBehaviour
{

    public  float tudou_hp=100;//土豆血量

    public void Update()
    {
        print($"土豆血量:{tudou_hp}");
        if (tudou_hp < 0)
        {
            Destroy(gameObject);//消失
        }
    }



}