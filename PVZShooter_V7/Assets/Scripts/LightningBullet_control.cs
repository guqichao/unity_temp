using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class LightningBullet_control : MonoBehaviour
{
    private float _speed = 5;
    public float Speed { get { return _speed; } }
    public float Damage { get; } = 30;
    // 闪电特效
    public GameObject effect_lightning;

    // 本颗子弹首次碰撞标记
    private bool hasTriggeredCollision = false;
    // 实时记录所有僵尸位置
    private Dictionary<GameObject, Vector3> zombiePosDict = new Dictionary<GameObject, Vector3>();
    

    // Start is called before the first frame update
    void Start()
    {
        UpdateZombiePosDict();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
        UpdateZombiePosDict();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 后面做卡片植物的碰撞器，要先判断tag是否是zombie，所有类型子弹都要记得弄
        if (collision.gameObject?.CompareTag("zombie") == true)
        {
            if (!hasTriggeredCollision)
            {
                // 一个子弹只碰一次，随后只隐藏不销毁
                this.GetComponent<SpriteRenderer>().enabled = false;
                hasTriggeredCollision = true;
                //GameObject effect = Instantiate(Effet_peabullet, transform.position, Quaternion.identity);
                Zombie_control zombie_control = collision.GetComponent<Zombie_control>();
                GameObject hitzombie;
                if (zombie_control != null)
                {
                    hitzombie = zombie_control.gameObject;
                    List<GameObject> result = new List<GameObject>();
                    result = getNearestZombie(hitzombie);
                    // 对至多5个僵尸形成闪电，要是有时间可将几个闪电特效加金色链条
                    //闪电没持续伤害，特效立即消失
                    foreach (GameObject zombie in result)
                    {
                        if (zombie!=null)
                        {
                            Zombie_control result_zombie_control = zombie.GetComponent<Zombie_control>();
                            result_zombie_control.Hp -= Damage;
                            GameObject effect = Instantiate(effect_lightning, zombie.transform.position, Quaternion.identity);
                            Destroy(effect, 0.2f);
                        }
                    }
                }
                Destroy(gameObject);
            }
        }
        
    }

    private void UpdateZombiePosDict()
    {
        // 每次都清空重新统计，因为有对象在销毁
        zombiePosDict.Clear();

        GameObject[] allZombies = GameObject.FindGameObjectsWithTag("zombie");
        //print($"当前有{allZombies.Length}个僵尸");

        foreach (GameObject zombie in allZombies)
        {
            if (zombie == null || !zombie.activeSelf)
            {
                continue;
            }

            Vector3 zombiePos = zombie.transform.position;
            if (!zombiePosDict.ContainsKey(zombie)) 
            {
                zombiePosDict.Add(zombie, zombiePos);
            }
            else
            {
                // 存在的话，更新一下位置
                zombiePosDict[zombie] = zombiePos;
            }
        }
        if (allZombies.Length != zombiePosDict.Count) print("!!!!!!!!!!!!!!!!");
    }

    private List<GameObject> getNearestZombie(GameObject hitZombie)
    {
        List<GameObject> result = new List<GameObject>();
        result.Add(hitZombie);
        // 计数，初始已有一个被击中僵尸
        int num = 1;
        Vector3 hit_pos = zombiePosDict[hitZombie];
        foreach (var zombie in zombiePosDict)
        {
            if (num == 5)
            {
                break;
            }
            GameObject zombie_k = zombie.Key;
            Vector3 zombie_v = zombie.Value;
            Vector2 srcZombiePos = new Vector2(hitZombie.transform.position.x, hitZombie.transform.position.y);
            Vector2 desZombiePos = new Vector2(zombie_v.x, zombie_v.y);
            float distance = (desZombiePos - srcZombiePos).sqrMagnitude;
            if (distance < 10)//闪电子弹范围
            {
                result.Add(zombie_k);
                num++;
            }
        }
        //print($"攻击了{result.Count}个僵尸");
        return result;
    }
}
