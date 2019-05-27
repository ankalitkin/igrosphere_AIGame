using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float x1 = 0;
    [SerializeField] private float x2 = 1;
    [SerializeField] private float y1 = 0;
    [SerializeField] private float y2 = 1;
    private Camera Camera => GameManager.Instance.Camera;

    void Start()
    {
        Vector3 p1 = GetPos(x1, y1);
        Vector3 p2 = GetPos(x2, y1);
        Vector3 p3 = GetPos(x1, y2);
        Vector3 p4 = GetPos(x2, y2);
        Vector3 c1 = (p1 + p2) / 2;
        Vector3 c2 = (p3 + p4) / 2;
        Vector3 c3 = (p1 + p3) / 2;
        Vector3 c4 = (p2 + p4) / 2;
        float w1 = p2.x - p1.x + 1;
        float w2 = p4.x - p3.x + 1;
        float w3 = p3.z - p1.z + 1;
        float w4 = p4.z - p2.z + 1;
        AddWall(c1, p1, w1);
        AddWall(c2, p4, w2);
        AddWall(c3, p3, w3);
        AddWall(c4, p2, w4);
    }

    private void AddWall(Vector3 center, Vector3 corner, float length)
    {
        Instantiate(wallPrefab, center, Quaternion.LookRotation(center - corner), transform).transform.localScale =
            new Vector3(1, 1, length);
    }

    private Vector3 GetPos(float x, float y)
    {
        Ray ray = Camera.ViewportPointToRay(new Vector3(x, y));
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        Vector3 point = hit.point;
        point.y = 0;
        return point;
    }
}