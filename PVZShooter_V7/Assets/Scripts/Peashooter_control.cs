using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter_control : MonoBehaviour
{
    public float _ShootInterval { get; } = 0.5f;
    private float currentTime = 0;

    public Transform shootPoint;
    public int _TypeOfBullet { get; set; } = 4; //默认为4，调试用
    public Bullet_control BulletPrefab;//普通子弹
    public FireBullet_control FireBulletPrefab; //火焰子弹
    public IceBullet_control IceBulletPrefab; //寒冰子弹
    public LightningBullet_control LightningBulletPrefab; //闪电子弹

    private bool sameFrame;
    private float moveDistance = 1.6f;
    private int posLimit = 2;

    private float bulletSpeed = 8f;

    void Start()
    {

    }

    void Update()
    {
        // 移动
        sameFrame = false;
        move();
        // 切换子弹
        altBullet();

        // currentTime += Time.deltaTime;
        // if (currentTime > _ShootInterval)
        // {
        //     shoot();
        //     currentTime = 0;
        // }

        // 每隔1秒检测有没有点击
        currentTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && currentTime > _ShootInterval) // 左键
        {
            currentTime = 0;
            // 向鼠标点击位置发射
            ShootAtMousePosition();
        }
    }

    void Shoot(Vector3 targetDirection)
    {
        targetDirection = targetDirection.normalized;

        switch (_TypeOfBullet)
        {
            case 1:
                if (BulletPrefab != null)
                {
                    Bullet_control bullet = Instantiate(BulletPrefab, shootPoint.position, Quaternion.identity);
                    SetBulletMoveDirection(bullet.gameObject, targetDirection);
                }
                break;
            case 2:
                if (FireBulletPrefab != null)
                {
                    FireBullet_control fireBullet = Instantiate(FireBulletPrefab, shootPoint.position, Quaternion.identity);
                    SetBulletMoveDirection(fireBullet.gameObject, targetDirection);
                }
                break;
            case 3:
                if (IceBulletPrefab != null)
                {
                    IceBullet_control iceBullet = Instantiate(IceBulletPrefab, shootPoint.position, Quaternion.identity);
                    SetBulletMoveDirection(iceBullet.gameObject, targetDirection);
                }
                break;
            case 4:
                if (LightningBulletPrefab != null)
                {
                    LightningBullet_control lightningBullet = Instantiate(LightningBulletPrefab, shootPoint.position, Quaternion.identity);
                    SetBulletMoveDirection(lightningBullet.gameObject, targetDirection);
                }
                break;
        }
    }

    // 计算点击位置, 发射
    void ShootAtMousePosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        Vector3 shootDirection = mouseWorldPos - shootPoint.position;
        shootDirection.z = 0;

        // 发射子弹
        if (shootDirection.magnitude > 0.1f)
        {
            Shoot(shootDirection);
        }
    }

    void SetBulletMoveDirection(GameObject bulletObj, Vector3 direction)
    {
        Rigidbody2D rb2D = bulletObj.GetComponent<Rigidbody2D>();
        if (rb2D != null)
        {
            rb2D.velocity = direction * bulletSpeed;
        }
    }

    void move()
    {
        if (sameFrame) return;
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            if (posLimit < 4)
            {
                transform.position += new Vector3(0, moveDistance, 0);
                sameFrame = true;
                posLimit += 1;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
        {
            if (posLimit > 0)
            {
                transform.position += new Vector3(0, -moveDistance, 0);
                sameFrame = true;
                posLimit -= 1;
            }
        }
    }

    void altBullet()
    {
        if (sameFrame) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _TypeOfBullet = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _TypeOfBullet = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _TypeOfBullet = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _TypeOfBullet = 4;
        }
    }
}


