using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMouvment : MonoBehaviour
{
    [SerializeField]
    Transform[] Backgrounds1;

    private bool m_LeftInput;
    private bool m_RightInput;
    void Start()
    {
        m_LeftInput = false;
        m_RightInput = false;
    }


    void Update()
    {
        Inputs();
        BackGroundParallax(m_LeftInput, m_RightInput);
    }

    private void Inputs()
    {
        if (Input.GetKey(KeyCode.A))
        {
            m_LeftInput = true;
        }
        else
        {
            m_LeftInput = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_RightInput = true;
        }
        else
        {
            m_RightInput = false;
        }
    }

    private void BackGroundParallax(bool left, bool right)
    {
        for (int i = 0; i < Backgrounds1.Length; i++)
        {
            if (left)
            {
                Backgrounds1[i].Translate(Vector3.right * ((Backgrounds1.Length + 1) - i) * Time.deltaTime);
            }
            if (right)
            {
                Backgrounds1[i].Translate(Vector3.left * ((Backgrounds1.Length + 1) - i) * Time.deltaTime);
            }
        }

    }
}
