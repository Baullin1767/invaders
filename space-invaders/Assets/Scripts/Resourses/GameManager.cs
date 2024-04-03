using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using System.Collections;

public sealed class GameManager : MonoBehaviour
{
    [Inject]
    Player player;
    [Inject]
    Invaders invaders;
    [Inject]
    MysteryShip mysteryShip;
    [Inject]
    UIController uiController;
    [Inject]
    GameConfig config;

    Bunker[] bunkers;

    int score;
    int lives;

    public int Score => score;
    public int Lives => lives;

#if UNITY_EDITOR
    [Inject]
    KeyboardInput keyboardInput;
#else
    [Inject]
    MobilInput mobilInput;

#endif


    private void Start()
    {
        bunkers = FindObjectsOfType<Bunker>();

        NewGame();
        bunkers = FindObjectsOfType<Bunker>();
#if UNITY_EDITOR
        Observable.EveryUpdate()
            .Where(_ => lives <= 0 && keyboardInput.GetButtonShoot())
            .Subscribe(_ => { NewGame(); });
#else
        Observable.EveryUpdate()
            .Where(_ => lives <= 0 && mobilInput.GetButtonShoot())
            .Subscribe(_ => { NewGame(); });
#endif
    }
    private void NewGame()
    {
        uiController.HideGameOverUI();
        uiController.HidePausePanelUI();
        Time.timeScale = 1.0f;
        SetScore(0);
        SetLives(config.lives);
        NewRound();
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        for (int i = 0; i < bunkers.Length; i++) {
            bunkers[i].ResetBunker();
        }

        Respawn();
    }

    private void Respawn()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        uiController.ShowGameOverUI();
        invaders.gameObject.SetActive(false);
    }

    public void SetScore(int score)
    {
        this.score = score;

        uiController.SetScore(score);
    }

    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        uiController.SetLives(lives);
    }

    public void OnPlayerKilled(Player player)
    {
        SetLives(lives - 1);

        player.gameObject.SetActive(false);

        if (lives > 0) {
            Invoke(nameof(NewRound), 1f);
        } else {
            GameOver();
        }
    }

    public void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);

        SetScore(score + invader.score);

        if (invaders.GetAliveCount() == 0) {
            NewRound();
        }
    }

    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(score + mysteryShip.score);
    }

    public void OnBoundaryReached()
    {
        if (invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);

            OnPlayerKilled(player);
        }
    }
}
