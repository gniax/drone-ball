using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnFromControl : MonoBehaviour
{
    public void Return(){
        int indexOfSceneToLoad = SceneManager.GetActiveScene().buildIndex - 2;
        //Debug.Log("Loading level at id: " + indexOfSceneToLoad + "!");
        SceneManager.LoadScene(indexOfSceneToLoad);
    }
}
