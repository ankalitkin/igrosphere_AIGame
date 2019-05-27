using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _aboutMenu;
    [SerializeField] private AudioSource _playButtonSound;
    [SerializeField] private AudioSource _otherButtonsSound;

    public void Play()
    {
        DontDestroyOnLoad(_playButtonSound);
        _playButtonSound.Play();
        SceneManager.LoadScene("GameScene");
    }

    public void About()
    {
        _mainMenu.SetActive(false);
        _aboutMenu.SetActive(true);
        _otherButtonsSound.Play();
    }

    public void Back()
    {
        _mainMenu.SetActive(true);
        _aboutMenu.SetActive(false);
        _otherButtonsSound.Play();
    }

    public void Exit()
    {
        _otherButtonsSound.Play();
        Application.Quit();
    }

    private void Update()
    {
        if (_otherButtonsSound.time > 0.04)
            _otherButtonsSound.Stop();
    }
}