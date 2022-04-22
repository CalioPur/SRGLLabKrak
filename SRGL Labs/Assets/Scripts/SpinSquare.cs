using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinSquare : MonoBehaviour
{
    public Camera cam;
    Vector3 localForward;
    public Quaternion lastRot;
    public float speed;
    public Rigidbody rigidbody;
    public GameObject fiole;

    void Awake()
    {
        lastRot = transform.rotation;
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            bool hasHit = Physics.Raycast(ray, out hit);


            if (hasHit && hit.collider.gameObject.tag == "Spin")
            {
                var dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);
                var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
            }
        }

        if (transform.rotation.z != lastRot.z)
        {
            print(transform.rotation);
            fiole.transform.rotation = Quaternion.Euler(20,transform.rotation.z*360,0);
            print("fiole = "+fiole.transform.rotation);


        }

        lastRot = transform.rotation;
    }
}
