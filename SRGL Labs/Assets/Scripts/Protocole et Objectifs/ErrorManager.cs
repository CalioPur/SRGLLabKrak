using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ErrorManager 
{
    public List<ErrorHolding> hold;
    public List<ErrorFilling> fill;

    //change name later -> error manager is not fitting
    public ErrorManager(List<ErrorHolding> h, List<ErrorFilling> f)
    {
        this.hold = h;
        this.fill = f;
    }

}
