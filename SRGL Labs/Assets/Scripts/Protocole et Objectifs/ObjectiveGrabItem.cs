using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveGrabItem : Objective
{
    //Tag de l'objet que l'on veut
    //On pourra modifier plus tard pour un systeme plus complexe
    string tagOrNameOfObject;

    //Constructeur
    public ObjectiveGrabItem(string tagOrName)
    {
        this.tagOrNameOfObject = tagOrName;
    }

    public override bool Evaluate(Objective obj)
    {
        if (obj.GetType() == typeof(ObjectiveGrabItem))
        {
            ObjectiveGrabItem temp = (ObjectiveGrabItem)obj;
            return temp.tagOrNameOfObject.Equals(this.tagOrNameOfObject);
        }
        else
        {
            return false;
        }
    }


}