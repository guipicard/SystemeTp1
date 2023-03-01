using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_LevelNameText;
    [SerializeField] private TextMeshProUGUI m_LevelDescriptionText;
    [SerializeField] private Text m_ButtonText; 
    [SerializeField] private GameObject Player;

    private BackgroundMouvment PlayerScript;

    private string m_LevelName;
    private string[] m_LevelDescription;

    private Coroutine m_Coroutine;

    private int letterIndex;

    private int SentenceIndex;

    private bool m_PlayingDescription;

    // Start is called before the first frame update
    void Start()
    {
        DisableSelf();
        letterIndex = 0;
        SentenceIndex = 0;
        m_PlayingDescription = false;

        PlayerScript = Player.GetComponent<BackgroundMouvment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PlayingDescription)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                MessageHandler();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LevelCoroutineStop();
            }
        }
    }

    public void MessageHandler()
    {
        if (letterIndex < m_LevelDescription[SentenceIndex].Length)
        {
            m_LevelDescriptionText.text = m_LevelDescription[SentenceIndex];
            letterIndex = m_LevelDescription[SentenceIndex].Length;
        }
        else if (SentenceIndex == m_LevelDescription.Length - 1)
        {
            m_ButtonText.text = "Play!";
        }
        else if (letterIndex == m_LevelDescription[SentenceIndex].Length)
        {
            SentenceIndex++;
            m_Coroutine = StartCoroutine(LevelDescriptionCoroutine());
        }
    }

    public void LevelCoroutineManager(ScripableDescriptions LevelInfos)
    {
        m_LevelName = LevelInfos.m_name;
        m_LevelDescription = LevelInfos.m_Description;
        SentenceIndex = 0;
        EnableSelf();
        m_ButtonText.text = "Next...";
        m_PlayingDescription = true;

        m_Coroutine = StartCoroutine(LevelDescriptionCoroutine());
    }

    public void LevelCoroutineStop()
    {
        StopCoroutine(m_Coroutine);
        m_PlayingDescription = false;
        DisableSelf();
    }

    IEnumerator LevelDescriptionCoroutine()
    {
        m_LevelDescriptionText.text = "";
        m_LevelNameText.text = m_LevelName;
        letterIndex = 0;
        while (letterIndex < m_LevelDescription[SentenceIndex].Length)
        {
            m_LevelDescriptionText.text += m_LevelDescription[SentenceIndex][letterIndex];
            letterIndex++;

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void DisableSelf()
    {
        GetComponent<Canvas>().enabled = false;
    }

    private void EnableSelf()
    {
        GetComponent<Canvas>().enabled = true;
    }
}