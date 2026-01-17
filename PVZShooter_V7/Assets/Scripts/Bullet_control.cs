using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_control : MonoBehaviour
{
    private float _speed = 5;
    public float Speed { get { return _speed; } }
    public float Damage { get; } = 100;
    public GameObject Effet_peabullet;
    // 本颗子弹首次碰撞标记
    private bool hasTriggeredCollision = false;

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
            if (!hasTriggeredCollision)
            {
                // 一个子弹只用在一次碰撞，不然会是群体伤害
                hasTriggeredCollision = true;
                GameObject effect = Instantiate(Effet_peabullet, transform.position, Quaternion.identity);
                Zombie_control zombie_control = collision.GetComponent<Zombie_control>();
                if (zombie_control != null)
                {
                    zombie_control.Hp -= Damage;
                }
                Destroy(gameObject);
                Destroy(effect, 0.1f);
            }
        }
        
        
        
    }
}
