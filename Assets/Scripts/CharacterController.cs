using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
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
    public bool IsAlive => !_isDead;
    private static bool _crouch => Input.GetKey(KeyCode.LeftShift);
    private Material _mat => _renderer.material;

    private float _attackDelay =>
        Input.GetMouseButton(1) ? GameManager.Instance.AttackDelay : GameManager.Instance.AutoAttackDelay;

    private void OnValidate()
    {
        _agent = GetComponent<NavMeshAgent>();
        _character = GetComponent<ThirdPersonCharacter>();
        _animator = GetComponent<Animator>();
        _renderer = transform.GetChild(0).GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        if (_isDead)
            return;
        _time -= Time.fixedDeltaTime;
        ProcessAutoAttack();
        ProcessManualAttack();
        ProcessCharacterMovement();
        ProcessCharacterRotation();
    }

    private void ProcessAutoAttack()
    {
        GameObject enemy = GameManager.Instance.GetClosestEnemy(transform.position);
        if (_time < 0 && enemy != null
                      && (enemy.transform.position - transform.position).magnitude <
                      GameManager.Instance.AutoAttackDistance)
        {
            Vector3 bulletPos = transform.position;
            bulletPos.y += 1;
            FireballBullet bullet = Instantiate(GameManager.Instance.SelfDrivenBulletPrefab, bulletPos,
                Quaternion.identity).GetComponent<FireballBullet>();
            bullet.goToObj = enemy;
            _time = _attackDelay;
        }
    }

    private void ProcessManualAttack()
    {
        if (_time < 0 && Input.GetMouseButton(1))
        {
            Vector3 bulletPos = transform.position;
            bulletPos.y += 1;
            FireballBullet bullet = Instantiate(GameManager.Instance.SelfDrivenBulletPrefab, bulletPos,
                Quaternion.identity).GetComponent<FireballBullet>();

            Vector3 goTo = GameManager.Instance.LookAt.transform.position;
            goTo.y = bulletPos.y;
            bullet.goToVect = goTo;
            _time = _attackDelay;
        }
    }

    private void ProcessCharacterRotation()
    {
        if (_agent.remainingDistance > _agent.stoppingDistance || Input.anyKey)
        {
            Vector3 lookAt = GameManager.Instance.LookAt.transform.position;
            lookAt.y = transform.position.y;
            _lookAt = lookAt;
        }

        if (transform.position == _lookAt) return;
        Vector3 direction = _lookAt - transform.position;
        Quaternion quaternion = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, quaternion,
            GameManager.Instance.RotationSpeed * Time.fixedDeltaTime);
    }

    private void ProcessCharacterMovement()
    {
        int i = transform.GetSiblingIndex();
        Vector3 pos = GameManager.Instance.GetCharacterPosition(i);
        if (_oldPos != pos)
        {
            _agent.SetDestination(pos);
            _oldPos = transform.position;
        }

        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            _character.Move(_agent.desiredVelocity, _crouch, false);
        }
        else
        {
            _character.Move(Vector3.zero, _crouch, false);
        }
    }

    public void Kill()
    {
        if (_isDead)
            return;
        transform.parent = GameManager.Instance.DeadCharacters;
        _isDead = true;
        _character.enabled = false;
        _agent.enabled = false;
        _renderer.material.SetFloat("_Metallic", 1);
        _animator.SetBool("Crouch", false);
        _animator.SetBool("DeathTrigger", true);
    }

    public IEnumerator Destroy()
    {
        _mat.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void FadeIn()
    {
        _mat.DOFade(0, 1).From();
    }
}