using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster Wave", menuName = "ScriptableObjects/MonsterWave")]
public class MonsterWaveSO : ScriptableObject
{
    [SerializeField] public List<GameObject> monstersWave1; 
    [SerializeField] public List<GameObject> monstersWave2; 
    [SerializeField] public List<GameObject> monstersWave3;
    [SerializeField] public List<GameObject> bossMonsters;
}
