using UnityEngine;

[RequireComponent(typeof(CustomInputManager))]
public class GameManager : MonoBehaviour
{
    public static CustomInputManager InputManager;
    public static AudioManager AudioManager;
    
    void Awake()
    {
        InputManager = GetComponent<CustomInputManager>();
        AudioManager = GetComponent<AudioManager>();
        //DontDestroyOnLoad(gameObject);
    }
}