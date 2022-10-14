using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public CustomInputManager cInputManager;
    public AudioSource source;
    [SerializeField]
    public AudioClip boost;


    public void Start()
    {
        cInputManager = this.GetComponent<CustomInputManager>();
        source = this.GetComponent<AudioSource>();
    }


    private void Update()
    {
        if (cInputManager == null)
        {
            return;
        }

        if (cInputManager.myInput != null && cInputManager.myInput.Action4.IsPressed)
        {
            source.Play();
            //Debug.Log("zaklzeajleaz");
        }
        else
        {
            if (source.isPlaying)
            source.Stop();
        }
    }

}