using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneController : MonoBehaviour
{   


    //Variables

    //How much the throttle ramps up or down.
    public float throttleIncrement = 0.1f;
    //Maximum engine thrust when at 100% throttle.
    public float maxThrust = 200f;
    //How responsive the plane is when rolling, pitching, and yawing.
    public float responsiveness = 10f;
    //How much lift force this plane generates as it gains speed.
    public float lift = 135f;

    //Variables
    private float throttle;
    private float roll;
    private float pitch; 
    private float yaw;

    

    //Dynamic Response Modifier
    //To control the smoothness and responsiveness of movements except throttle
    private float responseModifier {
        get{
            return (rb.mass / 10f) * responsiveness;
        }
    }
    
    //Add respective Elements from GUI to be used in Code
    Rigidbody rb;
    [SerializeField] TextMeshProUGUI hud;
    [SerializeField] Transform propella;
    
    //Awake initalizes variables before the simulator is started
    private void Awake() {
        //rb stores the rigit body the script is applied on
        rb = GetComponent<Rigidbody>();
    }
    
    private void HandleInputs() {
        //Input mapping done in input manager
        //Changed Horizontal Axis in Input manager i.e a and d
        roll = Input.GetAxis("Roll");
        //Changed Vetical Axis in Input manager i.e w and s
        pitch = Input.GetAxis("Pitch");
        //Added 3rd Axis in Input manager i.e q and e
        yaw = Input.GetAxis("Yaw");

        //Throttle Control   Space to Throttle Up, Left Control to Throttle Down
        if (Input.GetKey(KeyCode.Space))
        {
            throttle += throttleIncrement;
        }
        else if (Input.GetKey(KeyCode.LeftControl)) {
            throttle -= throttleIncrement; 
        }
        //Limit throttle values between 0 to 100
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }
    
    //Update is updated once every frame
    private void Update() {
        //Call controller in each frame
        HandleInputs();
        UpdateHUD();
        //Plane Propellar Rotatation
        propella.Rotate(Vector3.right * throttle);
    }
    //fixedUpdate is updated once in time set by editor. It is needed for doing actions in sync with Physice Engine

    private void FixedUpdate() {
        //Controlling Rigid body dynamics using unity Physics Engine
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(-transform.forward * roll * responseModifier);

        //Takeoff Lift
        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
    }


    //Hood Parameters
    private void UpdateHUD() {
        //Throttle press Amount
        hud.text = "Throttle: " + throttle.ToString("F0") + "%\n";
        //rb.velocity.magnitude is velocity of unity object. It must be multiplied in apt way to achieve real world velocity feel
        hud.text += "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F0") + "km/h\n";
        //Altitude is simply the y axis height of plane
        hud.text += "Altitude: " + transform.position.y.ToString("F0") + "m";
    }
}
