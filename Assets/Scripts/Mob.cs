using System;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private int speed = 4;
    [SerializeField] private int splitAngle = 60;
    private int _health;
    private int _state;

    [Serializable]
    public class State
    {
        public float scale;
        public int health;
        public bool split;
    }

    [SerializeField] private State[] states;

    private void FixedUpdate()
    {
        transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController characterController = other.GetComponent<CharacterController>();
        if (characterController != null)
            characterController.Kill();
        else
        {
            Hit();
        }
    }

    private void UpdateState()
    {
        transform.localScale = Vector3.one * states[_state].scale;
        _health = states[_state].health;
    }

    private void Start()
    {
        UpdateState();
    }

    private void Hit()
    {
        _health--;
        if (_health == 0)
        {
            GameManager.Instance.IncreaseScore(1);
            bool split = states[_state].split;
            if (++_state < states.Length)
            {
                UpdateState();
                if (split)
                {
                    GameObject mob = Instantiate(gameObject, transform.position, transform.rotation,
                        GameManager.Instance.MobContainer);
                    GameManager.Instance.AddEnemy(mob);
                    mob.GetComponent<Mob>()._state = _state;
                    transform.localEulerAngles += new Vector3(0, -splitAngle, 0);
                    mob.transform.localEulerAngles += new Vector3(0, splitAngle, 0);
                }
            }
            else
            {
                Destroy(gameObject);
            }

            GameManager.Instance.IncreaseScore(5);
        }
    }

    private static void Destroy(GameObject gameObject)
    {
        GameManager.Instance.RemoveEnemy(gameObject);
        GameObject.Destroy(gameObject);
    }
}