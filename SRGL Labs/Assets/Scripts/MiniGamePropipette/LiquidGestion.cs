using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidGestion : MonoBehaviour
{
    float timeElapsed;
    float lerpDuration = 100f;
    public Material mat;
    public float fillInput;
    float oldFillInput;
    public Transform viderLaPoire;
    public Transform remplirLaPropipette;
    public Transform viderLaPropipette;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform == remplirLaPropipette)
                {
                    fillInput += (0.1f*Time.deltaTime);
                }
                if (hit.transform == viderLaPropipette)
                {
                    fillInput -= (0.1f*Time.deltaTime);
                }
            }
        }
        

        mat.SetFloat("_fill", Mathf.Lerp(mat.GetFloat("_fill"), fillInput, 1));
        timeElapsed += Time.deltaTime;
        oldFillInput = fillInput;

    }
}
