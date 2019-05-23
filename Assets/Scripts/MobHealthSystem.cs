using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class MobHealthSystem : MonoBehaviour
{
    public Vector3 HealthBarPoint => transform.position + UIManager.Instance.HealthBarOffset;
    public float Health => _health;
    private MobHealthBar _hpBar;
    private float _health = 1;

    public MobHealthBar HpBar => _hpBar;

    private void Awake()
    {
        _hpBar = Instantiate(UIManager.Instance.HealthBarPrefab, UIManager.Instance.HealthBarContainer.transform)
            .GetComponent<MobHealthBar>();
        _hpBar.Parent = this;
        _hpBar.ProcessPosition();
    }

    public void Hit()
    {
        _health -= GameManager.Instance.Damage;
        if (_health <= Mathf.Epsilon)
        {
            Mob.Destroy(gameObject);
            GameManager.Instance.IncrementScore();
        }
    }

    private void Update()
    {
        Vector2 pos = GameManager.Instance.Camera.WorldToViewportPoint(transform.position);
        if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1)
        {
            Mob.Destroy(gameObject);
            GameManager.Instance.HealthSystem.RemoveLife();
        }
    }
}