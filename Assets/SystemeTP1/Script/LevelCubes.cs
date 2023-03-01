using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;


public class LevelCubes : MonoBehaviour
{

    [SerializeField] private Canvas m_LevelCanvas;

    private LevelInfo m_LevelInfo;

    public ScripableDescriptions m_ScriptableDescription;

    void Start()
    {
        m_LevelInfo = m_LevelCanvas.GetComponent<LevelInfo>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartLevelCoroutine();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopLevelCoroutine();
        }
    }

    private void StartLevelCoroutine()
    {
        m_LevelInfo.LevelCoroutineManager(m_ScriptableDescription);
    }

    private void StopLevelCoroutine()
    {
        m_LevelInfo.LevelCoroutineStop();
    }
    
}
