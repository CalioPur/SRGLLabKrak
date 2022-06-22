using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingTool : MonoBehaviour
{
    //************************************************************* VARIABLES

    //original placement (si pas placeholder)
    public Vector3 originalPlacement;

    //contains
    public bool isFull = false;

    public string containsName; //can be changed to dictionary later if needed

    public float containsQuantity;

    //spoon only ?
    public GameObject objectHeldWithin;

    //shader
    public float fillingValue; //entre 0 et 1 -> quantité de fill

    //************************************************************* FONCTIONS

    private void Start()
    {
        originalPlacement = transform.position;
        if (!isFull)
        {
            objectHeldWithin.SetActive(false);
        }
    }

    public void FillObject(string putInName, float putInQuatity) // remplir
    {
        objectHeldWithin.SetActive(true);
        this.containsName = putInName;
        this.containsQuantity = putInQuatity;
        this.isFull = true;
    }

    public void EmptyObject() //vider
    {
        objectHeldWithin.SetActive(false);
        this.containsName = null;
        this.containsQuantity = 0;
        this.isFull = false;
    }

}
