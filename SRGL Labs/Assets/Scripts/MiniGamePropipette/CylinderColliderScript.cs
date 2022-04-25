using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderColliderScript : MonoBehaviour
{
    public bool isPlaced = false;
    public ScrollPipette scroll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        scroll.step = 0.05f;
        isPlaced = true;
    }
    private void OnTriggerExit(Collider other)
    {
        scroll.step = 0.1f;
        isPlaced = false;
    }
}
