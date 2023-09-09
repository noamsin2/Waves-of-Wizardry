using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : MonoBehaviour
{
    
    [SerializeField] private Button closeMenuButton;
    [SerializeField] private Button exitGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private SettingsUI settingsUI;

    public event EventHandler OnOpenSettings;
    public event EventHandler OnCloseSettings;
    private void Awake()
    {
        closeMenuButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ToggleOpenMenu();
        });
        exitGameButton.onClick.AddListener(() =>
        {
            //save the game when clicking exit
            GameSaver.Instance.SaveGame();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        settingsButton.onClick.AddListener(() =>
        {
            Hide();
            OnOpenSettings?.Invoke(this, EventArgs.Empty);
        });
    }
    void Start()
    {
        settingsUI.OnBackClick += SettingsUI_OnBackClick;
        GameManager.Instance.OnOpenMenu += GameManager_OpenMenu;
        GameManager.Instance.OnCloseMenu += GameManager_CloseMenu;
        Hide();
    }

    private void SettingsUI_OnBackClick(object sender, EventArgs e)
    {
        Show();
    }

    private void GameManager_CloseMenu(object sender, EventArgs e)
    {
        Hide();
        OnCloseSettings?.Invoke(this, EventArgs.Empty);
        
    }

    private void GameManager_OpenMenu(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
