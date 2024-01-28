using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public float time;
    public float timer;
    public Text timerText;
    public Button retryButton;
    public CanvasGroup canvasGroup;
    public float fadeTime;
    public float fadeTimer;

    string niceTime;

    public bool gameOver;

    public void OnEnable()
    {
        retryButton.onClick.AddListener(() => Restart());
        Winbox.Win += OnWin;
        timer = time;
    }

    public void OnDisable()
    {
        retryButton.onClick.RemoveAllListeners();
        Winbox.Win -= OnWin;
    }

    public void OnWin()
    {
        Cursor.lockState = CursorLockMode.Confined;
        DataStore.finishingTime = niceTime;
        SceneManager.LoadScene("Victory Screen");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Update()
    {
        if (!gameOver)
        {
            timer = Mathf.Clamp(timer - Time.deltaTime, 0, time);
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            timerText.text = niceTime;
            if (timer <= 0)
            {
                gameOver = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
        else
        {
            if (fadeTimer < 1)
            {
                fadeTimer += Time.deltaTime / fadeTime;
            }
            canvasGroup.interactable = fadeTimer > 0.5f;
            canvasGroup.alpha = fadeTimer;
        }


    }


}
