using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class ShipGroundControl : MonoBehaviour
{
    private const float NaiveRotationForce = 5;
    private const float NaiveRotationDampeningForce = -10;


    private Rigidbody _rb;
    private ShipController _controller;
    private InputManager _inputManager;
    private ShipCollision _shipCollision;

    public bool disableGroundStabilization;
    public bool disableWallStabilization;
    public bool disableDrift;
    [Header("Steering")] [Range(0, 360)] public float turnRadiusCoefficient = 360;
    public float currentSteerAngle;
    public float driftFactor = 0.5f;

    private float _yaw;
    private float _roll;
    private float _pitch;

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _controller = GetComponent<ShipController>();
        _inputManager = GetComponentInParent<InputManager>();
        _shipCollision = GetComponentInParent<ShipCollision>();
    }


    private void FixedUpdate()
    {
        ApplyStabilizationFloor();
        ApplyStabilizationWall();
        //float isForwarding = Input.GetAxis("R2") == -1 ? 1f : 0f;
        //isForwarding += Input.GetAxis("L2") == 1 ? -1f : 0f;

        //Debug.LogError(_inputManager.throttleInput);
        var forwardAcceleration = CalcForwardForce(_inputManager.throttleInput);
        if (Mathf.Abs(_inputManager.throttleInput) >= 0.0001f) 
        {
            ApplyForwardForce(forwardAcceleration);
        }

        _yaw = _inputManager.yawInput;
        _pitch = _inputManager.pitchInput;
        _roll = _inputManager.rollInput;

        Quaternion target = Quaternion.Euler(_pitch, _yaw, _roll);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 2.0f );

    }

    private void ApplyStabilizationWall()
    {
        if (disableWallStabilization)
        {
            return;
        }

        if (Mathf.Abs(Vector3.Dot(Vector3.up, transform.up)) > 0.95f ||
            _controller.shipState == ShipController.ShipStates.Air || _controller.numCornersSurface < 2)
        {
            return;
        }

        _rb.AddForce(-transform.up * 5f, ForceMode.Acceleration);
    }

    private void ApplyStabilizationFloor()
    {
        if (disableGroundStabilization)
        {
            return;
        }

        if (Mathf.Abs(_inputManager.throttleInput) <= 0.0001f)
        {
            return;
        }

        if (_controller.shipState == ShipController.ShipStates.Air
            || _controller.numCornersSurface >= 3)
        {
            return;
        }

        if (_shipCollision == null || _shipCollision.surfaceNormal == null)
        {
            return;
        }

        var torqueDirection = -Mathf.Sign(Vector3.SignedAngle(_shipCollision.surfaceNormal, _rb.transform.up,
            _controller.cogLow.transform.forward));
        var torqueForceMode = ForceMode.Acceleration;
        var factor = 50.0f;
        if (_controller.shipState == ShipController.ShipStates.BodyGroundDead)
        {
            torqueForceMode = ForceMode.VelocityChange;
            factor = 0.4f;
        }

        _rb.AddTorque(_controller.cogLow.transform.forward * factor * torqueDirection, torqueForceMode);
        if (_controller.shipState == ShipController.ShipStates.SomeCornersSurface)
        {
            _rb.AddForce(-_shipCollision.surfaceNormal * 3.25f, ForceMode.Acceleration);
        }
    }

    public void ApplyForwardForce(float force)
    {
        _rb.AddForce(force * transform.forward, ForceMode.Acceleration);

        //if (_controller.is) return;

        // Kill velocity to 0 for small car velocities
        if (force == 0 && _controller.forwardSpeedAbs < 0.1 && !_inputManager.isDrift)
            _rb.velocity -= Vector3.Dot(_rb.velocity, transform.forward) * transform.forward;
    }

    private float CalcForwardForce(float throttleInput)
    {
        // Throttle
        float forwardAcceleration = 0;

        if (_inputManager.isBoost)
            forwardAcceleration = GetForwardAcceleration(_controller.forwardSpeedAbs);
        else
            forwardAcceleration = throttleInput * GetForwardAcceleration(_controller.forwardSpeedAbs);

        if (_inputManager.isDrift && !disableDrift)
            forwardAcceleration *= driftFactor;
        else if (_controller.forwardSpeedSign != Mathf.Sign(throttleInput) && throttleInput != 0)
            forwardAcceleration += -1 * _controller.forwardSpeedSign * 35; // Braking
        return forwardAcceleration;
    }


    private float CalculateForwardForce(float input, float speed)
    {
        return input * GetForwardAcceleration(_controller.forwardSpeedAbs);
    }

    private float CalculateSteerAngle()
    {
        var curvature = GetTurnRadius(_controller.forwardSpeed);
        return _inputManager.steerInput * curvature * turnRadiusCoefficient;
    }

    static float GetForwardAcceleration(float speed)
    {
        // Replicates acceleration curve from RL, depends on current car forward velocity
        speed = Mathf.Abs(speed);
        float throttle = 0;

        if (speed > (1410 / 100))
            throttle = 0;
        else if (speed > (1400 / 100))
            throttle = RoboUtils.Scale(14, 14.1f, 1.6f, 0, speed);
        else if (speed <= (1400 / 100))
            throttle = RoboUtils.Scale(0, 14, 16, 1.6f, speed);

        return throttle;
    }

    static float GetTurnRadius(float speed)
    {
        var forwardSpeed = Mathf.Abs(speed);

        var curvature = RoboUtils.Scale(0, 5, 0.0069f, 0.00398f, forwardSpeed);

        if (forwardSpeed >= 500 / 100)
            curvature = RoboUtils.Scale(5, 10, 0.00398f, 0.00235f, forwardSpeed);

        if (forwardSpeed >= 1000 / 100)
            curvature = RoboUtils.Scale(10, 15, 0.00235f, 0.001375f, forwardSpeed);

        if (forwardSpeed >= 1500 / 100)
            curvature = RoboUtils.Scale(15, 17.5f, 0.001375f, 0.0011f, forwardSpeed);

        if (forwardSpeed >= 1750 / 100)
            curvature = RoboUtils.Scale(17.5f, 23, 0.0011f, 0.00088f, forwardSpeed);

        float turnRadius = 1 / (curvature * 100);
        return turnRadius;
    }
}