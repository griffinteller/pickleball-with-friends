using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefabTest : MonoBehaviour
{
   public GameObject vertical;
   public GameObject horizontal;
   public int width = 10;
   public double height = 1.5;
  
   void Start()
   {
       for (float y=0; y<=height; y+=0.125f)
       {
       	Instantiate(horizontal, new Vector3(0.0f, y+0.025f, 0.0f), Quaternion.identity);
       }
       
        for (float z=0; z<=width; z+=0.125f)
        {
        Instantiate(vertical, new Vector3(0.0f,0.75f,z-5.0f), Quaternion.identity);
        }
              
   


   }
}
