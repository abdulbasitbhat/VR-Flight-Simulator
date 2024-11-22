using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //SerializeField makes a GUI drag drop place. We create 4 camera places(By creating empty objects) and drop them into first 4 Povs
    [SerializeField] Transform[] povs;  
    //Control Spped at which camera follows plane. It makes camera act not fixed to plane even if its child of plane.
    [SerializeField] float speed;

    //To index all campera places.
    private int index = 1;

    private Vector3 target;


    //Camera/ Pilot Vison Dynamics
    private void Update() {
        //Setting Point Of View in index Variable
        // Numbers 1-4 represent different povs. We can select them from using numeric keys from 1 to 4
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { 
            index = 3; 
        }

        // Set our target to the relevant POV. povs store empty objects placed at specific locations. target will store the location
        target = povs[index].position;
    }

    private void FixedUpdate() {

        //Moving Camera Towards POV Empty Object which is indiacated ny index and is a child of plane object
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed); 

        //Set direction of camera towards forward direction of POV (empty obj)
        transform.forward = povs[index].forward;
    }
}

// Move camera to desired position/orientation. It is in FixedUpdate to avoid camera jitters. deltatime is frame rate.
// It is written to provide same feel for devices with diff frame rate
//Set position of camera at target place. Do this with speed value, so that camera dosent appear locked to plane.
//Speed is the delay between camera movement and plane movement
//Camera moves towards POVS which are empty objects holding some position. These POVS are children of plane.
//This line makes camera move towards these childrens but with a delay of speed value