using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;

public class UIController : MonoBehaviour
{
    GameObject gameOverUI;
    GameObject pausePanel;
    GameObject controlPanel;
    Text scoreText;
    Text livesText;

    Button continueButton;
    Button restartButton;
    Button pauseButton;

    void Awake()
    {
        gameOverUI = transform.Find("GameOver").gameObject;
        pausePanel = transform.Find("PausePanel").gameObject;
        controlPanel = transform.Find("Control").gameObject;

        livesText = transform.Find("Lives").GetComponent<Text>();
        scoreText = transform.Find("Score").GetComponent<Text>();

        restartButton = pausePanel.transform.Find("RestartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(() => Restart());

        continueButton = pausePanel.transform.Find("ContinueButton").GetComponent<Button>();
        continueButton.onClick.AddListener(() => Continue());

        pauseButton = transform.Find("PauseButton").GetComponent<Button>();
        pauseButton.onClick.AddListener(() => Pause());

#if !UNITY_EDITOR
        ShowControlPanelUI();
#else
        HideControlPanelUI();
#endif
    }

    public void SetLives(int lives)
    {
        livesText.text = lives.ToString();
    }
    public void SetScore(int score)
    {
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    public void ShowGameOverUI() { gameOverUI.SetActive(true); }
    public void HideGameOverUI() { gameOverUI.SetActive(false); }
    public void ShowPausePanelUI() { pausePanel.SetActive(true); }
    public void HidePausePanelUI() { pausePanel.SetActive(false); }
    public void ShowControlPanelUI() { controlPanel.SetActive(true); }
    public void HideControlPanelUI() { controlPanel.SetActive(false); }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Continue()
    {
        Time.timeScale = 1.0f;
        HidePausePanelUI(); 
    }

    void Pause()
    {
        Time.timeScale = 0f;
        ShowPausePanelUI();
        HideGameOverUI();
    }
}
