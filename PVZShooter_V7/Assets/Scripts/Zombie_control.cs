using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Zombie_control : MonoBehaviour
{
    // 僵尸血量100，吃脑袋后加100，可突破100上限
    private float _hp = 100;
    private float _speed = 0.5f;
    private float _damage = 10;
    public float Hp { get { return _hp; } set { _hp = value; } }
    public float Speed { get { return _speed; } set { _speed = value; } }
    public float Damage { get { return _damage; } }

    public GameObject effect_fire;
    public GameObject zombie_head_prefab;
    public GameObject zombie_head_f_prefab;
    public bool hasHead = true;

    public Animator animator;
    // 保存自身碰撞器，要比动画提前删除
    public Collider2D zombie_collider;
    public float timer = 0;
    public bool isAlive;
    public bool isAttack;
    // 保存僵尸要攻击的脑袋
    public Collider2D attackingHead;
    // 保存僵尸攻击的卡片植物
    public Collider2D attackingCardPlant;

    // 保存僵尸攻击的城墙
    public Collider2D attackingWall;

    // 保存僵尸攻击的tudou
    public Collider2D plantTudo;

    // 保存僵尸攻击的onion
    public Collider2D plantOnion;



    [System.Serializable]
    public class PlantCardMapping
    {
        public PlantType plantType;    // 植物类型
        public GameObject cardPrefab;  // 该植物对应的专属卡片预制体
    }

    public PlantType[] dropPlantTypes = new PlantType[]
    {
        PlantType.xiangrikui,
        //PlantType.Sanyecao,
       // PlantType.zhadan,
        PlantType.jianguo
    };

    public PlantCardMapping[] plantCardMappings; // 对应二维数组

    public Vector3 cardSpawnOffset = new Vector3(0, 0.5f, 0);

    private float cardDropChance = 0.3f; // 默认30%概率掉落，可在Inspector调整
    private float head_p = 0.2f;


    void Start()
    {
        animator = GetComponent<Animator>();
        if (plantCardMappings == null || plantCardMappings.Length != 4)
        {
            plantCardMappings = new PlantCardMapping[4];
            plantCardMappings[0] = new PlantCardMapping { plantType = PlantType.xiangrikui };
            plantCardMappings[1] = new PlantCardMapping { plantType = PlantType.Sanyecao };
            plantCardMappings[2] = new PlantCardMapping { plantType = PlantType.zhadan };
            plantCardMappings[3] = new PlantCardMapping { plantType = PlantType.jianguo };


        }
        // 安全检查2：检测未配置的卡片预制体
        foreach (var mapping in plantCardMappings)
        {
            if (mapping.cardPrefab == null)
            {
                return;
            }
        }
        zombie_collider = GetComponent<Collider2D>();
        isAlive = true;
        isAttack = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0)
        {
            isAlive = false;
            // 要释放正在被吃的head，不然后面的僵尸会吃不到
            if (attackingHead != null)
            {
                attackingHead.GetComponent<Head>().IsEating = false;
            }
        }
        // 到达栅栏会攻击, 碰到植物tag也要攻击，在下面碰撞代码中设isAttack=true
        /*if (transform.position.x <= -4.5f)
        {
            isAttack = true;
        }*/
        // 吃完了切换回行走状态
        if (isAttack && attackingHead == null && plantTudo == null && attackingWall == null&& plantOnion == null)
        {
            isAttack = false;
            animator.SetBool("isAttack", false); // 重置攻击动画
        }


        if (isAlive)
        {
            if (!isAttack)
            {
                transform.position += new Vector3(-Time.deltaTime * Speed, 0, 0);
            }
            if (isAttack)
            {
                animator.SetBool("isAttack", true);
                // print("攻击");
                // 造成伤害
                if (attackingHead != null)
                {
                    zombieEatHead(attackingHead);
                }
                if (attackingWall != null)
                {
                    zombieAttackWall(attackingWall);
                }
                if (plantTudo != null)//土豆
                {
                    zombieAttackTudo(plantTudo);
                }
                if (plantOnion != null)//洋葱
                {
                    zombieAttackOnion(plantOnion);
                }

            }
        }
        if (!isAlive)
        {
            if (zombie_collider != null && zombie_collider.enabled == true)
            {
                // 死亡后要立即删掉碰撞器，动画要正常播放，但不能继续阻挡子弹
                zombie_collider.enabled = false;
            }

            if (!isAttack)
            {
                animator.SetTrigger("lostHead");
                // 头只掉一次
                if (hasHead)
                {
                    
                    hasHead = false;
                    if (Random.value < head_p)
                    {
                        GameObject zombie_head = Instantiate(zombie_head_prefab, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        GameObject zombie_f_head = Instantiate(zombie_head_f_prefab, transform.position, Quaternion.identity);
                        Destroy(zombie_f_head, 2);
                    }
                }
                timer += Time.deltaTime;
                if (timer >= 1)
                {
                    animator.SetTrigger("die");
                }
                if (timer >= 2)
                {
                    SpawnRandomPlantCard();//掉落卡片
                    
                    if (ScoreManager.Instance != null)//计分
                    {
                        ScoreManager.Instance.AddZombieKillScore();
                    }
                    Destroy(gameObject);
                }
            }
            if (isAttack)
            {
                animator.SetTrigger("lostHead_attack");
                if (hasHead)
                {
                    hasHead = false;
                    if (Random.value < head_p)
                    { 
                        GameObject zombie_head = Instantiate(zombie_head_prefab, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        GameObject zombie_f_head = Instantiate(zombie_head_f_prefab, transform.position, Quaternion.identity);
                        Destroy(zombie_f_head, 2);
                    }
                    // 最终：10%概率不销毁头，10%概率调用生成卡片，80%正常销毁
                }
                timer += Time.deltaTime;
                if (timer >= 1)
                {
                    animator.SetTrigger("die");
                }
                if (timer >= 2)
                {
                    SpawnRandomPlantCard(); // 调用生成卡片方法
                    if (ScoreManager.Instance != null)//计分
                    {
                        ScoreManager.Instance.AddZombieKillScore();
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    public void Decelerate()
    {
        // 加一个bool,使冷冻状态僵尸的头也是冷冻状态
        SpriteRenderer zombie_spriteRenderer = GetComponent<SpriteRenderer>();
        if (zombie_spriteRenderer != null)
        {
            Color color = new Color(0.12041f, 0.335754f, 0.773584f, 1f);
            zombie_spriteRenderer.color = color;
            Speed = 0.5f;
        }
    }

    public void Fired()
    {
        GameObject  effect = Instantiate(effect_fire, transform.position+new Vector3 (0.3f,-0.5f,0), Quaternion.identity);
        effect.transform.SetParent(transform);
        Destroy(effect,2);
    }
    // 闪电单体特效放在闪电子弹里了, 后续要做闪电链条的话可能需要在这里加
    public void lightning()
    {
    }

    // 僵尸对脑袋造成伤害
    public void zombieEatHead(Collider2D whichHead)
    {

        
        Head head = whichHead.GetComponent<Head>();
        if (head != null)
        {
            float headHp = head.Hp;
            if (headHp > 0)
            {
                whichHead.GetComponent<Head>().Hp = headHp - Damage * Time.deltaTime;
            }
            // 简易版回血，小于1即可加hp, 模拟加100hp，后续看有没有其他方法
            if (headHp > 0 && headHp < 1)
            {
                Hp += 1.8f;
            }
        }
        else
        {
            print("未获取Head对象");
        }
        
    }

    // 僵尸对城墙造成伤害
    public void zombieAttackWall(Collider2D wall)
    {
        Wall_control wall_control = wall.GetComponent<Wall_control>();
        if (wall_control != null)
        {
            float wallHp = wall_control.Hp;
            if (wallHp > 0)
            {
                wall.GetComponent<Wall_control>().Hp = wallHp -Damage * Time.deltaTime;
            }
        }
    }

    // 僵尸对土豆造成伤害
    public void zombieAttackTudo(Collider2D colide)
    {
        tudou tudou_control = colide.GetComponent<tudou>();
        if (tudou_control != null)
        {
            float tudou_hp = tudou_control.tudou_hp;
            if (tudou_hp > 0)
            {
                tudou_control.GetComponent<tudou>().tudou_hp = tudou_hp - Damage * Time.deltaTime;
            }
        }
    }


    // 僵尸对洋葱造成伤害
    public void zombieAttackOnion(Collider2D colide)
    {
        onion onion_control = colide.GetComponent<onion>();
        if (onion_control != null)
        {
            float hp = onion_control.onion_hp;
            if (hp > 0)
            {
                onion_control.GetComponent<onion>().onion_hp = hp - Damage * Time.deltaTime;
            }
        }
    }


    // 碰到植物攻击
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // 排除在攻击的过程中滚过来的脑袋
        if (collision.gameObject?.CompareTag("head") == true && !isAttack)
        {
            print("碰到脑袋！");
            // Head脚本的IsEating标签，看看是否正在被吃
            bool isEating = collision.GetComponent<Head>().IsEating;
            if (!isEating)
            {
                print("脑袋可以吃");
                isAttack = true;
                collision.GetComponent<Head>().IsEating = true;
                // 赋值要吃的脑袋
                attackingHead = collision;
            }
        }
        else if (collision.gameObject?.CompareTag("wall") == true)
        {
            print("碰到城墙");
            isAttack = true;
            // 加上扣城墙hp
            attackingWall = collision;
        }
        // else if 碰到其他如"plant", 执行各自功能, 将碰到的collision传递给对应的变量保存
        else if (collision.gameObject?.CompareTag("tudou") == true)
        {
          
               print("碰到植物");
               isAttack = true;
                plantTudo = collision;//拿到碰撞器
           

        }
        else if (collision.gameObject?.CompareTag("onion") == true)
        {
            print("碰到植物");
            isAttack = true;
            plantOnion = collision;//拿到碰撞器
        }

    }




    // 核心方法：随机掉落四种植物对应的专属卡片
    private void SpawnRandomPlantCard()
    {


        if (Random.value > cardDropChance)
        {
            Debug.Log("本次未触发卡片掉落（概率不足）");
            return; // 直接返回，不生成卡片
        }

        if (plantCardMappings == null || plantCardMappings.Length == 0)
        {
            Debug.LogError("植物-卡片映射数组未配置！");
            return;
        }


        int randomIndex = Random.Range(0, plantCardMappings.Length);
        randomIndex = 3;

        PlantCardMapping selectedMapping = plantCardMappings[randomIndex];


        if (selectedMapping.cardPrefab == null)
        {
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            return;
        }


        GameObject newCard = Instantiate(selectedMapping.cardPrefab, canvas.transform);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position + cardSpawnOffset);
        newCard.GetComponent<RectTransform>().position = screenPos;


        card cardComponent = newCard.GetComponent<card>();
        if (cardComponent != null)
        {
            cardComponent.planttype = selectedMapping.plantType;
        }
        else
        {
            return;
        }
    }


    //僵尸被移动
    public void move_zombie(Vector3 movedistance)
    { 
        
       gameObject.transform.position += movedistance;
    }



}

