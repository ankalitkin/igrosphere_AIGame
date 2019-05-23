using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FireballBullet : MonoBehaviour
{
    [HideInInspector] public GameObject goTo;
    private float _duration;
    private Vector3 _oldPos;
    private Vector3 _oldgoToPos;
    private float _time;

    private void Start()
    {
        _oldPos = transform.position;
        _oldgoToPos = goTo.transform.position;
        _duration = (transform.position - goTo.transform.position).magnitude / GameManager.Instance.BulletSpeed;
    }

    void FixedUpdate()
    {
        if (goTo != null)
            _oldgoToPos = goTo.transform.position;
        if (_time < _duration)
        {
            _time += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(_oldPos, _oldgoToPos, _time / _duration);
        }
        else
        {
            Destroy(gameObject);
            if (goTo != null)
                goTo.GetComponent<MobHealthSystem>().Hit();
        }
    }
}