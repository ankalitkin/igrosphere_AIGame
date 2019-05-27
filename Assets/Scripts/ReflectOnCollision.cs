using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.GetComponent<Mob>() == null)
            return;
        Vector3 direction = Quaternion.AngleAxis(Random.Range(-10f,+10f), Vector3.up)*Vector3.Reflect(other.transform.forward, other.GetContact(0).normal);
        direction.y = 0;
        other.transform.forward = direction;
    }
}
