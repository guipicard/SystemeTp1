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
            if (NewGameTransitionTime < NewGameTransitionDuration)
            {
                StartNewGameCoroutine = StartCoroutine(NewGameCoroutine());
            }
            else
            {
                StopCoroutine(NewGameCoroutine());
                SceneManager.LoadScene("SelectionNiveau");
            }
        }
    }

    private IEnumerator NewGameCoroutine()
    {
        while (true)
        {
            Debug.Log("Coroutine");
            if (m_ButtonsGoDown && NewGameTransitionTime < 0)
            {
                m_ButtonsGoDown = false;
                // m_ButtonsStartPosition = m_ButtonsContainer.anchoredPosition3D;
            }
            NewGameTransitionTime = m_ButtonsGoDown ? NewGameTransitionTime - Time.deltaTime : NewGameTransitionTime + Time.deltaTime;
            
            // a + (b-a) * t
            m_ButtonsContainer.anchoredPosition3D = m_ButtonsStartPosition + (m_NewButtonsPosition - m_ButtonsStartPosition) * 
                EaseOut(NewGameTransitionTime / NewGameTransitionDuration);
            yield return new WaitForSeconds(NewGameTransitionDuration);
        }
    }
    
    private float EaseOut(float t)
    {
        return t * Mathf.Abs(t);
    }

    public void NewGameButton()
    {
        Debug.Log("button");
        m_StartNewGame = true;
    }

    public void LoadGameButton()
    {
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}