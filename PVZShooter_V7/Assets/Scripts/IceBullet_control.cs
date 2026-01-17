using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet_control : MonoBehaviour
{
    private float _speed = 5;
    public float Speed { get { return _speed; } }
    public float Damage { get; } = 20;
    // 子弹穿透计数
    private int num_of_collision = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 后面做卡片植物的碰撞器,要设成plant，避免被子弹打到
        if (collision.gameObject?.CompareTag("zombie") == true)
        {
            if (num_of_collision < 3)
            {
                num_of_collision++;
                Zombie_control zombie_control = collision.GetComponent<Zombie_control>();
                zombie_control.Hp -= Damage;
                zombie_control.Decelerate();
            }
            if (num_of_collision >= 3)
            {
                Destroy(gameObject);
            }
        }
        
        
        
    }
}
