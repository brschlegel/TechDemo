using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DropSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor()
    {
        switch(gameObject.GetComponent<Dropdown>().value)
        {
        
            case 0:
                Raycaster.CurrentColor = Color.red;
                break;

            case 1:
                Raycaster.CurrentColor =  Color.blue;
                break;

            case 2:
                Raycaster.CurrentColor = Color.green;
                break;

        }
    }
}
