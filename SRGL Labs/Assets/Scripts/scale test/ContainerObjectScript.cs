using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerObjectScript : MonoBehaviour
{
    //************************************************************* VARIABLES

    //event -> pour les objectifs
    public delegate void ObjectHadSomethingHappen(Objective objective);
    public static event ObjectHadSomethingHappen ObjectHadSomethingHappenEvent;

    //nom (may be removed if directly name of game object)
    public string containerName;

    //bouchon
    public bool capIsOn = false;

    //danger
    public int danger;

    //melange
    public bool wasMixed = false;

    //fill (% ?) (taux de remplissage) -> float ?
    public int fill;

    //contient
    public Dictionary<string, float> elementsContained;

    //pH (?)
    public string pH;

    //placeholder it occupies
    public GameObject hiddenPlaceholder;

    //poids en g
    public float weight;


    //************************************************************* FONCTIONS

    public void Start()
    {
        elementsContained = new Dictionary<string, float>();
    }

    public void FillObject(string putInName,float putInQuant, float goal) //remplir
    {
        if (!this.elementsContained.ContainsKey(putInName))
        {
            this.elementsContained.Add(putInName, putInQuant);
        }
        else
        {
            this.elementsContained[putInName] += putInQuant;
        }


        //poids
        if (weight > goal)
        {
            weight += (weight / 2);
        }
        else //ok
        {
            if(Mathf.Abs(goal - weight) < 2)
            {
                weight = goal;
            }
            else
            {
                float temp = goal - weight;
                weight += Random.Range((temp - temp*1/5), (temp + temp*1/5));
            }
            
        }

    }

    public void TakeFromObject(float takeFromQuantity, float goal)
    {
        if (elementsContained.Count==1)
        {
            /*elementsContained[KEY] -= takeFromQuantity;*/
        }


        //poids

        if (weight < goal) 
        {
            weight -= (weight / 2);
        }
        else //ok
        {
            if (Mathf.Abs(goal - weight) < 2)
            {
                weight = goal;
            }
            else
            {
                float temp = goal - weight;
                weight -= Random.Range((temp - temp * 1 / 5), (temp + temp * 1 / 5));
            }
        }

        if (weight < 0)
        {
            weight = 0;
        }
    }


}
