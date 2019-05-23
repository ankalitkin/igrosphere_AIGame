using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CharacterController : MonoBehaviour
{
    private const float Eps = 0.01f;
    [SerializeField, HideInInspector] private NavMeshAgent _agent;
    [SerializeField, HideInInspector] private ThirdPersonCharacter _character;
    private Vector3 _oldPos;
    private float _time;

    private void OnValidate()
    {
        _agent = GetComponent<NavMeshAgent>();
        _character = GetComponent<ThirdPersonCharacter>();
    }

    private void FixedUpdate()
    {
        _time -= Time.fixedDeltaTime;
        if (_time < 0 && GameManager.Instance.target != null
                      && (GameManager.Instance.target.transform.position - transform.position).magnitude <
                      GameManager.Instance.AttackDistance)
        {
            Vector3 bulletPos = transform.position;
            bulletPos.y += 1;
            FireballBullet bullet = Instantiate(GameManager.Instance.SelfDrivenBulletPrefab, bulletPos,
                Quaternion.identity).GetComponent<FireballBullet>();
            bullet.goTo = GameManager.Instance.target;
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
            var lookAt = GameManager.Instance.LookAt.transform.position;
            lookAt.y = transform.position.y;
            transform.LookAt(lookAt);
        }
    }
}