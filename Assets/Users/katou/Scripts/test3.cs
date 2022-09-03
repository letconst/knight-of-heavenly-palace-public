using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Material material = GetComponent<Image>().material;
        HPbar.SetAmount(material, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
