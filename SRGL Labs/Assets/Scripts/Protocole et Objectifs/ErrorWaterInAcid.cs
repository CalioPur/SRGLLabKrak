using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorWaterInAcid : Error
{
    Dictionary<string, int> elementsPouredIn;
    Dictionary<string, int> elementsAlreadyPresent;
    public ErrorWaterInAcid(Dictionary<string,int> pouredIn, Dictionary<string,int> alreadyIn)
    {
        this.elementsAlreadyPresent = alreadyIn;
        this.elementsPouredIn = pouredIn;
    }


    //NE PREND EN COMPTE QUE SI ON VERSE SEULEMENT DE L'EAU DANS L'ACIDE (sans eau avec)
    //(eau avec ou sans autres �l�ments ?)
    public override bool EvaluateError(Error error)
    {
        if (error.GetType() == typeof(ErrorWaterInAcid))
        {
            ErrorWaterInAcid temp = (ErrorWaterInAcid)error;
            if (temp.elementsPouredIn.ContainsKey("eau distill�e") && temp.elementsAlreadyPresent.ContainsKey("acide") && !temp.elementsAlreadyPresent.ContainsKey("eau distill�e"))
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


}
