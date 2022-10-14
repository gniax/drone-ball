using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private CustomInputManager cInputManager;

    private void Update()
    {
        if (cInputManager == null)
        {
            return;
        }

        if (cInputManager.isBoost == true)
        {
            // todo: le son
        }
    }

}
