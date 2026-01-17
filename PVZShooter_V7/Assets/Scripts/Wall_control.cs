using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro; 

public class Wall_control : MonoBehaviour
{
 
    private float _hp = 5000;//血量


    public float maxHp = 5000;
    public float Hp
    {
        get { return _hp; }
        set
        {
            _hp = Mathf.Max(value, 0); // 新增：防止血量小于0
            UpdateHealthUI(); // 新增：血量变化时同步更新血条
        }
    }

    // 新增：UI血条和文字引用
    [Header("UI血量显示")]
    public Slider wallHealthSlider; // 城墙血条
    //public Text wallHealthText;     // 血量文字（若用TMP则改为 TextMeshProUGUI）
    public TextMeshProUGUI wallHealthText;

    //public float Hp {  get { return _hp; } set { _hp = value; } }
    // 有时间做成动画prefab
    public GameObject zombieWin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // print($"城墙HP={Hp}");
        if (Hp <= 0)
        {
            Destroy(gameObject);
            Instantiate(zombieWin,new Vector3(0,0,0), Quaternion.identity);
            Time.timeScale = 0;
        }
    }


    // 新增：更新UI血条和文字的核心方法
    private void UpdateHealthUI()
    {

        // 新增日志：检查组件是否绑定
       // Debug.Log($"血条组件是否绑定：{wallHealthSlider != null}");
       // Debug.Log($"文字组件是否绑定：{wallHealthText != null}");
       // Debug.Log($"当前城墙血量：{_hp}，最大血量：{maxHp}，血条比例：{_hp / maxHp}");

        // 安全检查：避免空引用报错
        if (wallHealthSlider != null)
        {
            // 血条值是0-1的比例（当前血量/最大血量）
            wallHealthSlider.value = _hp / maxHp;
        }

        if (wallHealthText != null)
        {
            // 显示格式：当前血量/最大血量（如 450/500）
            wallHealthText.text = $"{Mathf.Round(_hp)}/{maxHp}";
        }
    }

}
