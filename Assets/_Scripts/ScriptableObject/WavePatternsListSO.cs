using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave Pattern List", menuName = "ScriptableObjects/Wave Pattern List")]
public class WavePatternsListSO : ScriptableObject
{
    [SerializeField] public List<MonsterWaveSO> wave1Patterns;
    [SerializeField] public List<MonsterWaveSO> wave2Patterns;
    [SerializeField] public List<MonsterWaveSO> wave3Patterns;
    [SerializeField] public List<MonsterWaveSO> wave4Patterns;
}
