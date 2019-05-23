using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerHealthSystem))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject lookAt;
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject selfDrivenBulletPrefab;
    [SerializeField] private PointerController pointerController;
    [SerializeField] private Transform characters;
    [SerializeField] private float rotationSpeed = 6;
    [SerializeField] private float radius = 1;
    [SerializeField] private float mobSpeed = 2;
    [SerializeField] private float attackDelay = .5f;
    [SerializeField] private float bulletSpeed = 5;
    [SerializeField] private float damage = .2f;
    [SerializeField] private float attackDistance = 5f;
    public static GameManager Instance;
    private bool _gameActive = true;
    private int _score;
    private List<GameObject> _enemies;


    [SerializeField, HideInInspector] private PlayerHealthSystem _healthSystem;
    public PlayerHealthSystem HealthSystem => _healthSystem;

    public GameObject Pointer => pointer;

    public Camera Camera => camera;

    public GameObject LookAt => lookAt;

    public float RotationSpeed => rotationSpeed;

    public float MobSpeed => mobSpeed;

    public float Damage => damage;

    public float AttackDistance => attackDistance;

    public float AttackDelay => attackDelay;

    public float BulletSpeed => bulletSpeed;

    public GameObject SelfDrivenBulletPrefab => selfDrivenBulletPrefab;

    public GameObject Floor => floor;

    public int Score => _score;

    private void OnEnable()
    {
        Instance = this;
        _enemies = new List<GameObject>();
        _healthSystem = GetComponent<PlayerHealthSystem>();
    }

    public Vector3 GetCharacterPosition(int i)
    {
        if (i == 0)
            return pointer.transform.position;
        float angle = characters.childCount > 2 ? -Mathf.PI * (i - 1) / (characters.childCount - 2) : -Mathf.PI / 2;
        Vector3 shift = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        shift = pointer.transform.localToWorldMatrix * shift;
        return shift * radius + pointer.transform.position;
    }

    private void Update()
    {
        if (_gameActive && characters.childCount == 0)
            GameOver();
        if (!_gameActive && Input.anyKey)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddEnemy(GameObject enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
    }

    public GameObject GetClosestEnemy(Vector3 position)
    {
        GameObject closest = null;
        float lastDist = -1;
        foreach (var obj in _enemies)
        {
            float dist = (obj.transform.position - position).sqrMagnitude;
            if (lastDist < 0 || dist < lastDist)
            {
                lastDist = dist;
                closest = obj;
            }
        }

        return closest;
    }
    
    public void GameOver()
    {
        if (!_gameActive)
            return;
        _gameActive = false;
        StartCoroutine(_GameOver());
    }

    private IEnumerator _GameOver()
    {
        yield return new WaitForSeconds(2);
        spawner.SetActive(false);
        UIManager.Instance.GameOver();
    }

    public void IncrementScore()
    {
        _score++;
        UIManager.Instance.UpdateScore();
    }
}