using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject mobPrefab;
    [SerializeField] private float delay = 4;
    [SerializeField] private float deltaAngleDeg = 60;
    private float time;

    void FixedUpdate()
    {
        time -= Time.fixedDeltaTime;
        if (time < 0)
        {
            float x = Random.Range(0.1f, 0.9f);
            float y = Random.Range(0.1f, 0.9f);
            Physics.Raycast(GameManager.Instance.Camera.ViewportPointToRay(new Vector3(x, y)), out var hit);
            Vector3 pos = hit.point;
            pos.z = 0;
            GameObject mob = Instantiate(mobPrefab, pos, Quaternion.identity);
            Vector3 lookAt = transform.position;
            lookAt.y = mob.transform.position.y;
            mob.transform.LookAt(lookAt);
            float angle = Random.Range(-deltaAngleDeg, deltaAngleDeg);
            mob.transform.localEulerAngles += new Vector3(0, angle, 0);
            time = delay;
        }
    }
}