﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager2 : MonoBehaviour
{
    //input Managers tasks:
    //register input from controller
    //differentiate between players
    //translate input into action to call in form of functions from other script
    //keeps track of the state of the car


    public int playerNumber = 0;

    //input button names
    string camY;
    string camX;
    string resetCam;
    string vertical;
    string horizontal;
    string accelerate;
    string reverse;
    string jump;
    string shoot;
    // axis values
    float verticalVal;
    float horizontalVal;
    float accelerateVal;


    //states
    public bool IsGrounded;
    public bool IsDriving;
    bool HasSpikyWheels;
    bool HasJumped;
    bool IsUpsideDown;
    bool HasShot;
    bool HasHit;
    bool IsSwinging;
    bool BrakeNotReverse;
    bool HasTiltedHorizontal;
    bool HasTiltedVertical;

    //connected scripts
    DriveController dc;
    CameraController cc;

    void Start()
    {
        //set input to playernumber
        camY = "CamY"+playerNumber;
        camX = "CamX"+playerNumber;
        resetCam = "ResetCam"+playerNumber;
        vertical = "Vertical"+playerNumber;
        horizontal = "Horizontal"+playerNumber;
        accelerate = "Accelerate"+playerNumber;
        reverse = "Reverse"+playerNumber;
        jump = "Jump"+playerNumber;
        shoot = "Shoot"+playerNumber;

        dc = GetComponent<DriveController>();
        //cc = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {


//******Camera movement******
        float camYVal = Input.GetAxis(camY);
        if(camYVal !=0)
        {
            //Debug.Log("camY "+ camYVal);
        }

        float camXVal = Input.GetAxis(camX);
        if(camXVal !=0)
        {
            //Debug.Log("camX "+ camXVal);
        }

        if(camYVal !=0|| camXVal !=0)
        {
            cc.RotateCamera(camYVal, camXVal);
        }

        if(Input.GetButtonDown(resetCam))
        {
           //Debug.Log("Reset");
           //cc.ResetCamera();
           sc.rotateCarTowardsHook();
        }

// ******Car movement******
        verticalVal = Input.GetAxis(vertical);
        horizontalVal = Input.GetAxis(horizontal);
        if(IsGrounded)
        {
            //Steering
            if (IsDriving && HasTiltedHorizontal)
            {
                
                dc.Steer(horizontalVal);
                Debug.Log("steer");
            }
            else
            {
                dc.Steer(0);
            }
            
            
            //turning
            if (!IsDriving && HasTiltedHorizontal)
            {
                dc.Turn(horizontalVal);
            }
            else
            {
                dc.ResetTurningStiffness();
            }
        }
        else if(!IsGrounded &&  (HasTiltedHorizontal || HasTiltedVertical))
        {
            //Debug.Log("air steer");
            dc.AerialSteer(horizontalVal, verticalVal);
        }

        //Accelerate
        
        if(Input.GetButton(accelerate) && IsGrounded)
        {
           //Debug.Log("Accelerate");
           IsDriving = true;
           dc.Accelerate(1);
        }
        if(Input.GetButtonUp(accelerate))
        {
           //Debug.Log("Accelerate");
           dc.Accelerate(0);
        }

        //Reverse or Brake
        if(Input.GetButtonDown(reverse))
        {
            if(IsDriving)
            {
                BrakeNotReverse = true;
            }
            else
            {
                BrakeNotReverse = false;
            }
        }

        if (Input.GetButton(reverse) && IsGrounded)
        {
            if (BrakeNotReverse)
            {
                Debug.Log("Brake");
                dc.Brake(1);
            }
            else
            {
                IsDriving = true;
                dc.Reverse(1);
            }
        }

        if (Input.GetButtonUp(reverse))
        {
           dc.Brake(0);
           dc.Reverse(0);
        }

//*****Abilities*****
        if(Input.GetButtonDown(jump) && !HasJumped)
        {
           //Debug.Log("Jump");
           dc.Jump();
           HasJumped=true;
        }

        if(Input.GetButtonDown(shoot))
        {
           //Debug.Log("Shoot");
           hb.ShootHook();
        }

        //Check States
        IsGrounded = dc.CheckWheelsGrounded();
        HasJumped = !IsGrounded;
        HasHit = hb.GetHasHit();
        HasTiltedHorizontal = SetStickBool(horizontalVal);
        HasTiltedVertical = SetStickBool(verticalVal);


    }        
    public void SetIsDriving(bool boolean)
    {
        IsDriving = boolean;
    }

    private bool SetStickBool(float val)
    {
        if(val == 0)
        {
            return false;
        }
        return true;
    }
}