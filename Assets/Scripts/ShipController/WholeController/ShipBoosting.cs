using TMPro;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class ShipBoosting : MonoBehaviour
{
    private const float BoostForce = 991.666f / 100f;
    
    private int _boostCountdown = 13;
    private ShipController _c;
    private CustomInputManager _inputManager;
    private Rigidbody _rb;
    //private GUIStyle _style;

    public TextMeshProUGUI boostTextValue;
    public bool isBoosting;
    public bool infiniteBoosting;
    public bool disableBoosting;
    public float boostForceMultiplier = 2f;
    public float boostAmount = 100f;

    private void Start()
    {
        /*
        _style = new GUIStyle();
        _style.normal.textColor = Color.red;
        _style.fontSize = 25;
        _style.fontStyle = FontStyle.Bold;
        */

        if (boostTextValue)
        {
            boostTextValue.text = ((int)boostAmount).ToString();
        }

        if (infiniteBoosting)
        {
            boostAmount = 100f;
        }

        _inputManager = GetComponentInParent<CustomInputManager>();
        _c = GetComponent<ShipController>();
        _rb = GetComponentInParent<Rigidbody>();

        // Activate ParticleSystems GameObject
        if (Resources.FindObjectsOfTypeAll<ShipParticleSystem>()[0] != null)
        {
            Resources.FindObjectsOfTypeAll<ShipParticleSystem>()[0].gameObject.SetActive(true);
        }

        InvokeRepeating("UpdateBoostValue", 1f, 1f);
    }

    private void UpdateBoostValue()
    {
        if (!isBoosting)
        {
            boostAmount += 15f;
            if (boostAmount > 100)
            {
                boostAmount = 100;
            }
        }

        if (boostTextValue)
        {
            boostTextValue.text = ((int)boostAmount).ToString();
        }
    }

    void FixedUpdate()
    {
        Boosting();
    }

    private void Boosting()
    {
        if(disableBoosting) return;

        if (_inputManager.isBoost || (_boostCountdown < 13 && _boostCountdown > 0) )
        {
            _boostCountdown--;
            if (boostAmount > 0)
            {
                isBoosting = true;
                if (_c.forwardSpeed < ShipController.MaxSpeedBoost)
                {
                    _rb.AddForce(BoostForce * boostForceMultiplier * transform.forward, ForceMode.Acceleration);
                }

                if (!infiniteBoosting)
                {
                    boostAmount = Mathf.Max(0.0f, boostAmount - 0.27f);
                    if (boostTextValue)
                    {
                        boostTextValue.text = ((int)boostAmount).ToString();
                    }
                }
            }
            else
            {
                isBoosting = false;
            }
        }
        else
        {
            _boostCountdown = 13;
            isBoosting = false;
        }
    }

    public void SetInfiniteBoost(bool infiniteBoost)
    {
        infiniteBoosting = infiniteBoost;
    }

    //returns true if the boost was alreade 100
    public bool IncreaseBoost(float boost)
    {
        if (boostAmount == 100)
        {
            return true;
        }

        boostAmount = Mathf.Min(100f, boostAmount + boost);
        return false;
    }

    /*void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 140, Screen.height - 50, 150, 130), $"Boost {(int)boostAmount}", _style);
    }*/
}