using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    private PlayerHealthSystem _healthSystem => GameManager.Instance.HealthSystem;
    private int _lastHealth;

    private float _fadeDuration = 0.5f;

    void Start()
    {
        for (int i = 0; i < _healthSystem.MaxHealth; i++)
            Instantiate(heartPrefab, transform).GetComponent<Image>().DOFade(0, _fadeDuration).From();
        _lastHealth = _healthSystem.CurrentHealth;
    }

    void Update()
    {
        for (int i = _lastHealth; i < _healthSystem.MaxHealth && i < _healthSystem.CurrentHealth; i++)
        {
            var el = transform.GetChild(i).gameObject.GetComponent<Image>();
            el.DOKill();
            el.DOFade(1, _fadeDuration);
            _lastHealth++;
        }

        for (int i = _lastHealth; i > 0 && i > _healthSystem.CurrentHealth;)
        {
            var el = transform.GetChild(--i).gameObject.GetComponent<Image>();
            el.DOKill();
            el.DOFade(0, _fadeDuration);
            _lastHealth--;
        }
    }
}