using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class ErrorTaking : Error
{
    //not used yet
    string message;

    public string container;
    public string tools;
    public string content;
    public int maxFill;
    public int place;

    public override string ErrorMessage()
    {
        return this.message;
    }

    public override bool EvaluateError(Error error)
    {
        throw new System.NotImplementedException();
    }
}
