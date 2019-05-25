﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class CharacterController : MonoBehaviour
{
    private const float Eps = 0.01f;
    [SerializeField, HideInInspector] private NavMeshAgent _agent;
    [SerializeField, HideInInspector] private ThirdPersonCharacter _character;
    [SerializeField, HideInInspector] private Renderer _renderer;
    [SerializeField, HideInInspector] private Animator _animator;
    private Vector3 _oldPos;
    private Vector3 _lookAt;
    private float _time;
    private bool _isDead;

    private void OnValidate()
    {
        _agent = GetComponent<NavMeshAgent>();
        _character = GetComponent<ThirdPersonCharacter>();
        _animator = GetComponent<Animator>();
        _renderer = transform.GetChild(0).GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        if(_isDead)
            return;
        _time -= Time.fixedDeltaTime;
        GameObject enemy = GameManager.Instance.GetClosestEnemy(transform.position);
        bool isAttacking = enemy != null
                           && (enemy.transform.position - transform.position).magnitude < GameManager.Instance.AttackDistance;
        if (_time < 0 && isAttacking)
        {            
            Vector3 bulletPos = transform.position;
            bulletPos.y += 1;
            FireballBullet bullet = Instantiate(GameManager.Instance.SelfDrivenBulletPrefab, bulletPos,
                Quaternion.identity).GetComponent<FireballBullet>();
            bullet.goTo = enemy;
            _time = GameManager.Instance.AttackDelay;
        }

        int i = transform.GetSiblingIndex();
        Vector3 pos = GameManager.Instance.GetCharacterPosition(i);
        if (_oldPos != pos)
        {
            _agent.SetDestination(pos);
            _oldPos = transform.position;
        }

        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            _character.Move(_agent.desiredVelocity, false, false);
        }
        else
        {
            _character.Move(Vector3.zero, false, false);
            Vector3 lookAt = isAttacking ? enemy.transform.position : GameManager.Instance.LookAt.transform.position;
            lookAt.y = transform.position.y;
            _lookAt = lookAt;
        }

        Vector3 direction = _lookAt - transform.position;
        Quaternion quaternion = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, GameManager.Instance.RotationSpeed * Time.fixedDeltaTime);
    }

    public void Kill()
    {
        if(_isDead)
            return;
        transform.parent = GameManager.Instance.DeadCharacters;
        _isDead = true;
        _character.enabled = false;
        _agent.enabled = false;
        _renderer.material.SetFloat("_Metallic", 1);
        _animator.SetBool("DeathTrigger", true);
        StartCoroutine(Reborn());
    }
    
    

    public IEnumerator Reborn()
    {
        yield return new WaitForSeconds(5);
        if(!_isDead)
            yield break;
        _animator.SetBool("DeathTrigger", false);
        Material mat = _renderer.material;
        DOTween.To(() => mat.GetFloat("_Metallic"), x => mat.SetFloat("_Metallic", x), 0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.parent = GameManager.Instance.Characters;
        _isDead = false;
        _character.enabled = true;
        _agent.enabled = true;        
    }
}