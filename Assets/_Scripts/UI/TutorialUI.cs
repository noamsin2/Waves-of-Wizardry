using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            PlayerPrefs.SetInt("FirstTime", 1);
            gameObject.SetActive(false);
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
