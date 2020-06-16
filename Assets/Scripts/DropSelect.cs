using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DropSelect : MonoBehaviour
{
    Raycaster cast;
    void Start()
    {
        cast = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Raycaster>();
    }
  
    public void SetColor()
    {
        switch(gameObject.GetComponent<Dropdown>().value)
        {
        
            case 0:
                cast.CurrentColor = Color.red;
                break;

            case 1:
                cast.CurrentColor =  Color.blue;
                break;

            case 2:
                cast.CurrentColor = Color.green;
                break;

        }
    }
}
