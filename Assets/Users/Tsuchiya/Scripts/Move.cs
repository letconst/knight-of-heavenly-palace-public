using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    void Update()
    {
        
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(new Vector3(-20f * Time.deltaTime, 0f, 0f));
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(new Vector3(20f * Time.deltaTime, 0f, 0f));
        }       
        
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(new Vector3(0f , 0f , 20f * Time.deltaTime));
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(new Vector3(0f, 0f , -20f * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.Translate(new Vector3(0f, 20f * Time.deltaTime, 0f ));
        }


    }
}
