using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerObjectScript : MonoBehaviour
{
    //************************************************************* VARIABLES

    //nom (may be removed if directly name of game object)
    public string containerName;

    //bouchon
    public bool capIsOn = false;

    //danger
    public int danger;

    //melange
    public bool wasMixed = false;

    //fill (% ?) (taux de remplissage)
    public float fill;

    //contient
    public Dictionary<string, float> elementsContained;

    //pH (?)
    public string pH;
    

    //************************************************************* FONCTIONS
}
