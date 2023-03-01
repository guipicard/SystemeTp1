using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private float NewGameTransitionDuration;
    [SerializeField] private RectTransform m_ButtonsContainer;
    [SerializeField] private Vector3 m_NewButtonsPosition;
    private float NewGameTransitionTime;
    private Coroutine StartNewGameCoroutine;
    private Vector3 m_ButtonsStartPosition;
    private bool m_ButtonsGoDown;
    private bool m_StartNewGame;


    void Start()
    {
        NewGameTransitionTime = 0.2f;
        m_StartNewGame = false;
        m_ButtonsStartPosition = m_ButtonsContainer.anchoredPosition3D;
        m_ButtonsGoDown = true;

    }

    void Update()
    {
        if (m_StartNewGame)
        {
            if (NewGameTransitionTime >= NewGameTransitionDuration)
            {
                StopCoroutine(StartNewGameCoroutine);
                SceneManager.LoadScene("SelectionNiveau");
            }
        }
    }

    private IEnumerator NewGameCoroutine()
    {
        while (true)
        {
            if (m_ButtonsGoDown && NewGameTransitionTime < 0)
            {
                m_ButtonsGoDown = false;
                // m_ButtonsStartPosition = m_ButtonsContainer.anchoredPosition3D;
            }
            NewGameTransitionTime = m_ButtonsGoDown ? NewGameTransitionTime - Time.deltaTime : NewGameTransitionTime + Time.deltaTime;
            
            // a + (b-a) * t
            m_ButtonsContainer.anchoredPosition3D = m_ButtonsStartPosition + (m_NewButtonsPosition - m_ButtonsStartPosition) * 
                EaseOut(NewGameTransitionTime / NewGameTransitionDuration);
            yield return null;
        }
    }
    
    private float EaseOut(float t)
    {
        return t * t;
        // return t * Mathf.Abs(t);
    }

    public void NewGameButton()
    {
        m_StartNewGame = true;
        StartNewGameCoroutine = StartCoroutine(NewGameCoroutine());
    }

    public void LoadGameButton()
    {
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}