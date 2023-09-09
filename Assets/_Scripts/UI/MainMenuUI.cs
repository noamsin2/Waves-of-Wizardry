using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private ShopMenuUI shopMenuUI;
    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        shopButton.onClick.AddListener(() =>
        {
            shopMenuUI.Load();
        });
        
    }
}
