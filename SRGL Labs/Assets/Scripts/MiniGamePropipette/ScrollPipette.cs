using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ScrollPipette : MonoBehaviour
{
    public SphereColliderScript sphereColliderScript;
    public CylinderColliderScript cylinderColliderScript;
    public TextMeshPro mText;
    public float step = 0.1f;
    float LastTimeSinceScroll = 0;
    public bool ICanScroll = true;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,6f,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (((Mouse.current.scroll.ReadValue().y >0 && transform.position.y<7) || (Mouse.current.scroll.ReadValue().y < 0 && transform.position.y > 5)) && ICanScroll)
        {
            transform.position+= new Vector3(0,step*(Mouse.current.scroll.ReadValue().y/Mathf.Abs(Mouse.current.scroll.ReadValue().y)) , 0);
            if (cylinderColliderScript.isPlaced)
            {
                if (Mouse.current.scroll.ReadValue().y < 0)
                {
                    step /= 1.5f;
                }
                else
                {
                    step *=1.5f;
                }
                LastTimeSinceScroll = Time.realtimeSinceStartup;
            }
            
        }
        

        if (cylinderColliderScript.isPlaced)
        {
            if (!sphereColliderScript.isBroken)
            {
                
                if (transform.position.y < 5.4)
                {
                    mText.SetText("La propipette est en place");
                    if (Time.realtimeSinceStartup - LastTimeSinceScroll > 1f)
                    {
                        ICanScroll = false;
                        mText.SetText("");
                    }
                }

            }
            else
            {
                mText.SetText("La propipette est cassé");
            }
            
        }
        else
        {

            mText.SetText("La propipette n'est pas en place");
        }
        if (Time.realtimeSinceStartup - LastTimeSinceScroll > 0.5f)
        {
            step = 0.05f;
        }
    }
}
