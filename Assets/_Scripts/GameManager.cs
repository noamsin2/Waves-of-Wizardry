using System;
using System.Collections;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player.GoldEventHandler TotalGoldAccumulated;

    public event EventHandler OnOpenMenu;
    public event EventHandler OnCloseMenu;
    public event EventHandler OnOpenInventory;
    public event EventHandler OnCloseInventory;
    public event EventHandler OnStateChanged;

    private bool isMenuOpen = false;
    private bool isInventoryOpen = false;
    private bool isSkillOpen = false;
    private enum State
    {
        GameLoadedUp,
        WaitingToStart,
        WaveStarted,
        WaveEnded,
        GameOver,
    }
    private State state;

    private const float WAITING_TO_START_TIMER = 3f;
    private const float WAVE_PLAYING_TIMER = 20f;

    private float waitingToStartTimer = WAITING_TO_START_TIMER;
    private float wavePlayingTimer = WAVE_PLAYING_TIMER;

    private int TotalGoldCollected = 0;
    private void Awake()
    {
        Instance = this;
        state = State.GameLoadedUp;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameInput.Instance.OnOpenMenuAction += GameInput_OnToggleMenuAction;
        GameInput.Instance.OnOpenInventoryAction += GameInput_OnOpenInventoryAction;
        WaveManager.Instance.OnAllMonstersDeath += WaveManager_OnAllMonstersDeath;
        Player.Instance.OnLevelUp += Player_OnLevelUp;
        Player.Instance.OnGoldPickup += Player_OnGoldPickup;
        SkillsUI.Instance.OnSkillAssign += SkillsUI_OnSkillAssign;
    }

    private void Player_OnGoldPickup(int gold)
    {
        TotalGoldCollected += gold;
    }

    private void SkillsUI_OnSkillAssign(object sender, EventArgs e)
    {
        Time.timeScale = 0.2f;
        StartCoroutine(ContinueGame());
    }
    private IEnumerator ContinueGame()
    {
        isSkillOpen = false;
        yield return new WaitForSeconds(0.4f);
        Time.timeScale = 1f;
    }
    private void Player_OnLevelUp(object sender, EventArgs e)
    {
        isSkillOpen = true;
        SkillsUI.Instance.Show();
        Time.timeScale = 0f;
    }

    private void Update()
    {
        switch (state)
        {
            case State.GameLoadedUp:
                GameSaver.Instance.LoadGame();
                if (!PlayerPrefs.HasKey("FirstTime"))
                {
                    TutorialUI.Instance.Show();
                }
                state = State.WaitingToStart;
                break;
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0)
                {
                    state = State.WaveStarted;
                    waitingToStartTimer = WAITING_TO_START_TIMER;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.WaveStarted:
                wavePlayingTimer -= Time.deltaTime;
                if (wavePlayingTimer < 0)
                {
                    state = State.WaveEnded;
                    wavePlayingTimer = WAVE_PLAYING_TIMER;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.WaveEnded:
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    break;
            case State.GameOver:
                GameSaver.Instance.SaveGame();
                TotalGoldAccumulated?.Invoke(TotalGoldCollected);
                OnStateChanged?.Invoke(this, EventArgs.Empty);
                break;
        }
        //Debug.Log(state);
        
    }

    public bool IsGameOverState()
    {
        return state == State.GameOver;
    }
    public void SetGameOverState()
    {
        state = State.GameOver;
    }

    private void WaveManager_OnAllMonstersDeath(object sender, EventArgs e)
    {
        state = State.WaitingToStart;
    }

    private void GameInput_OnToggleMenuAction(object sender, EventArgs e)
    {
        ToggleOpenMenu();
    }
    private void GameInput_OnOpenInventoryAction(object sender, EventArgs e)
    {
        ToggleOpenInventory();
    }
    public bool IsWaveStarted()
    {
        return state == State.WaveStarted;
    }
    public bool IsWaveEnded()
    {
        return state == State.WaveEnded;
    }
    public void ToggleOpenMenu()
    {
        if (!isInventoryOpen && !isSkillOpen)
        {
            isMenuOpen = !isMenuOpen;
            if (isMenuOpen && state != State.GameOver)
            {
                OnOpenMenu?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 0;
            }
            else
            {
                OnCloseMenu?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 1;
            }
        }
    }
    public void ToggleOpenInventory()
    {
        if (!isMenuOpen && state != State.GameOver && !isSkillOpen)
        {
            isInventoryOpen = !isInventoryOpen;
            if (isInventoryOpen)
            {
                OnOpenInventory?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 0;
            }
            else
            {
                OnCloseInventory?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 1;
            }
        }
    }
}
