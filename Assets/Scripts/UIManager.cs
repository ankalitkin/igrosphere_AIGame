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
    }

    public void UpdateScore()
    {
        scoreLabel.text = "Score: " + GameManager.Instance.Score;
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        gameOverScreen.GetComponent<Image>().DOFade(0, 1).From();
        gameOverScreen.transform.GetChild(0).GetComponent<Text>().DOFade(0, 1).From();
    }
}