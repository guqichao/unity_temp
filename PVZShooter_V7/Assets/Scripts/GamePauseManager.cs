using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 修复版暂停菜单管理器（解决场景加载卡死问题）
/// </summary>
public class GamePauseManager : MonoBehaviour
{
    [Header("UI组件赋值")]
    public GameObject pauseMenuPanel;
    public Button openMenuButton;

    private bool isGamePaused = false;
    private Coroutine loadSceneCoroutine; // 场景加载协程（用于取消）

    private void Start()
    {
        if (pauseMenuPanel == null || openMenuButton == null)
        {
            Debug.LogError("请给暂停菜单的UI组件赋值！");
            return;
        }
        pauseMenuPanel.SetActive(false);
        openMenuButton.onClick.AddListener(OnOpenMenuButtonClicked);
        BindPauseMenuButtons();
    }

    // 打开菜单（逻辑不变） ok
    private void OnOpenMenuButtonClicked()
    {
        if (!isGamePaused)
        {
            Time.timeScale = 0f;
            pauseMenuPanel.SetActive(true);
            openMenuButton.gameObject.SetActive(false);
            isGamePaused = true;
        }
    }

    // 返回游戏（逻辑不变） ok
    public void ResumeGame()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1f;
            pauseMenuPanel.SetActive(false);
            openMenuButton.gameObject.SetActive(true);
            isGamePaused = false;
        }
    }

    // 重新开始游戏（修复版） ok
    public void RestartGame()
    {
        Time.timeScale = 1f;
        // 1. 先停止僵尸生成协程（核心新增）
        if (Generater.Instance != null)
        {
            // Generater.Instance.StopGenerate();
            GameObject temp = GameObject.Find("Manager");
           
            Destroy(temp);
        }
        StopAllCoroutines();
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
    }

    // 【核心修改】返回主菜单按钮→改为结束整个游戏
    public void BackToMainMenu()
    {
        // 1. 先恢复时间缩放（避免退出前游戏仍暂停）
        Time.timeScale = 1f;

        // 2. 停止所有协程、清理残留逻辑
        StopAllCoroutines();
        if (Generater.Instance != null)
        {
            GameObject temp = GameObject.Find("Manager");
            if (temp != null) Destroy(temp);
        }

        // 3. 结束游戏（适配编辑器/打包后环境）
        Debug.Log("开始退出游戏...");
        // 编辑器中：停止运行模式
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        // 打包后（PC/移动端）：调用系统退出方法
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// 异步加载场景（仅用于重新开始游戏，逻辑不变）
    /// </summary>
    /// <param name="sceneIndex">场景索引</param>
    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return null;

        if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"场景索引{sceneIndex}无效！请检查Build Settings");
            pauseMenuPanel.SetActive(true);
            yield break;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        isGamePaused = false;
        loadSceneCoroutine = null;
    }

    // 绑定按钮（逻辑不变）
    private void BindPauseMenuButtons()
    {
        Button resumeBtn = pauseMenuPanel.transform.Find("ResumeBtn")?.GetComponent<Button>();
        if (resumeBtn != null) resumeBtn.onClick.AddListener(ResumeGame);
        else Debug.LogError("未找到ResumeBtn按钮！");

        Button restartBtn = pauseMenuPanel.transform.Find("RestartBtn")?.GetComponent<Button>();
        if (restartBtn != null) restartBtn.onClick.AddListener(RestartGame);
        else Debug.LogError("未找到RestartBtn按钮！");

        Button backToMainBtn = pauseMenuPanel.transform.Find("BackToMainBtn")?.GetComponent<Button>();
        if (backToMainBtn != null) backToMainBtn.onClick.AddListener(BackToMainMenu);
        else Debug.LogError("未找到BackToMainBtn按钮！");
    }

    // ESC恢复游戏（逻辑不变）
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGamePaused)
        {
            ResumeGame();
        }
    }

    // 场景卸载时清理协程（逻辑不变）
    private void OnDestroy()
    {
        StopAllCoroutines();
        loadSceneCoroutine = null;
    }
}