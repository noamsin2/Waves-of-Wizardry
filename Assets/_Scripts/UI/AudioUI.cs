using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AudioUI : MonoBehaviour
{
    [SerializeField] private GameMenuUI gameMenuUI;
    [SerializeField] private Button backButton;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI effectsText;

    public event EventHandler OnBackClick;

    private void Awake()
    {
        backButton.onClick.AddListener(() =>
        {
            OnBackClick?.Invoke(this, EventArgs.Empty);
            Hide();
        });
        volumeSlider.onValueChanged.AddListener((v) =>
        {
            AudioManager.Instance.SetTotalVolume(v);
            MusicManager.Instance.SetTotalVolume(v);
            volumeText.text = v.ToString("0.00");
        });
        musicSlider.onValueChanged.AddListener((v) =>
        {
            MusicManager.Instance.SetMusicVolume(v);
            musicText.text = v.ToString("0.00");
        });
        effectsSlider.onValueChanged.AddListener((v) =>
        {
            AudioManager.Instance.SetEffectsVolume(v);
            effectsText.text = v.ToString("0.00");
        });
    }
    void Start()
    {
        InitializeVolumes();

        gameMenuUI.OnCloseSettings += GameMenuUI_OnCloseSettings;
        
        Hide();
    }

    private void InitializeVolumes()
    {
        volumeSlider.value = AudioManager.Instance.GetTotalVolume();
        musicSlider.value = MusicManager.Instance.GetMusicVolume();
        effectsSlider.value = AudioManager.Instance.GetEffectsVolume();
        volumeText.text = volumeSlider.value.ToString("0.00");
        musicText.text = musicSlider.value.ToString("0.00");
        effectsText.text = effectsSlider.value.ToString("0.00");
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
