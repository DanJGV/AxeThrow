using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{

    public bool thrown = false;
    public float rotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       /* if(thrown)
        {
            transform.localEulerAngles += transform.forward * rotSpeed * Time.deltaTime;
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        
       // GetComponent<Rigidbody>().isKinematic = true;
    }
}
