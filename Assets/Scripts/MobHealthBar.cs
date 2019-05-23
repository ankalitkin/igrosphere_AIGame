using UnityEngine;

public class MobHealthBar : MonoBehaviour
{
    public MobHealthSystem Parent { get; set; }
    private Camera _camera => GameManager.Instance.Camera;

    void Update()
    {
        ProcessPosition();
    }

    public void ProcessPosition()
    {
        Vector3 pos = _camera.WorldToScreenPoint(Parent.HealthBarPoint);
        pos.z = -(_camera.transform.position - Parent.HealthBarPoint).magnitude;
        transform.position = pos + UIManager.Instance.HealthScreenOffset;
        transform.GetChild(0).localScale = new Vector3(Parent.Health, 1, 1);
    }
}