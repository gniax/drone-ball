using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    public GameObject mainMenu;

    public void PlayGame(){
        int indexOfSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        //Debug.Log("Loading level at id: " + indexOfSceneToLoad + "!");
        SceneManager.LoadScene(indexOfSceneToLoad);
    }

    public void DisplayCredits(){
        int indexOfSceneToLoad = SceneManager.GetActiveScene().buildIndex + 3;
        //Debug.Log("Loading level at id: " + indexOfSceneToLoad + "!");
        SceneManager.LoadScene(indexOfSceneToLoad);
    }

    public void DisplayControlPanel(){
        int indexOfSceneToLoad = SceneManager.GetActiveScene().buildIndex + 2;
        //Debug.Log("Loading level at id: " + indexOfSceneToLoad + "!");
        SceneManager.LoadScene(indexOfSceneToLoad);
    }

    public void Quit(){
        Application.Quit();
    }
}
