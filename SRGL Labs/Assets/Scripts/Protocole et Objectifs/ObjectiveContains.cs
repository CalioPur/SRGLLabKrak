using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveContains : Objective
{
    //liste de couples (élément,quantité) que l'on veut / que l'on a
    List<(string,int)> listOfElementsAndQuantityRequired;

    //Constructeur
    public ObjectiveContains(List<(string,int)> list)
    {
        this.listOfElementsAndQuantityRequired = list;
    }

    //Verification du type de l'objectif reçu puis verification des elements de la liste de l'objectif reçu
    public override bool Evaluate(Objective obj)
    {
        if(obj.GetType() == typeof(ObjectiveContains))
        {
            ObjectiveContains temp = (ObjectiveContains)obj;
            bool flag = true;

            for (int k = 0; k < this.listOfElementsAndQuantityRequired.Count; k++)
            {
                if (!temp.listOfElementsAndQuantityRequired.Contains(this.listOfElementsAndQuantityRequired[k]))
                {
                    flag = false;
                    break;
                }
            }

            return flag;
            
            
        }
        else
        {
            return false;
        }
        
    }


}
