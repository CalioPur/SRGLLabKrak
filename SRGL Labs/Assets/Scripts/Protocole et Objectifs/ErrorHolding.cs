using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class ErrorHolding : Error
{
    //not used yet
    string message;

    //danger
    public int danger;
    //gant
    public bool? gloves;
    //bouchon 
    public bool? cap;

    //bool? est un nullable bool

    public ErrorHolding(int d, bool g, bool l)
    {
        this.danger = d;
        this.gloves = g;
        this.cap = l;
    }

    //message pour popup
    public override string ErrorMessage()
    {
        return this.message;
    }


    public override bool EvaluateError(Error error)
    {
        if (error.GetType() == typeof(ErrorHolding))
        {
            ErrorHolding temp = (ErrorHolding)error;

            return false; // remove later
        }
        else
        {
            return false;
        }
            
    }
}
