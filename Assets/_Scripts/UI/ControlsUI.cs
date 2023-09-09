using System;
using UnityEngine;
using UnityEngine.UI;
public class ControlsUI : MonoBehaviour
{
    [SerializeField] private GameMenuUI gameMenuUI;
    [SerializeField] private Button backButton;
    public event EventHandler OnBackClick;

    private void Awake()
    {
        backButton.onClick.AddListener(() =>
        {
            OnBackClick?.Invoke(this, EventArgs.Empty);
            Hide();
        });
    }
    void Start()
    {
        gameMenuUI.OnCloseSettings += GameMenuUI_OnCloseSettings;
        
        Hide();
    }

    private void GameMenuUI_OnCloseSettings(object sender, EventArgs e)
    {
        Hide();
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
