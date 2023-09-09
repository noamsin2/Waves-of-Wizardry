using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    public event EventHandler OnAllMonstersDeath;

    private const float UP_SPAWN_Y = 5.5f;
    private const float DOWN_SPAWN_Y = -5.5f;
    private const int SECONDS_PER_MINI_WAVE = 5;
    private const int NUM_OF_MINI_WAVES = 3;
    private const int SPAWN_AT_A_TIME = 4;
    public const float SPAWN_DISTANCE_RADIUS = 10f;
    public int wave {get; private set;}
    [SerializeField] private List<WavePatternsListSO> WavePatternsList;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int enemyCount;
    [SerializeField] private TextMeshProUGUI waveNum;
    [SerializeField] private Transform bubble;
    private int enemySpawned = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        wave = 1;
        
        StartCoroutine(SpawnBubbles());
    }

    private IEnumerator SpawnBubbles()
    {
        while (true)
        {
            Transform tempBubble = Instantiate(bubble, new Vector2(UnityEngine.Random.Range(playerTransform.position.x - SPAWN_DISTANCE_RADIUS, playerTransform.position.x + SPAWN_DISTANCE_RADIUS), DOWN_SPAWN_Y), Quaternion.identity);
            Destroy(tempBubble.gameObject, 30f);
            yield return new WaitForSeconds(6f);
        }
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if(GameManager.Instance.IsWaveStarted())
        {
            Debug.Log("wave started: " + wave);
            enemyCount += 10;
            waveNum.text = wave.ToString();
            int listIndex = wave / 5;
            if (listIndex > 6)
                listIndex = 6;
            WavePatternsListSO PatternsListSO = WavePatternsList[listIndex];
            MonsterWaveSO monsterWaveSO = PatternsListSO.wavePatterns[UnityEngine.Random.Range(0, PatternsListSO.wavePatterns.Count)];
            StartCoroutine(SpawnWaves(monsterWaveSO));
        }
        else if (GameManager.Instance.IsWaveEnded())
        {
            if (enemySpawned == 0)
            {
                OnAllMonstersDeath?.Invoke(this, EventArgs.Empty);
                wave++;
            }
        }
    }
    private IEnumerator SpawnWaves(MonsterWaveSO monsterWaveSO)
    {        
        StartCoroutine(SpawnMobs(monsterWaveSO.monstersWave1));
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnMobs(monsterWaveSO.monstersWave2));
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnMobs(monsterWaveSO.monstersWave3));
        yield return new WaitForSeconds(spawnInterval);

    }
    private IEnumerator SpawnMobs(List <GameObject> monsters)
    {
        int nof_Iterations = enemyCount / SPAWN_AT_A_TIME / NUM_OF_MINI_WAVES;
        float spawnRate = 7 / (nof_Iterations / SECONDS_PER_MINI_WAVE);
        for(int i = 0; i < nof_Iterations; i++)
        {
            Instantiate(monsters[i % monsters.Count], new Vector2(UnityEngine.Random.Range(playerTransform.position.x - SPAWN_DISTANCE_RADIUS, playerTransform.position.x + SPAWN_DISTANCE_RADIUS), UP_SPAWN_Y), Quaternion.identity);
            Instantiate(monsters[i % monsters.Count], new Vector2(UnityEngine.Random.Range(playerTransform.position.x - SPAWN_DISTANCE_RADIUS, playerTransform.position.x + SPAWN_DISTANCE_RADIUS), DOWN_SPAWN_Y), Quaternion.identity);
            Instantiate(monsters[i % monsters.Count], new Vector2(playerTransform.position.x - SPAWN_DISTANCE_RADIUS, UnityEngine.Random.Range(DOWN_SPAWN_Y, UP_SPAWN_Y)), Quaternion.identity);
            Instantiate(monsters[i % monsters.Count], new Vector2(playerTransform.position.x + SPAWN_DISTANCE_RADIUS, UnityEngine.Random.Range(DOWN_SPAWN_Y, UP_SPAWN_Y)), Quaternion.identity);
            enemySpawned += SPAWN_AT_A_TIME;
            yield return new WaitForSeconds(spawnRate);
        }
        
        

    }
    public void DecreaseEnemyOnDeath()
    {
        enemySpawned -= 1;
        //Debug.Log(enemySpawned);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
