using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]


public class ScripableDescriptions : ScriptableObject
{
    public string m_name;
    public string[] m_Description;
    public int m_Stars;
}
