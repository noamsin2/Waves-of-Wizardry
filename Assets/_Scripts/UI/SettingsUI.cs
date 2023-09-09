using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private GameMenuUI gameMenuUI;
    [SerializeField] private ControlsUI controlsUI;
    [SerializeField] private AudioUI audioUI;
    [SerializeField] private VideoUI videoUI;
    [SerializeField] private InterfaceUI interfaceUI;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button videoButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button interfaceButton;
    [SerializeField] private Button backButton;

    public event EventHandler OnBackClick;

    private void Awake()
    {
        controlsButton.onClick.AddListener(() => {

            Hide();
        });
        videoButton.onClick.AddListener(() => {

            Hide();
        });
        audioButton.onClick.AddListener(() => {

            Hide();
        });
        interfaceButton.onClick.AddListener(() => {

            Hide();
        });
        backButton.onClick.AddListener(() => {
            OnBackClick?.Invoke(this, EventArgs.Empty);
            Hide();
        });
    }
    void Start()
    {
        gameMenuUI.OnOpenSettings += GameMenuUI_OnOpenSettings;
        gameMenuUI.OnCloseSettings += GameMenuUI_OnCloseSettings;
        controlsUI.OnBackClick += Settings_OnBackClick;
        videoUI.OnBackClick += Settings_OnBackClick;
        audioUI.OnBackClick += Settings_OnBackClick;
        interfaceUI.OnBackClick += Settings_OnBackClick;
        
        Hide();
    }

    private void Settings_OnBackClick(object sender, EventArgs e)
    {
        Show();
    }

    private void GameMenuUI_OnCloseSettings(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameMenuUI_OnOpenSettings(object sender, System.EventArgs e)
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
