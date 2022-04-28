using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comprtementMenisque : MonoBehaviour
{
    public Material matFill;
    public float offset;
    //public Material monMat;
    //Color monMatColor;
    private void Update()
    { 
        transform.position = new Vector3(transform.position.x, (4*matFill.GetFloat("_fill"))+offset+(transform.parent.transform.position.y), transform.position.z);
    }
}
