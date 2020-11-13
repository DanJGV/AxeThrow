using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField]
    float moveSpeed = 3.0f;
    float rotSpeed = 2f;
    float throwPower = 15f;
    public float axeRotSpeed;
    public CharacterController controller;
    public GameObject axe;
    public Transform returnTarget;
    public Transform curvePoint;
    private Vector3 oldPos;
    public bool isReturning = false;
    float time = 0f;
    [Header("References")]
    [SerializeField]
    Transform mainCamera;
    [SerializeField]
    BoxCollider swordCollider;
   

    [SerializeField]
    CinemachineVirtualCamera aimCam;

    bool aiming = false; 

    Rigidbody rb;
    Animator anim;

    bool startedCombo = false;
    float timeSinceButtonPressed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0f, v);

        transform.Rotate(Vector3.up, h * rotSpeed);

        if (v != 0)
        {
            controller.Move(transform.forward * moveSpeed * v * Time.deltaTime);
        }


        /*var camForward = mainCamera.forward;
        var camRight = mainCamera.right;

        camForward.y = 0;
        camForward.Normalize();
        camRight.y = 0;
        camRight.Normalize();*/

        //var moveDirection = (camForward * v * moveSpeed) + (camRight * h * moveSpeed);

        /*transform.LookAt(transform.position + moveDirection);
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);*/

        anim.SetFloat("moveSpeed", Mathf.Abs(direction.magnitude));

        /*if (direction.magnitude > 0)
        {
            Quaternion newDirection = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, Time.deltaTime * rotSpeed);
        }*/

        if(Input.GetButtonDown("Jump") && !startedCombo && axe.GetComponent<AxeScript>().thrown == false)
        {
            anim.SetTrigger("swordCombo");
            startedCombo = true;
            
        }

        if(Input.GetButtonDown("Jump") && startedCombo)
        {
            timeSinceButtonPressed = 0;
        }

       

        if (Input.GetButtonDown("Fire2"))
        {
            aiming = true;
            aimCam.GetComponent<CinemachineVirtualCamera>().Priority = 15;
           
        }

        if (Input.GetButtonDown("Fire1") && aiming == true && axe.GetComponent<AxeScript>().thrown == false)
        {
            anim.SetTrigger("throwSword");
            
            isReturning = false;
        }

        if (axe.GetComponent<AxeScript>().thrown == true && Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("catchSword");
            //returnAxe();
        }

        if (isReturning)
        {
            if (time < 1.0f)
            {
                axe.GetComponent<Rigidbody>().position = getCurvePoint(time, oldPos, curvePoint.position, returnTarget.position);
                time += Time.deltaTime;
            }
            else
            {
                ResetAxe();
                
            }
        }

        if (Input.GetButtonUp("Fire2"))
        {
            aiming = false;
            aimCam.GetComponent<CinemachineVirtualCamera>().Priority = 10;
        }

            timeSinceButtonPressed += Time.deltaTime;
    }

    

    public void PotentialComboEnd()
    {
        TurnOffSwordCollider();

        if (timeSinceButtonPressed < 0.5f)
            return;

        anim.SetTrigger("stopCombo");
        startedCombo = false;
        timeSinceButtonPressed = 0;
        
    }

    public void EndOfCombo()
    {
        startedCombo = false;
        timeSinceButtonPressed = 0;
        TurnOffSwordCollider();
    }

    public void TurnOnSwordCollider()
    {
        swordCollider.enabled = true;
    }

    public void TurnOffSwordCollider()
    {
        swordCollider.enabled = false;
    }

    public void AxeThrow()
    {
        isReturning = false;
        axe.GetComponent<Rigidbody>().isKinematic = false;
        axe.GetComponent<Rigidbody>().transform.parent = null;

        axe.GetComponent<Rigidbody>().AddForce(transform.forward * throwPower, ForceMode.Impulse);
        axe.GetComponent<Rigidbody>().AddTorque(axe.transform.TransformDirection(Vector3.right) * axeRotSpeed, ForceMode.Impulse);
        axe.GetComponent<AxeScript>().thrown = true;
    }

    public void returnAxe()
    {
        time = 0f;
        oldPos = axe.GetComponent<Rigidbody>().position;
        isReturning = true;
        axe.GetComponent<Rigidbody>().velocity = Vector3.zero;
        axe.GetComponent<Rigidbody>().isKinematic = true;
        //axe.transform.position += (returnTarget.position - axe.GetComponent<Rigidbody>().position) * Time.deltaTime;
        //axe.transform.position = Vector3.MoveTowards(axe.GetComponent<Rigidbody>().position, new Vector3(returnTarget.position.x, returnTarget.position.y, returnTarget.position.z), 40);
    }   

    void ResetAxe()
    {
        isReturning = false;
        axe.transform.parent = returnTarget;
        axe.GetComponent<Rigidbody>().position = returnTarget.position;
        axe.GetComponent<Rigidbody>().rotation = returnTarget.rotation;
        axe.GetComponent<AxeScript>().thrown = false;
    }
    Vector3 getCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }

}
