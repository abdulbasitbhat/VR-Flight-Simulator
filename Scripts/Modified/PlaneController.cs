using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneController : MonoBehaviour
{
    public float throttleIncrement = 0.1f;
    public float maxThrust = 200f;
    public float responsiveness = 10f;
    public float lift = 135f;

    private float throttle;
    private float roll;
    private float pitch; 
    private float yaw;


    private int lg_up_alt = 30f;  //Set val here
    private int lg_down_alt = 30f; //Set value here
    
    //Switch values
    private int autopilot = 0;
    private int landingGears = 1;
    private int t_seq = 1;  //Default take off sequence
    private int l_seq = 0;

    //Rigid Body GUI/Code mapping
    Rigidbody rb;
    [SerializeField] TextMeshProUGUI hud;
    [SerializeField] Transform propella;


    private void Awake()  //Awake initalizes variables before the simulator is started
    {
        rb = GetComponent<Rigidbody>();
    }

    private float responseModifier {
        get{
            return (rb.mass / 10f) * responsiveness;
        }
    }
    
    private void HandleInputs() {
        if (Input.GetKey(KeyCode.Alpha2))
        {
            landing_sequence();
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            autopilot_on_off();
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            takeoff_sequence();
        }
        
        roll = Input.GetAxis("Roll");  //a and d
        pitch = Input.GetAxis("Pitch");  //w and s
        yaw = Input.GetAxis("Yaw");  //q and e

        if (Input.GetKey(KeyCode.Space)) //Space to Throttle Up
        {
            throttle += throttleIncrement;
        }
        else if (Input.GetKey(KeyCode.LeftControl))  //Left Control to Throttle Down 
        { 
            throttle -= throttleIncrement; 
        }
        throttle = Mathf.Clamp(throttle, 0f, 100f);  //Limit throttle
    }
    
    
    private void Update()  //Updated once every frame
    {
        HandleInputs();
        UpdateHUD();
        propella.Rotate(Vector3.right * throttle);
    }
    

    private void FixedUpdate() {
        if (t_seq == 0 && l_seq == 0)
        {
            in_air_control();
        }
        if (autopilot == 0)  //Manual Control
        {   
            rb.AddForce(transform.forward * maxThrust * throttle);
            rb.AddTorque(transform.up * yaw * responseModifier);
            rb.AddTorque(transform.right * pitch * responseModifier);
            rb.AddTorque(-transform.forward * roll * responseModifier);

            //Air breaks
            rb.AddForce(-transform.forward * maxThrust * Alpha3);
        }
        else  //Autopilot, to initiate press alpha 0. To disengage again press alpha 0
        {   
            rb.AddForce(transform.forward * 100f); //set value here
        }
        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift); //Lift of plane due to Bernaullis Principle
    }


    private void UpdateHUD() {
        hud.text = "Throttle: " + throttle.ToString("F0") + "%\n";
        hud.text += "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F0") + "km/h\n";
        hud.text += "Altitude: " + transform.position.y.ToString("F0") + "m";
        if (autopilot)
        {
            hud.text += "Autopilot: " + "On" + "\n";
        }
        else
        {
            hud.text += "Autopilot: " + "Off" + "\n";
        }
        if (landingGears)
        {
            hud.text += "Landing Gears " + "On" + "\n";
        }
        else
        {
            hud.text += "Landing Gears " + "Off" + "\n";
        }
        if (t_seq)
        {
            hud.text += "Takeoff Sequence " + "On" + "\n";
        }
        else
        {
            hud.text += "Takeoff Sequence " + "Off" + "\n";
        }
        if (l_seq)
        {
            hud.text += "Landing Sequence " + "On" + "\n";
        }
        else
        {
            hud.text += "Landing Sequence " + "Off" + "\n";
        }

    }

    

    private void takeoff_sequence()
    {
        if (at_seq == 0)
        {
            t_seq = 1;
            landingGears = 1;
            responsiveness = 5f;
            autopilot = 0;

            if (transform.position.y > lg_up_alt)
            {
                landingGears = 0;
            }
        }
        else
        {
            t_seq = 0;
        }

    }

    private void in_air_control()
    {
        responsiveness = 10f;
        t_seq = 0;
        l_seq = 0;
    }

    private void autopilot_on_off()
    {
        if (autopilot == 0)
        {
            autopilot = 1;
        }
        else
        {
            autopilot = 0;
        }
    }

    private void landing_sequence()
    {
        if (l_seq == 0)
        {
            l_seq = 1;
            
            if (transform.position.y < lg_down_alt)
            {
                landingGears = 1;
            }
            landingGears = 1;
            responsiveness = 7f; //Plane controls becomes less responsive due to ground effect
            maxThrust = 100f; //Thrust power decreases
            autopilot = 0;
            
            if (Input.GetKey(KeyCode.Alpha3))  //Ground breaks
            {
                rb.AddForce(-transform.forward * maxThrust * Alpha3);

            }
        }
        else
        {
            l_seq = 0;
        }

    }    
}

/*
Notes
1. fixedUpdate is updated once in time set by editor. It is needed for doing actions in sync with Physice Engine
2. In Hud //rb.velocity.magnitude is velocity of unity object. It must be multiplied in apt way to achieve real world velocity feel

*/
