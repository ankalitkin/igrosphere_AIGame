﻿using UnityEngine;

public class FireballBullet : MonoBehaviour
{
    [HideInInspector] public Vector3 goToVect;
    [HideInInspector] public GameObject goToObj;
    [SerializeField, HideInInspector] private Rigidbody _rigidbody;
    private float _duration;
    private Vector3 _oldPos;
    private Vector3 _oldGoToPos;
    private float _time;
    private bool _auto;
    
    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (goToObj != null)
        {
            _auto = true;
            _oldPos = transform.position;
            _oldGoToPos = goToObj.transform.position;
            _duration = (transform.position - goToObj.transform.position).magnitude / GameManager.Instance.BulletSpeed;
        }
        else
        {
            _auto = false;
            transform.LookAt(goToVect);
            _rigidbody.velocity = transform.forward * GameManager.Instance.BulletSpeed;
            _time = GameManager.Instance.AttackDistance / GameManager.Instance.BulletSpeed;
        }
    }

    void FixedUpdate()
    {
        if (_auto)
        {
            if(goToObj != null)
                _oldGoToPos = goToObj.transform.position;
            if (_time < _duration)
            {
                _time += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(_oldPos, _oldGoToPos, _time / _duration);
            }
            else
                Destroy(gameObject);
        }
        else
        {
            _time -= Time.deltaTime;
            if(_time < 0)
                Destroy(gameObject);
        }
    }
}