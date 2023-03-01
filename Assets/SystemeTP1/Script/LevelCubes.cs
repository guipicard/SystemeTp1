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

    public ScripableDescriptions m_ScriptableDescription;

    private Action<ScripableDescriptions> m_LevelAction;
    
    void Start()
    {

    }
    
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.CompareTag("Player"))
        {
            StartLevelCoroutine();
        }
    }

    private void StartLevelCoroutine()
    {
        m_LevelAction = m_LevelCanvas.GetComponent<LevelInfo>().LevelCoroutineManager;
        m_LevelAction(m_ScriptableDescription);
    }
    
}
