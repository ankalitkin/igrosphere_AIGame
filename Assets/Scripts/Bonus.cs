using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField, HideInInspector] private Renderer _renderer;
    [SerializeField, HideInInspector] private Collider _collider;
    private Material _mat => _renderer.material;

    private void OnValidate()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        Material mat = _renderer.material;
        DOTween.To(() => mat.color.a, x => Utils.ChangeAlpha(mat, x), 0, 1).From();
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(_collider);
        foreach (Transform character in GameManager.Instance.DeadCharacters)
        {
            StartCoroutine(character.GetComponent<CharacterController>().Destroy());
            GameObject newChar = Instantiate(GameManager.Instance.CharacterPrefab, transform.position, Quaternion.identity, GameManager.Instance.Characters);
            newChar.GetComponent<CharacterController>().FadeIn();
        }  
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        DOTween.To(() => _mat.color.a, x => Utils.ChangeAlpha(_mat, x), 0, 1);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        GameManager.Instance.BonusSpawner.BonusRemoved();
    }
}