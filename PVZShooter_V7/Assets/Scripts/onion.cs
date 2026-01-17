using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class onion : MonoBehaviour
{
    //大蒜

    public float onion_hp = 500;//土豆血量

    // Update is called once per frame
    void Update()
    {
        if (onion_hp < 0)
        {
            Destroy(gameObject);//消失
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("zombie") == true)
        {

            Zombie_control zombie = collision.GetComponent<Zombie_control>();
            zombie.move_zombie(new Vector3(0, 1.6f, 0));
        }
    }
}
