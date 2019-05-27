using System;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private float minimalSpawnDistance = 10f;
    [SerializeField] private Spawner[] spawners;

    [Serializable]
    public class Spawner
    {
        public GameObject mobPrefab;
        public float initialDelay = 0;
        public float delay = 4;
        public float delayDecreasement = 0.01f;
        public float deltaAngleDeg = 60;
        [HideInInspector] public float _time;
    }

    private void Start()
    {
        foreach (var spawner in spawners)
        {
            spawner._time = spawner.initialDelay;
        }
    }

    void FixedUpdate()
    {
        foreach (var spawner in spawners)
        {
            spawner._time -= Time.fixedDeltaTime;
            if (spawner._time < 0)
            {
                Vector3 pos = GetSpawnPoint(1);
                GameObject mob = Instantiate(spawner.mobPrefab, pos, Quaternion.identity,
                    GameManager.Instance.MobContainer);
                GameManager.Instance.AddEnemy(mob);
                Vector3 lookAt = transform.position;
                lookAt.y = mob.transform.position.y;
                mob.transform.LookAt(lookAt);
                float angle = Random.Range(-spawner.deltaAngleDeg, spawner.deltaAngleDeg);
                mob.transform.localEulerAngles += new Vector3(0, angle, 0);
                spawner._time = spawner.delay;
                spawner.delay -= spawner.delayDecreasement;
            }
        }
    }

    public Vector3 GetSpawnPoint(float halfExtents)
    {
        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(0.1f, 0.9f);
            float y = Random.Range(0.1f, 0.9f);
            Physics.Raycast(GameManager.Instance.Camera.ViewportPointToRay(new Vector3(x, y)), out var hit);
            if (hit.point.y > 0)
                continue;
            Vector3 pos = hit.point;
            pos.y = 0;
            if ((GameManager.Instance.Pointer.transform.position - pos).magnitude < minimalSpawnDistance)
                continue;
            if (!Physics.CheckBox(pos, halfExtents * Vector3.one))
                continue;
            return pos;
        }

        return Vector3.positiveInfinity;
    }
}