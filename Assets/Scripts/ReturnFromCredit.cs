using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnFromCredit : MonoBehaviour
{
    public void Return(){
        int indexOfSceneToLoad = SceneManager.GetActiveScene().buildIndex - 3;
        //Debug.Log("Loading level at id: " + indexOfSceneToLoad + "!");
        SceneManager.LoadScene(indexOfSceneToLoad);
    }
}
