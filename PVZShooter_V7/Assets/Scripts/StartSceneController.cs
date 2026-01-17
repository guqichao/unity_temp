using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    //检测按钮点击
    public void OnStartButtonClick()
    {
        //先得到当前场景,加载下一个场景 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
