using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private Text scoreLabel;
    [SerializeField] private GameObject gameOverScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScore();
        gameOverScreen.GetComponent<CanvasGroup>().alpha=0;
    }

    public void UpdateScore()
    {
        scoreLabel.text = "Score: " + GameManager.Instance.Score;
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        StartCoroutine(_gameOver());
    }

    private IEnumerator _gameOver()
    {
        yield return new WaitForSeconds(1);
        gameOverScreen.GetComponent<CanvasGroup>().DOFade(1, 1);
    }
}