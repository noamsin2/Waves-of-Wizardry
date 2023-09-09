using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance { get; private set; }
 
    public delegate void ExperienceChangeHandler(int exp);
    public event ExperienceChangeHandler OnExpChange;

    private void Awake()
    {
        Instance = this;
    }
    public void AddExperience(int exp)
    {
        OnExpChange?.Invoke(exp);
    }
}
