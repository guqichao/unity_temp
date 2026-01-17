using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlantType//植物类型
{ 
    xiangrikui,//向日葵
    wandousheshou,//豌豆射手
    wogua,//窝瓜
    zhadan,//樱桃炸弹
    jianguo,//坚果
    hanbingsheshou,//寒冰射手
    Sanyecao


}

public class card : MonoBehaviour
{
    public PlantType planttype = PlantType.xiangrikui;//默认一开始植物类型向日葵
    public void Oclick_xiangrikui()//向日葵点击
    {
        //种植
       bool issuccess =  HandManager.instance.AddPlant(planttype);//种植植物
        if (issuccess)//种植成功
        {
            Destroy(gameObject);
        }
    }


    public void Oclick_wogua()//窝瓜点击
    {
        //种植
        bool issuccess = HandManager.instance.AddPlant(planttype);//种植植物
        if (issuccess)//种植成功
        {
            Destroy(gameObject);
        }
    }

    public void Oclick_tudou()//土豆点击
    {
        //种植
        bool issuccess = HandManager.instance.AddPlant(planttype);//种植植物
        if (issuccess)//种植成功
        {
            Destroy(gameObject);
        }
    }

    public void Oclick_zhadan()//炸弹点击
    {
        //种植
        bool issuccess = HandManager.instance.AddPlant(planttype);//种植植物
        if (issuccess)//种植成功
        {
            Destroy(gameObject);
        }
    }

    public void Oclick_sanyecao()//三叶草点击
    {
        //种植
        bool issuccess = HandManager.instance.AddPlant(planttype);//种植植物
        if (issuccess)//种植成功
        {
            Destroy(gameObject);
        }
    }

}
