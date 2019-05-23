using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PointerController : MonoBehaviour
{
    [SerializeField, HideInInspector] private Camera _camera;

    private Transform _pointer => GameManager.Instance.Pointer.transform;

    private Transform _lookAt => GameManager.Instance.LookAt.transform;

    private void OnValidate()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _lookAt.position = hit.point;
            if (Input.GetMouseButton(0))
            {
                if (_pointer != _lookAt)
                    _pointer.LookAt(_lookAt);
                _pointer.position = _lookAt.position;
            }
        }
    }
}