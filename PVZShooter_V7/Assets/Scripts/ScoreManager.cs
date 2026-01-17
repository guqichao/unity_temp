using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // 单例：全局唯一的计分管理器
    public static ScoreManager Instance;

    [Header("计分配置")]
    private int killZombieCount = 0; // 击杀僵尸总数

    [Header("UI显示引用")]
    // 若用TextMeshPro，改为 public TextMeshProUGUI killScoreText;
    public TextMeshProUGUI killScoreText;

    void Awake()
    {
        // 单例初始化：确保只有一个实例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切换场景不销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 初始化计分UI显示
        UpdateScoreUI();
    }

    // 对外暴露的加分方法（僵尸被打死时调用）
    public void AddZombieKillScore(int addCount = 1)
    {
        killZombieCount += addCount;
        UpdateScoreUI(); // 更新UI显示
    }

    // 更新计分UI文字
    private void UpdateScoreUI()
    {
        if (killScoreText != null)
        {
            killScoreText.text = $"Kill:{killZombieCount}";
        }
    }

    // 可选：重置分数（比如重新开始游戏时调用）
    public void ResetScore()
    {
        killZombieCount = 0;
        UpdateScoreUI();
    }
}