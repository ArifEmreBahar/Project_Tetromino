using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tomino.Game;

public class UIManager : MonoBehaviour
{
    #region VARIABLES

    public event GameEventHandler OnStartButtonEvent = delegate { };
    public event GameEventHandler OnEscapeButtonEvent = delegate { };

    #endregion

    #region UNITY

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnEscapeButtonEvent();
    }

    #endregion

    #region PUBLIC

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnStartButton()
    {
        OnStartButtonEvent();
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    #endregion
}
