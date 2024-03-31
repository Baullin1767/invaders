using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIController : MonoBehaviour
{
    private GameObject gameOverUI;
    private Text scoreText;
    private Text livesText;

    private void Start()
    {
        gameOverUI = transform.Find("GameOver").gameObject;
        livesText = transform.Find("Lives").GetComponent<Text>();
        scoreText = transform.Find("Score").GetComponent<Text>();
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
}
