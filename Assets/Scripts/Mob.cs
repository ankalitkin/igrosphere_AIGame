using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Mob : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.position += transform.forward * Time.fixedDeltaTime * GameManager.Instance.MobSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //GameObject.Destroy(other.gameObject);
        other.GetComponent<CharacterController>().Kill();
    }

    public static void Destroy(GameObject gameObject)
    {
        GameManager.Instance.RemoveEnemy(gameObject);
        GameObject.Destroy(gameObject.GetComponent<MobHealthSystem>().HpBar.gameObject);
        GameObject.Destroy(gameObject);
    }
}