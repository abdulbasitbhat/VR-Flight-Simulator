using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControllerTest : MonoBehaviour
{   //Declare Variables Here

    // Start is called before the first frame update
    void Start()
    {

    }


    void thrusters(int direction)
    {
        if (direction == 1)
        {
            //Thrusters Up. Will speed Up Plane
            transform.Translate(Vector3.forward * Time.deltaTime * 50);
        }
        if (direction == -1)
        {
            //Thrusters Down. Will Slow Down Plane
            transform.Translate(-Vector3.forward * Time.deltaTime * 5);
        }
    }


    void rudders(int direction)
    {
        if (direction == 1)
        {
            //Go Left   Rudder must go left
            transform.Rotate(Vector3.down * Time.deltaTime * 10);
        }
        if (direction == -1)
        {
            //Go Right   Rudder must go right
            transform.Rotate(Vector3.up * Time.deltaTime * 10);
        }

    }

    void flaps_Elivators(int direction)
    {
        //Flaps are turned on when small nose adjustment is needed. But when large adjustment, Elivators are used
        if (direction == 1)
        {
            //Flaps Control UP     As Flaps go up, plane goes up
            transform.Rotate(Vector3.left * Time.deltaTime * 10);
        }
        if (direction == -1)
        {
            //Flaps Control Down    As Flaps go do, plane goes down
            //Less force for negative G Lock
            transform.Rotate(Vector3.right * Time.deltaTime * 7);
        }
    }

    void ailuron(int direction)
    {
        if (direction == 1)
        {
            //Ailuron to dip left  Left Up,Right Down 
            transform.Rotate(Vector3.forward * Time.deltaTime * 10);
        }
        if (direction == -1)
        {
            //Ailuron to dip right  Left Down,Right UP
            transform.Rotate(-Vector3.forward * Time.deltaTime * 10);
        }
    }

    int thrusterflag = 0;
    int takeoff = 0;
    int landing = 0;
    void takeOffSequence()
    {
        //Thruster Control
        if (thrusterflag == 1)
        {
            thrusters(1);
        }
        //Thrister Control     ---  Control Speed
        if (Input.GetKey("u"))
        {
            thrusterflag = 1;
            thrusters(1);
        }
        if (Input.GetKey("j"))
        {
            thrusterflag = 0;
            thrusters(-1);
        }

        //Flaps_elivators Control
        //Flaps,Elivators Control     ---  Control Nose angle and plane angle
        if (Input.GetKey("w"))
        {
            //Nose Up
            flaps_Elivators(1);
        }

        if (Input.GetKey("s"))
        {
            //Nose Down
            flaps_Elivators(-1);
        }
    }

void landingSequence()
    {
        if (thrusterflag == 1)
        {
            thrusters(1);
        }
        //Thrister Control     ---  Control Speed
        if (Input.GetKey("u"))
        {
            thrusterflag = 1;
            thrusters(1);
        }
        if (Input.GetKey("j"))
        {
            thrusterflag = 0;
            thrusters(-1);
        }

        //Flaps_elivators Control
        //Flaps,Elivators Control     ---  Control Nose angle and plane angle
        if (Input.GetKey("w"))
        {
            //Nose Up
            flaps_Elivators(1);
        }

        if (Input.GetKey("s"))
        {
            //Nose Down
            flaps_Elivators(-1);
        }
    }

    void controls()
    {   
        if (takeoff == 1)
        {
            takeOffSequence();
        }
        if (landing == 1)
        {
            landingSequence();
        }

        if (thrusterflag == 1)
        {
            thrusters(1);
        }

        if (Input.GetKey("t"))
        {
            if (takeoff == 1)
            {
                takeoff = 0;
            }
            else
            {
                takeoff = 1;
            }
        }

        if(Input.GetKey("l"))
        {
            if (landing == 1)
            {
                landing = 0;
            }
            else
            {
                landing = 1;
            }
        }


        //Thruster Control     ---  Control Speed
        if (Input.GetKey("u"))
        {
            thrusterflag = 1;
            thrusters(1);
        }
        if (Input.GetKey("j"))
        {
            thrusterflag = 0;
            thrusters(-1);
        }



        //Rudder Control     ---  Control Direction
        if (Input.GetKey("a"))
        {
            //Left
            rudders(1);
        }

        if (Input.GetKey("d"))
        {
            //Right
            rudders(-1);
        }



        //Flaps,Elivators Control     ---  Control Nose angle and plane angle
        if (Input.GetKey("w"))
        {
            //Nose Up
            flaps_Elivators(1);
        }

        if (Input.GetKey("s"))
        {
            //Nose Down
            flaps_Elivators(-1);
        }



        //Ailuron Control     ---  Control Spins
        if (Input.GetKey("q") && takeoff == 0 && landing == 0)
        {
            ailuron(1);
        }

        if (Input.GetKey("e") && takeoff == 0 && landing == 0)
        {
            ailuron(-1);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        controls();
    }
}
