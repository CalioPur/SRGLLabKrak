using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveGrabItem : Objective
{
    //Tag de l'objet que l'on veut
    //On pourra modifier plus tard pour un systeme plus complexe
    string tagOfObject;

    //Constructeur
    public ObjectiveGrabItem(string tag) 
    {
        this.tagOfObject = tag;
    }

    public override bool Evaluate(Objective obj)
    {
        if (obj.GetType() == typeof(ObjectiveGrabItem))
        {
            ObjectiveGrabItem temp = (ObjectiveGrabItem)obj;
            return temp.tagOfObject.Equals(this.tagOfObject);
        }
        else
        {
            return false;
        }
    }

 
}
