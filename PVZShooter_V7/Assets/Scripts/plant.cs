using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Plantstate
{ 
  disable,//点击状态
  enable//种植状态
}

//植物相关
public class plant : MonoBehaviour
{
    //两种状态 不可用状态 可用状态
    Plantstate plantstate = Plantstate.disable;
    public PlantType planttype = PlantType.Sanyecao;//默认向日葵


    private void Start()//一开始状态
    {
        TranstitionToDisable();//一开始卡片是disable状态
    }


    private void Upstate()
    {
        switch (plantstate)
        { 
           case Plantstate.disable: disableupdate(); break;//状态1
           case Plantstate.enable: enableupdate(); break;//状态2
           default:break;
        }
    }

    void disableupdate()//不可选中更新
    { 
       
    }

    void enableupdate()//选中更新
    {

    }

    public void TranstitionToDisable()//转换到disable状态
    {
        plantstate = Plantstate.disable;//禁用状态
        GetComponent<Animator>().enabled = false;//禁用动画
    }

    public void TranstitionToEnable()//转换到enable状态
    {
        plantstate = Plantstate.enable;//禁用状态
        GetComponent<Animator>().enabled = true;//启用动画
    }

}
