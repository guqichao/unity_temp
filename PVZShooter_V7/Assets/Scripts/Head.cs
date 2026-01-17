using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    private bool _isEating = false;
    public bool IsEating { get { return _isEating; } set { _isEating = value; } }
    // 僵尸攻击为10，脑袋hp为30，攻击3秒
    private float _hp = 30; 
    public float Hp { get { return _hp; } set { _hp = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0)
        {
            //print(Hp);
            Destroy(gameObject);
            // 持续一定时间必须销毁
        }
    }

}
