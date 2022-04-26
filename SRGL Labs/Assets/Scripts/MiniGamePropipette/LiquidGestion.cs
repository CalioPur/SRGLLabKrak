using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidGestion : MonoBehaviour
{
    float timeElapsed;
    float lerpDuration = 100;
    public Material mat;
    public float fillInput;
    float oldFillInput;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (oldFillInput != fillInput)
        {
            timeElapsed = 0;
        }
        if (mat.GetFloat("_fill") != fillInput)
        {
            if (timeElapsed < lerpDuration)
            {
                mat.SetFloat("_fill", Mathf.Lerp(mat.GetFloat("_fill"), fillInput, timeElapsed/lerpDuration));
                timeElapsed += Time.deltaTime;
            }
        }
        oldFillInput = fillInput;
    }
}
