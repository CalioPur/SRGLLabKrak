using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColliderScript : MonoBehaviour
{

    public bool isBroken;
    // Start is called before the first frame update
    void Start()
    {
        isBroken = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isBroken);
    }

    private void OnTriggerEnter(Collider other)
    {
        isBroken = true;
    }

}
