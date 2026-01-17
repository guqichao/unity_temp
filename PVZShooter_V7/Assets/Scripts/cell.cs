using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell : MonoBehaviour
{
    public plant currentPlant;
    private void OnMouseDown()
    {
        print("11");
        HandManager.instance.OnCellClick(this);  
    }

    public  bool  AddPlant(plant  p1)//ÖÖÖ²
    { 
       if(currentPlant != null) return false;
       currentPlant = p1;
        currentPlant.transform.position = transform.position;
        p1.TranstitionToEnable();
        return true;

    }
}
