using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;

    private int _currentHealth;

    public int CurrentHealth => _currentHealth;

    public int MaxHealth => maxHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void RemoveLife()
    {
        _currentHealth--;
        if (_currentHealth == 0)
            GameManager.Instance.GameOver();
    }
}