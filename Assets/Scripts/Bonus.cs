using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField, HideInInspector] private Renderer _renderer;
    [SerializeField, HideInInspector] private Collider _collider;
    private Material _mat => _renderer.material;
    private bool _active = true;

    private void OnValidate()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _mat.DOFade(0, 1).From();
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController ch = other.GetComponent<CharacterController>();
        if (_active && (ch == null || !ch.IsAlive))
            return;
        _active = false;
        Destroy(_collider);
        foreach (Transform character in GameManager.Instance.DeadCharacters)
        {
            StartCoroutine(character.GetComponent<CharacterController>().Destroy());
            GameObject newChar = Instantiate(GameManager.Instance.CharacterPrefab, transform.position,
                Quaternion.identity, GameManager.Instance.Characters);
            newChar.GetComponent<CharacterController>().FadeIn();
        }

        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        _mat.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        GameManager.Instance.BonusSpawner.BonusRemoved();
    }
}