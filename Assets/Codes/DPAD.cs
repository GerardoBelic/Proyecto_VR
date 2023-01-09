using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPAD
{
    public bool first_dpad_up = false;
    public bool pressed_dpad_up = false;
    //bool last_dpad_up = false;

    public bool first_dpad_down = false;
    public bool pressed_dpad_down = false;
    //bool last_dpad_down = false;

    public bool first_dpad_left = false;
    public bool pressed_dpad_left = false;
    //bool last_dpad_left = false;

    public bool first_dpad_right = false;
    public bool pressed_dpad_right = false;
    //bool last_dpad_right = false;

    public void update()
    {
        if (first_dpad_up && pressed_dpad_up)
            first_dpad_up = false;
        else if (!first_dpad_up && !pressed_dpad_up && Input.GetAxis("DPAD_v") > 0.9f)
        {
            first_dpad_up = true;
            pressed_dpad_up = true;
        }
        else if (Input.GetAxis("DPAD_v") < 0.9f)
            pressed_dpad_up = false;


        if (first_dpad_down && pressed_dpad_down)
            first_dpad_down = false;
        else if (!first_dpad_down && !pressed_dpad_down && Input.GetAxis("DPAD_v") < -0.9f)
        {
            first_dpad_down = true;
            pressed_dpad_down = true;
        }
        else if (Input.GetAxis("DPAD_v") > -0.9f)
            pressed_dpad_down = false;


        if (first_dpad_left && pressed_dpad_left)
            first_dpad_left = false;
        else if (!first_dpad_left && !pressed_dpad_left && Input.GetAxis("DPAD_h") < -0.9f)
        {
            first_dpad_left = true;
            pressed_dpad_left = true;
        }
        else if (Input.GetAxis("DPAD_h") > -0.9f)
            pressed_dpad_left = false;


        if (first_dpad_right && pressed_dpad_right)
            first_dpad_right = false;
        else if (!first_dpad_right && !pressed_dpad_right && Input.GetAxis("DPAD_h") > 0.9f)
        {
            first_dpad_right = true;
            pressed_dpad_right = true;
        }
        else if (Input.GetAxis("DPAD_h") < 0.9f)
            pressed_dpad_right = false;

    }
}
