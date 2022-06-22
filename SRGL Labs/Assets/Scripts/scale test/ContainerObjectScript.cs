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
    public float shaderFill;
    public Material material;

    //contient
    public Dictionary<string, float> elementsContained;

    //pH (?)
    public string pH;

    //placeholder it occupies
    public GameObject hiddenPlaceholder;

    //poids en g
    public float weight;

    //weight goal -> -1 si doit etre ignoré
    public float weightGoal;

    //peut-on prelever ?
    //add bool ?
    //si a deja prelevé -> string du premier prelevement pour erreurs prelevement


    //************************************************************* FONCTIONS

    public void Start()
    {
        elementsContained = new Dictionary<string, float>();
        if (material != null)
        {
            material.SetFloat("_fill", shaderFill);
        }
    }

    public void FillObject(string putInName,float putInQuant,float fillQuantity) //remplir -> fill quantity pour shader seulement
    {
        //ajout element + quantité dans dico
        if (!this.elementsContained.ContainsKey(putInName))
        {
            this.elementsContained.Add(putInName, putInQuant);
        }
        else
        {
            this.elementsContained[putInName] += putInQuant;
        }

        //gestion shader
        if (material != null)
        {
            shaderFill += fillQuantity;
            LeanTween.value(gameObject, UpdateShaderFill, material.GetFloat("_fill"), shaderFill, 0.5f).setEaseOutCubic();
        
        }


        //poids
        if (weightGoal != -1)
        {
            if (weight >= weightGoal)
            {
                weight += (weight / 2);
            }
            else //ok
            {
                if (Mathf.Abs(weightGoal - weight) < 2)
                {
                    weight = weightGoal;
                }
                else
                {
                    float temp = weightGoal - weight;
                    weight += Random.Range((temp - temp * 1 / 5), (temp + temp * 1 / 5));
                }

            }
        }
        

    }

    public void TakeFromObject(float takeFromQuantity, float fillQuantity)
    {
        /*if (elementsContained.Count==1)
        {
            elementsContained[KEY] -= takeFromQuantity;
        }*/

        //gestion shader
        if (material != null)
        {
            shaderFill -= fillQuantity;
            if (shaderFill < 0)
            {
                shaderFill = 0;
                LeanTween.value(gameObject, UpdateShaderFill, material.GetFloat("_fill"), shaderFill, 0.5f).setEaseOutCubic();
            }
        }

        //poids
        if (weightGoal != -1)
        {
            if (weight <= weightGoal)
            {
                weight -= (weight / 2);
            }
            else //ok
            {
                if (Mathf.Abs(weightGoal - weight) < 2)
                {
                    weight = weightGoal;
                }
                else
                {
                    float temp = weightGoal - weight;
                    weight += Random.Range((temp - temp * 1 / 5), (temp + temp * 1 / 5));
                }
            }

            if (weight < 0)
            {
                weight = 0;
            }
        }
        
    }

    public void UpdateShaderFill(float newValue)
    {
        material.SetFloat("_fill", newValue);
    }

    public void EmptyObject()
    {

    }

}
