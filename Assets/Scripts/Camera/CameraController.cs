using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float distanceToBall = 20;

    [Header("Ballcam on settings")]
    public float cameraDist = 9f;
    public float cameraHeight = 3f;
    public float cameraAngle = 2.3f;
    public float stiffnessPosition = 50;
    public float stiffnessAngle = 30;

    public bool keyBoardControl = true;

    float _rotationUpdateTime = 0.05f;
    float _lastRotationUpdate = 0f;

    Vector3 _actualRotationAxis = Vector3.zero;
    Vector3 _desiredRotationAxis = Vector3.zero;
    float _tmpRotationX = 0f;
    float _tmpRotationY = 0f;

    public Transform Ball;
    public GameObject ShipControllableReference;
    private Transform _shipControllableTR;
    private ShipController _shipController;
    private CustomInputManager _inputManager;


    Vector3 _checkVelocity;
    Vector3 _prevPosistion;

    Vector3 _pivotPosition;

    private bool _isBallCam = false;
    void Start()
    {
        _shipControllableTR = ShipControllableReference.transform;
        _shipController = ShipControllableReference.GetComponentInChildren<ShipController>();
        if (_shipController == null)
        {
            Debug.LogError("Vous devez mettre la reference de ShipControllable sur votre CameraController!!");
            return;
        }
        _inputManager = ShipControllableReference.GetComponent<CustomInputManager>();

        _pivotPosition = _shipControllableTR.position + Vector3.up * cameraHeight;
        _checkVelocity = Vector3.zero;
        _prevPosistion = _shipControllableTR.position;
    }

    void Update()
    {
        if (_inputManager?.myInput != null && _inputManager.myInput.Action4.IsPressed)
            _isBallCam = !_isBallCam;
    }

    private void FixedUpdate()
    {
        _checkVelocity = (_shipControllableTR.position - _prevPosistion) / Time.deltaTime;
        _prevPosistion = _shipControllableTR.position;
        //Debug.Log("X: " + _checkVelocity.x + "Y: " + _checkVelocity.y + "Z: " + _checkVelocity.z);
        UpdatePivotElement(stiffnessPosition);
        UpdateCamDirection(stiffnessAngle);
        UpdateCamPositon(stiffnessPosition);
    }

    Vector3 AddRotation(Vector3 startPos)
    {
        float sensitivityX;
        float sensitivityY;

        if (keyBoardControl)
        {
            sensitivityX = 0.05f;
            sensitivityY = 0.05f;
        }
        else
        {
            sensitivityX = 120f;
            sensitivityY = 60f;
        }

        float minimumX = -120f;
        float maximumX = 120f;
        float minimumY = -10;
        float maximumY = 60F;

        if (_inputManager?.myInput == null)
        {
            return Vector3.zero;
        }
        _tmpRotationX += _inputManager.myInput.RightStickX.Value * sensitivityX * (Time.deltaTime / _rotationUpdateTime);
        _tmpRotationX = Mathf.Clamp(_tmpRotationX, minimumX, maximumX);
        _tmpRotationY += _inputManager.myInput.RightStickY.Value * sensitivityY * (Time.deltaTime / _rotationUpdateTime);
        _tmpRotationY = Mathf.Clamp(_tmpRotationY, minimumY, maximumY);

        if (Time.time - _lastRotationUpdate >= _rotationUpdateTime)
        {
            _desiredRotationAxis = new Vector3(_tmpRotationY, _tmpRotationX, 0f);
            _tmpRotationX = 0f;
            _tmpRotationY = 0f;
            _lastRotationUpdate = Time.time;
        }

        _actualRotationAxis = Vector3.Lerp(_actualRotationAxis, _desiredRotationAxis, _rotationUpdateTime);

        return Quaternion.Euler(_actualRotationAxis) * (startPos - _pivotPosition) + _pivotPosition;
    }
    void UpdatePivotElement(float stiffnessPos)
    {
        Vector3 desiredPosition;
        ShipController.ShipStates carState = _shipController.shipState;
        bool grounded = carState == global::ShipController.ShipStates.AllCornersSurface || carState == global::ShipController.ShipStates.SomeCornersSurface;
        if (grounded)
        {
            desiredPosition = _shipControllableTR.position + _shipControllableTR.up * cameraHeight;
        }
        else
        {
            desiredPosition = _shipControllableTR.position + Vector3.up * cameraHeight;
        }
        _pivotPosition = Vector3.Lerp(_pivotPosition, desiredPosition, stiffnessPos * Time.deltaTime);
    }

    void UpdateCamPositon(float stiffnessPos)
    {
        Vector3 desiredPosition;
        ShipController.ShipStates carState = _shipController.shipState;
        bool grounded = carState == global::ShipController.ShipStates.AllCornersSurface || carState == global::ShipController.ShipStates.SomeCornersSurface;
        if (_isBallCam)
        {

            desiredPosition = _pivotPosition + (_shipControllableTR.position - Ball.position).normalized * cameraDist;
        }
        else
        {
            if (grounded)
            {
                desiredPosition = _pivotPosition - _shipControllableTR.forward * cameraDist;
            }
            else
            {
                desiredPosition = _pivotPosition - ShipControllableReference.GetComponent<Rigidbody>().velocity.normalized * cameraDist;
            }
        }
        desiredPosition = AddRotation(desiredPosition);
        if (grounded)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, stiffnessPos * Time.deltaTime);
        }
        else
        {
            if (Mathf.Abs(_checkVelocity.x) < 3f && Mathf.Abs(_checkVelocity.z) < 3f)
            {
                transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.01f * stiffnessPos * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.4f * stiffnessPos * Time.deltaTime);

            }
        }
    }

    void UpdateCamDirection(float stiffnessAngle)
    {
        Vector3 angleOffset = new Vector3(0, cameraAngle, 0);
        Vector3 desiredAngle = _pivotPosition - transform.position + angleOffset;
        Quaternion rot = Quaternion.LookRotation(desiredAngle, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, stiffnessAngle * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, Ball.position);
    }

    RaycastHit Raycast(Vector3 origin, Vector3 direction, float maxDist)
    {
        Physics.Raycast(origin, direction, out RaycastHit hit, maxDist);
        return hit;
    }
    bool isRaycast(Vector3 origin, Vector3 direction, float maxDist)
    {
        return Physics.Raycast(origin, direction, maxDist);
    }
}