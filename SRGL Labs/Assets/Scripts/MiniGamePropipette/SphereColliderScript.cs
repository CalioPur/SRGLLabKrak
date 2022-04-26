using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColliderScript : MonoBehaviour
{
    public Material mat;
    public bool isBroken;
    // Start is called before the first frame update
    void Start()
    {
        isBroken = false;
    }

    private void Update()
    {
        if (mat.GetFloat("_fill") > 0.8f)
        {
            isBroken = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isBroken = true;
    }

}
