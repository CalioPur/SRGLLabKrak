using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScrollPipette : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,5.6f,0);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Mouse.current.scroll.ReadValue().y > 0)
        {
            
        }*/
    }
}
