using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputManager : MonoBehaviour
{
    public bool isAgent = false;

    public float throttleInput, steerInput, yawInput, pitchInput, rollInput;
    public bool isBoost, isDrift, isAirRoll;
    public bool isJump, isJumpUp, isJumpDown;
    public int InputID;

    public InControl.InputDevice myInput;

    void Update() {
        var lDevices = InputManager.Devices;
        if (lDevices.Count > InputID)
        {
            myInput = InputManager.Devices[InputID];
        }

        if (!isAgent)
        {

            throttleInput = GetThrottle();
            steerInput = GetSteerInput();

            if (myInput != null)
            {

                yawInput = myInput.LeftStickX.Value;
                pitchInput = -myInput.LeftStickY.Value;
                rollInput = GetRollInputDevice();

                isJump = Input.GetMouseButton(1) || Input.GetButton("A");
                isJumpUp = Input.GetMouseButtonUp(1) || Input.GetButtonUp("A");
                isJumpDown = Input.GetMouseButtonDown(1) || Input.GetButtonDown("A");

                isBoost = myInput.Action1.IsPressed;
                isDrift =  myInput.Action3.IsPressed;
                isAirRoll = myInput.Action3.IsPressed;
            }
        }
    }

    private static float GetRollInputDevice()
    {
        var inputRoll = 0;

        return inputRoll;
    }

    private static float GetRollInput()
    {
        var inputRoll = 0;
        if (Input.GetKey(KeyCode.E))
            inputRoll = -1;
        else if (Input.GetKey(KeyCode.Q))
            inputRoll = 1;

        return inputRoll;
    }

    static float GetThrottle()
    {
        float throttle = 0;

        if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("L2") > 0)
            throttle = Mathf.Max(Input.GetAxis("Vertical"), Input.GetAxis("L2")) * -1;
        else if (Input.GetAxis("Vertical") < 0 || Input.GetAxis("R2") < 0)
            throttle = Mathf.Min(Input.GetAxis("Vertical"), Input.GetAxis("R2")) * -1;

        return throttle;
    }

    static float GetSteerInput()
    {
        //return Mathf.MoveTowards(steerInput, Input.GetAxis("Horizontal"), Time.fixedDeltaTime);
        return Input.GetAxis("Horizontal");
    }

    public string axisName = "Horizontal";
    public AnimationCurve sensitivityCurve;
    private float _vel = 0;
    float _currentHorizontalInput = 0;
    public float GetValue()
    {
        var target = Mathf.MoveTowards(_currentHorizontalInput, Input.GetAxis(axisName), Time.fixedDeltaTime / 25);
        _currentHorizontalInput = sensitivityCurve.Evaluate(Mathf.Abs(target));
        var ret = _currentHorizontalInput * Mathf.Sign(Input.GetAxis(axisName));
        
        // Debug.Log("Input: " + Input.GetAxis("Horizontal"));
        // Debug.Log("Out: " + ret);
        // Debug.Log("");

        return ret;

        //var ret = sensitivityCurve.Evaluate(Mathf.Abs(Input.GetAxis(axisName)));
        //return ret * Mathf.Sign(ret);

        //var target = sensitivityCurve.Evaluate(Mathf.Abs(Input.GetAxis(axisName)));
        //currentInput = Mathf.SmoothDamp(currentInput, target, ref vel, 1f, Mathf.Infinity, Time.fixedDeltaTime);
        //return currentInput * Mathf.Sign(Input.GetAxis(axisName));
    }


    private void OnGUI()
    {
        //GUILayout.HorizontalSlider(pitchInput, -1, 1, GUILayout.Width(200));
        //GUILayout.HorizontalSlider(yawInput, -1, 1, GUILayout.Width(200));
        //GUILayout.HorizontalSlider(rollInput, -1, 1, GUILayout.Width(200));
    }
}
