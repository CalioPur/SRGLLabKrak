using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//On peut enlever monobehaviour plus tard
public abstract class Objective : MonoBehaviour
{
    public abstract bool Evaluate(Objective obj);

}
