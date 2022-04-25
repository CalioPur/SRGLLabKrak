using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveContains : Objective
{
    //liste de couples (élément,quantité) que l'on veut / que l'on a
    List<(string,int)> listOfElementsAndQuantityRequired;

    //
    public override bool Evaluate(Objective obj)
    {
        if(obj.GetType() == typeof(ObjectiveContains))
        {

            bool flag = true;

            for(int k = 0; k< this.listOfElementsAndQuantityRequired.Count; k++)
            {
                //if()
            }

            return flag;
            
        }
        else
        {
            return false;
        }
        
    }


}
