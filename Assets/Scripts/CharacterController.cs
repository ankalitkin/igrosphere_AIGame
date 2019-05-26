using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private Vector3 _lookAtTmp;
    private float _time;
    private bool _isDead;
    private static bool _crouch => Input.GetKey(KeyCode.LeftShift);

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
        ResetLookAtTmp();
        ProcessAutoAttack();
        ProcessManualAttack();
        ProcessCharacterMovement();
        ProcessCharacterRotation();
    }

    private void ResetLookAtTmp()
    {
        _lookAtTmp = GameManager.Instance.LookAt.transform.position;
    }

    private void ProcessCharacterRotation()
    {
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
            _lookAtTmp.y = transform.position.y;
            _lookAt = _lookAtTmp;
        }
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
            _time = GameManager.Instance.AutoAttackDelay;
            _lookAtTmp = enemy.transform.position;
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
            _time = GameManager.Instance.AttackDelay;
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
        //StartCoroutine(Reborn());
    }

    public IEnumerator Reborn()
    {
        yield return new WaitForSeconds(5);
        if (!_isDead)
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