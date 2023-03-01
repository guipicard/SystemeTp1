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
    [SerializeField] private GameObject Player;

    private BackgroundMouvment PlayerScript;

    private string m_LevelName;
    private string[] m_LevelDescription;

    private Coroutine m_Coroutine;

    private int letterIndex;

    private int SentenceIndex;

    // Start is called before the first frame update
    void Start()
    {
        DisableSelf();
        letterIndex = 0;
        SentenceIndex = 0;

        PlayerScript = Player.GetComponent<BackgroundMouvment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerScript.m_CanGetInput)
        {
            if (letterIndex >= m_LevelDescription[SentenceIndex].Length)
            {
                StopCoroutine(m_Coroutine);
                if (SentenceIndex >= m_LevelDescription.Length - 1)
                {
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        PlayerScript.m_CanGetInput = true;
                        DisableSelf();
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        SentenceIndex++;
                        m_Coroutine = StartCoroutine(LevelDescriptionCoroutine());
                    }

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        PlayerScript.m_CanGetInput = true;
                        DisableSelf();
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_LevelDescriptionText.text = m_LevelDescription[SentenceIndex];
                    letterIndex = m_LevelDescription[SentenceIndex].Length;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopCoroutine(m_Coroutine);
                PlayerScript.m_CanGetInput = true;
                DisableSelf();
            }
        }
    }

    public void LevelCoroutineManager(ScripableDescriptions LevelInfos)
    {
        m_LevelName = LevelInfos.m_name;
        m_LevelDescription = LevelInfos.m_Description;
        PlayerScript.m_CanGetInput = false;
        SentenceIndex = 0;
        EnableSelf();

        m_Coroutine = StartCoroutine(LevelDescriptionCoroutine());
    }

    IEnumerator LevelDescriptionCoroutine()
    {
        m_LevelDescriptionText.text = "";
        letterIndex = 0;
        while (true)
        {
            m_LevelDescriptionText.text += m_LevelDescription[SentenceIndex][letterIndex];
            letterIndex++;

            yield return new WaitForSeconds(0.5f);
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