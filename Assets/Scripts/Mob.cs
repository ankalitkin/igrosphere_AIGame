using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color targetColor = Color.yellow;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * Time.fixedDeltaTime * GameManager.Instance.MobSpeed;
    }

    private void Update()
    {
        if (gameObject == GameManager.Instance.target)
            _renderer.material.color = targetColor;
        else
            _renderer.material.color = defaultColor;
    }

    private void OnMouseDown()
    {
        GameManager.Instance.target = gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject.Destroy(other.gameObject);
    }

    public static void Destroy(GameObject gameObject)
    {
        GameObject.Destroy(gameObject.GetComponent<MobHealthSystem>().HpBar.gameObject);
        GameObject.Destroy(gameObject);
    }
}