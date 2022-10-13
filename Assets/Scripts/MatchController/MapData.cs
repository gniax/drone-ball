using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MapData : MonoBehaviour
{
    // game score hud
    public TextMeshProUGUI scoreEsipeTMP;
    public TextMeshProUGUI scoreEsieeTMP;

    public float diag;
    public bool isScoredBlue = false;
    public bool isScoredOrange = false;
    public int blueScore = 0;
    public int orangeScore = 0;

    private GoalController _blueGoal;
    private GoalController _redGoal;

    //private GUIStyle _styleOrangeLabel;
   // private GUIStyle _styleBlueLabel;


    void Start()
    {
        diag = Vector3.Distance(transform.Find("diagP1").position, transform.Find("diagP2").position);
        _blueGoal = transform.Find("GoalLines").transform.Find("GoalLineBlue").GetComponent<GoalController>();
        _redGoal = transform.Find("GoalLines").transform.Find("GoalLineRed").GetComponent<GoalController>();

        if (scoreEsipeTMP)
        {
            scoreEsipeTMP.text = blueScore.ToString();
        }

        if (scoreEsieeTMP)
        {
            scoreEsieeTMP.text = orangeScore.ToString();
        }

        /*
        _styleOrangeLabel = new GUIStyle();
        _styleOrangeLabel.normal.textColor = new Color(1.0f, 0.47f, 0f);
        _styleOrangeLabel.fontSize = 25;
        _styleOrangeLabel.fontStyle = FontStyle.Bold;
        _styleOrangeLabel.alignment = TextAnchor.UpperCenter;

        _styleBlueLabel = new GUIStyle();
        _styleBlueLabel.normal.textColor = new Color(0f, 0.72f, 1f);
        _styleBlueLabel.fontSize = 25;
        _styleBlueLabel.fontStyle = FontStyle.Bold;
        _styleBlueLabel.alignment = TextAnchor.UpperCenter;
        */
    }

    public void NotifyScore(bool isBlueTeam)
    {
        if (isBlueTeam)
        {
            isScoredBlue = true;
            blueScore++;
            if (scoreEsipeTMP)
            {
                scoreEsipeTMP.text = blueScore.ToString();
            }
        }
        else
        {
            isScoredOrange = true;
            orangeScore++;
            if (scoreEsieeTMP)
            {
                scoreEsieeTMP.text = orangeScore.ToString();
            }
        }
    }

    public void ResetIsScored()
    {
        isScoredBlue = false;
        isScoredOrange = false;
    }

}
