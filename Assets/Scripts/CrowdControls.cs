using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControls : MonoBehaviour
{
    public GameObject CrowdPrefab;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(CrowdPrefab, new Vector3(0,.5f,0), Quaternion.identity);
        }

       
    }
}
