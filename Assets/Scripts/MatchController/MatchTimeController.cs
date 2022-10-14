using System;
using TMPro;
using UnityEngine;

public class MatchTimeController : MonoBehaviour
{
    public TextMeshProUGUI scoreTMP;

    public bool paused = false;
    public int matchTimeSeconds = 300;
    public float MatchTimer { get; private set; }
    private int _minutesRemaining;
    private int _secondsRemaining;
    public bool Overtime { get; private set; } = false;

    private GUIStyle _style;

    void Awake()
    {
        MatchTimer = matchTimeSeconds;
        /*
        _style = new GUIStyle
        {
            normal = {textColor = new Color(1f, 1f, 1f)},
            fontSize = 25,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperCenter
        };
        */

    }

    void Update()
    {
        if (paused) return;
        MatchTimer -= Time.deltaTime;
        var seconds = Mathf.Floor(MatchTimer);
        _minutesRemaining = (int) seconds / 60;
        _secondsRemaining = (int) seconds % 60;
        if (scoreTMP)
        {
            scoreTMP.text = String.Format("{0:00}:{1:00}", _minutesRemaining, _secondsRemaining);
        }
    }

    private void OnGUI()
    {
        /*
        GUI.Label(new Rect(Screen.width / 2 - 75, 0f, 150, 130), $"{(Overtime ? "+" : "")}{Math.Abs(_minutesRemaining):D2}:{Math.Abs(_secondsRemaining):D2}", _style);
        if(_minutesRemaining == 0 && _secondsRemaining <= 10 && !Overtime)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 75, 150, 130), $"{_secondsRemaining}", _style);
        }
        */
    }

    public bool HasEnded()
    {
        return paused == false && MatchTimer <= .0f;// && !Overtime;
    }

    public void ActivateOvertime()
    {
        Overtime = true;
    }

    public void EndOvertime()
    {
        Overtime = false;
    }
}
