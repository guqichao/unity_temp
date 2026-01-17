using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager instance { get; private set; }//私有get方法

    //所有植物perfab集合
    public List<plant> plantPrefabList;

    private plant currentplant;//当前要种植的植物


    private void Awake()
    {
        instance = this;
    }

    private void Update()//植物跟随鼠标
    {
        FollowCursor();
    }

    
    public bool AddPlant(PlantType planttype)//添加植物
    {
        //判断手上是否有植物
        if (currentplant != null) return false;
      

        plant plantprefab = GetPlantPrefab(planttype);
        if (plantprefab == null)//为空
        {
            print("种植植物不存在111111");return false ; 
        }
        

        currentplant = GameObject.Instantiate(plantprefab);//当前植物实例化
       
        return true;
    }

    private plant GetPlantPrefab(PlantType planttype)//获取植物
    {
        print($"传入的是{planttype}");
        foreach (plant Plant in plantPrefabList)
        {
            print(Plant);
            if (Plant.planttype == planttype)
            {
                print(planttype);
                return Plant;
            }
        }
        return null;
    }

    void FollowCursor()//鼠标跟随
    {
        if (currentplant == null)//没有植物
        {
            return;
        }
        //获取鼠标位置  世界坐标


        Vector3 mouseworldposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseworldposition.z = 0;
        currentplant.transform.position = mouseworldposition;
       
    }

    public void OnCellClick(cell c1)//种植
    {
        if (currentplant == null) return;
       
        bool issuccess = c1.AddPlant(currentplant);//种植
        if (issuccess)
        {
            currentplant = null;
        }
        
    }  

}
