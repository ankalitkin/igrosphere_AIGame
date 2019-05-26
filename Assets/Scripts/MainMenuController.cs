using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _aboutMenu;

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void About()
    {
        _mainMenu.SetActive(false);
        _aboutMenu.SetActive(true);
    }

    public void Back()
    {
        _mainMenu.SetActive(true);
        _aboutMenu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
