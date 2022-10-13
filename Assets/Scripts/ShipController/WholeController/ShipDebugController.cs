using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDebugController : MonoBehaviour
{
    private ShipGroundTrigger[] _sphereArray;
    
    private void Start()
    {
        _sphereArray = GetComponentsInChildren<ShipGroundTrigger>();
        int sphereArrayLength = _sphereArray.Length;
        if (sphereArrayLength > 0)
        {
            _isDrawRaycasts = _sphereArray[0].isDrawContactLines;
        }
    }
    
    //[Button("@\"Draw All Contact Lines: \" + _isDrawRaycasts", ButtonSizes.Large)]
    void DrawRaycast()
    {
        _isDrawRaycasts = !_isDrawRaycasts;
        foreach (var sphereCollider in _sphereArray)
        {
            sphereCollider.isDrawContactLines = _isDrawRaycasts;
        }
        
    }
    bool _isDrawRaycasts;
}
