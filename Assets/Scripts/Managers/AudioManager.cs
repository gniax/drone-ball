using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public ShipBoosting _shipBoosting;
    public CustomInputManager cInputManager;
    public AudioSource source;
    [SerializeField]
    public AudioClip boost;


    public void Start()
    {
        cInputManager = this.GetComponent<CustomInputManager>();
        source = this.GetComponent<AudioSource>();
        _shipBoosting = this.GetComponentInChildren<ShipBoosting>();
    }


    private void Update()
    {

        if (cInputManager == null)
        {
            return;
        }

        if (cInputManager.myInput != null && cInputManager.myInput.Action2.IsPressed)
        {
            if (_shipBoosting != null && _shipBoosting.isBoosting)
                source.PlayOneShot(boost);
        }
        else if (cInputManager.myInput != null && !cInputManager.myInput.Action2.IsPressed)
        {
            if (source.isPlaying)
            source.Stop();
        }
    }

}