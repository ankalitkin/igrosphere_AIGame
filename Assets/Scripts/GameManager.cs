using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerHealthSystem))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject lookAt;
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject selfDrivenBulletPrefab;
    [SerializeField] private PointerController pointerController;
    [SerializeField] private Transform characters;
    [SerializeField] private float radius = 1;
    [SerializeField] private float mobSpeed = 2;
    [SerializeField] private float attackDelay = .5f;
    [SerializeField] private float bulletSpeed = 5;
    [SerializeField] private float damage = .2f;
    [SerializeField] private float attackDistance = 5f;
    public static GameManager Instance;
    [HideInInspector] public GameObject target;
    private bool _gameActive = true;
    private int score;

    private int n => characters.childCount;

    [SerializeField, HideInInspector] private PlayerHealthSystem _healthSystem;
    public PlayerHealthSystem HealthSystem => _healthSystem;

    public GameObject Pointer => pointer;

    public Camera Camera => camera;

    public GameObject LookAt => lookAt;

    public float MobSpeed => mobSpeed;

    public float Damage => damage;

    public float AttackDistance => attackDistance;

    public float AttackDelay => attackDelay;

    public float BulletSpeed => bulletSpeed;

    public PointerController PointerController => pointerController;

    public GameObject SelfDrivenBulletPrefab => selfDrivenBulletPrefab;

    public int Score => score;

    private void OnEnable()
    {
        Instance = this;
        _healthSystem = GetComponent<PlayerHealthSystem>();
    }

    public Vector3 GetCharacterPosition(int i)
    {
        if (i == 0)
            return pointer.transform.position;
        float angle = n > 2 ? -Mathf.PI * (i - 1) / (n - 2) : -Mathf.PI / 2;
        Vector3 shift = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        shift = pointer.transform.localToWorldMatrix * shift;
        return shift * radius + pointer.transform.position;
    }

    private void Update()
    {
        if (_gameActive && n == 0)
            GameOver();
        if (!_gameActive && Input.anyKey)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        if (!_gameActive)
            return;
        spawner.SetActive(false);
        UIManager.Instance.GameOverScreen.SetActive(true);
        UIManager.Instance.GameOverScreen.GetComponent<Image>().DOFade(0, 1).From();
        UIManager.Instance.GameOverScreen.transform.GetChild(0).GetComponent<Text>().DOFade(0, 1).From();
        _gameActive = false;
    }

    public void IncrementScore()
    {
        score++;
        UIManager.Instance.UpdateScore();
    }
}