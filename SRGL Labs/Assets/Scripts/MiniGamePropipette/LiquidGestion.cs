using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidGestion : MonoBehaviour
{
    public Material mat;
    public float fillInput;
    public Transform viderLaPoire;
    public Transform remplirLaPropipette;
    public Transform viderLaPropipette;
    public ScrollPipette scrollPipette;

    bool poireVidée=false;
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
                if (!scrollPipette.ICanScroll && poireVidée)
                {
                    if (hit.transform == remplirLaPropipette)
                    {
                        fillInput += (0.1f * Time.deltaTime);
                    }
                    if (hit.transform == viderLaPropipette)
                    {
                        fillInput -= (0.1f * Time.deltaTime);
                    }
                   
                }
                if (hit.transform == viderLaPoire)
                {
                    poireVidée = true;
                }
            }
        }
        
        

        mat.SetFloat("_fill", Mathf.Lerp(mat.GetFloat("_fill"), fillInput, 1));
        if (fillInput < 0)
        {
            fillInput = 0;
        }

    }
}
