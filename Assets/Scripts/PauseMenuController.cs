using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField, HideInInspector] private CanvasGroup _canvasGroup;
    [SerializeField, HideInInspector] private AudioSource _buttonsSound;
    private float _oldTimeScale;
    private float duration = 1;

    private void OnValidate()
    {
        _canvasGroup = pauseMenu.GetComponent<CanvasGroup>();
        _buttonsSound = GetComponent<AudioSource>();
    }

    public void ShowHide()
    {
        if (!pauseMenu.activeSelf)
        {
            _oldTimeScale = Time.timeScale;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = _oldTimeScale;
            pauseMenu.SetActive(false);
        }
        _buttonsSound.Play();
    }

    public void Restart()
    {
        Time.timeScale = _oldTimeScale;
        _buttonsSound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = _oldTimeScale;
        _buttonsSound.Play();
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        if (GameManager.Instance.GameActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                ShowHide();
            if(_buttonsSound.time > 0.04)
                _buttonsSound.Stop();
        }
    }
}
