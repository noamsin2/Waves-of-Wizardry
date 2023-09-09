using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameOverUI : MonoBehaviour
{
    
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI wave;
    [SerializeField] private TextMeshProUGUI waveMaxScore;
    private void Awake()
    {
        playAgainButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        GameManager.Instance.TotalGoldAccumulated += GameManager_TotalGoldAccumulated;
        Hide();
    }

    private void GameManager_TotalGoldAccumulated(int gold)
    {
        this.gold.text = gold.ToString();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOverState())
        {
            Show();
            wave.text = WaveManager.Instance.wave.ToString();
            waveMaxScore.text = PlayerPrefs.GetInt("wave",0).ToString();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
     

    }

    protected void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
