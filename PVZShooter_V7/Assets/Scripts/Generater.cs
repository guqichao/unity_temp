using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Generater : MonoBehaviour
{
    public static Generater Instance { get; private set; }

    //位置集合
    public Transform[] generatePointList;
    public GameObject zombiePrefab;
    private Coroutine generateCoroutine;
    private bool isstop = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartGenerate();
    }

    public void StartGenerate()
    {
        if (generateCoroutine == null)
        {
            generateCoroutine = StartCoroutine(GenerateZombie());
        }
    }

    //协程
    private int increase = 3;//僵尸波次
    IEnumerator GenerateZombie()
    {
        /*//第一波 
        for (int i = 0; i < 10; i++)
        {
            GenerateARandomZombie();
            yield return new WaitForSeconds(3);
        }
        yield return new WaitForSeconds(3);
        //第二波 
        for (int i = 0; i < 10; i++)
        {
            GenerateARandomZombie();
            yield return new WaitForSeconds(3);
        }
        yield return new WaitForSeconds(3);
        //第三波 
        for (int i = 0; i < 10; i++)
        {
            GenerateARandomZombie();
            yield return new WaitForSeconds(3);
        }*/
        
        while (!isstop)
        {
            for (int i = 0; i < increase; i++)
            {
                increase += 6;
                GenerateARandomZombie();
                yield return new WaitForSeconds(2);
            }
        }
    }
    private void GenerateARandomZombie()
    {
        /*int[] indexs = new int[5];
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, generatePointList.Length);
            indexs[i] = index;
        }
        for (int i = 0; i < 3; i++)
        {
            GameObject.Instantiate(zombiePrefab, generatePointList[indexs[i]].position, Quaternion.identity);
        }*/
        int index = Random.Range(0, generatePointList.Length);
        GameObject.Instantiate(zombiePrefab, generatePointList[index].position, Quaternion.identity);

    }

    // 停止协程, 其他脚本可以调用这个
    public void StopGenerate()
    {
        if (generateCoroutine != null)
        {
            isstop = true;
            StopCoroutine(generateCoroutine);
            generateCoroutine = null;
            print("协程停止");
        }
    }
    // 对象销毁时自动调用
    private void OnDestroy()
    {
        StopGenerate();
    }

}