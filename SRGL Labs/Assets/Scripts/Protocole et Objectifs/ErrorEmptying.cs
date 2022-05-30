using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class ErrorEmptying : Error
{
    //not used yet
    string message;

    public int danger;
    public string trashCan;
    public override string ErrorMessage()
    {
        return this.message;
    }

    public override bool EvaluateError(Error error)
    {
        throw new System.NotImplementedException();
    }

}
