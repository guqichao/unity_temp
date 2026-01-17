using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//场景 
//1 进入游戏  1
//2 菜单      2
//3 白天模式  3
//4 黑夜模式  4


//菜单脚本
public class MenuScenrController : MonoBehaviour
{
    public GameObject inputPanelGo;
    public TMP_InputField nameInputField;
    public TextMeshProUGUI nameText;

    private void Start()//一开始更新名字
    {
        updateNameUI();
    }

    public void OnChangeNameButtonClick()//名字修改
    {
        string name = PlayerPrefs.GetString("name", " ");
        nameText.text = name;//更新名字
        bool currentState = inputPanelGo.activeSelf;//获取当前面板状态
        inputPanelGo.SetActive(!currentState);//显示面板
        //audiomanager.Instance.PlayClip(Config.btn_click);//播放点击音乐
    }

    public void OnSubmitButtonClick()//修改点击
    {

        PlayerPrefs.SetString("name", nameInputField.text);
        inputPanelGo.SetActive(false);//隐藏面板
        updateNameUI();//名字更新
    }

    //用户名显示
    void updateNameUI()
    {
        string name =PlayerPrefs.GetString("name", "-");//默认状态
        nameText.text = name;//更新名字
    }

    //-------------------------关卡进入-----------------//
    public void OnFirstMisssion()//进入黑夜模式
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);//场景切换
       
    }

    public void OnSecondMisssion()//进入白天模式
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);//场景切换

    }
}
