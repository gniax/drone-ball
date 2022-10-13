using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class ShipAirControl : MonoBehaviour
{
    public bool isUseDamperTorque = true;
    
    float _inputRoll = 0, _inputPitch = 0, _inputYaw = 0;

    ShipDodging _shipDodging;
    Rigidbody _rb;
    private Transform _cogLow;
    InputManager _inputManager;
    ShipController _controller;
    
    #region Torque Coefficients for rotation and drag
    const float Tr = 5.07956616966136f; // torque coefficient for roll
    const float Tp = 4.14599781908070f; // torque coefficient for pitch
    const float Ty = 3.91962804287785f; // torque coefficient for yaw
    const float Dr = -4.75f; // drag coefficient for roll
    const float Dp = -2.85f; // drag coefficient for pitch
    const float Dy = -1.886491900437232f; // drag coefficient for yaw
    #endregion

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _cogLow = transform.Find("cogLow");
        _inputManager = GetComponentInParent<InputManager>();
        _controller = GetComponent<ShipController>();
        _shipDodging = GetComponent<ShipDodging>();
    }

    void Update()
    {
        _inputYaw = _inputManager.yawInput;
        _inputPitch = _inputManager.pitchInput;
        _inputRoll = _inputManager.rollInput;

        if (_inputManager.isAirRoll)
        {
            _inputRoll = -_inputYaw;
            _inputYaw = 0;
        }
    }

    private void FixedUpdate()
    {
        if (_controller.numCornersSurface >= 3) return;

        if (!_shipDodging.isDodge || _shipDodging.isCancelled)
        {
            // pitch
            _rb.AddTorque(Tp * _inputPitch * -_cogLow.right, ForceMode.Impulse);
            if (isUseDamperTorque)
            {
                _rb.AddTorque(
                    _cogLow.right * (Dp * (1 - Mathf.Abs(_inputPitch)) * _cogLow.InverseTransformDirection(_rb.angularVelocity).x),
                    ForceMode.Impulse
                );
            }
        }

        if (!_shipDodging.isDodge)
        {
            // roll
            _rb.AddTorque(Tr * _inputRoll * _cogLow.forward, ForceMode.Impulse);
            if (isUseDamperTorque)
            {
                _rb.AddTorque(
                    _cogLow.forward * Dr * _cogLow.InverseTransformDirection(_rb.angularVelocity).z,
                    ForceMode.Impulse
                );
            }
            //yaw
            _rb.AddTorque(Ty * _inputYaw * _cogLow.up, ForceMode.Impulse);
            if (isUseDamperTorque)
            {
                _rb.AddTorque(
                    _cogLow.up * (Dy * (1 - Mathf.Abs(_inputYaw)) * _cogLow.InverseTransformDirection(_rb.angularVelocity).y),
                    ForceMode.Impulse
                );
            }
        }
    }
}