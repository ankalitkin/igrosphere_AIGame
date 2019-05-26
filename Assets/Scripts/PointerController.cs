using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PointerController : MonoBehaviour
{
    [SerializeField, HideInInspector] private Camera _camera;
    [SerializeField] private float sensitivity = .1f;
    private bool _lookLock = true;

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
            if (!_lookLock || Input.GetMouseButton(0))
            {
                if (_pointer != _lookAt)
                    _pointer.LookAt(_lookAt);
            }
            if (Input.GetMouseButton(0))
            {
                _pointer.position = _lookAt.position;
            }
        }
        _pointer.position += Input.GetAxis("Horizontal") * sensitivity * Vector3.right;
        _pointer.position += Input.GetAxis("Vertical") * sensitivity * Vector3.forward;
        if (Input.GetKeyDown(KeyCode.LeftControl))
            _lookLock = !_lookLock;
    }
}