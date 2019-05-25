using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject mobPrefab;
    [SerializeField] private float delay = 4;
    [SerializeField] private float deltaAngleDeg = 60;
    private float _time;

    void FixedUpdate()
    {
        _time -= Time.fixedDeltaTime;
        if (_time < 0)
        {
            float x = Random.Range(0.1f, 0.9f);
            float y = Random.Range(0.1f, 0.9f);
            Physics.Raycast(GameManager.Instance.Camera.ViewportPointToRay(new Vector3(x, y)), out var hit);
            Vector3 pos = hit.point;
            pos.y = 0;
            GameObject mob = Instantiate(mobPrefab, pos, Quaternion.identity, GameManager.Instance.MobContainer);
            GameManager.Instance.AddEnemy(mob);
            Vector3 lookAt = transform.position;
            lookAt.y = mob.transform.position.y;
            mob.transform.LookAt(lookAt);
            float angle = Random.Range(-deltaAngleDeg, deltaAngleDeg);
            mob.transform.localEulerAngles += new Vector3(0, angle, 0);
            _time = delay;
        }
    }
}