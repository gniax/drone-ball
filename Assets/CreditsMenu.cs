using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMeu : MonoBehaviour
{
    [SerializeField]
    public GameObject mainMenu;

    // Start is called before the first frame update
    void Awake()
    {
        if (mainMenu == null)
        {
            return;
        }

        mainMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
