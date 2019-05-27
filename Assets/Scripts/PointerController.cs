using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PointerController : MonoBehaviour
{
    [SerializeField, HideInInspector] private Camera _camera;
    private bool _lookLock = true;

    private Transform _pointer => GameManager.Instance.Pointer.transform;

    private Transform _lookAt => GameManager.Instance.LookAt.transform;

    private void OnValidate()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Time.timeScale < Mathf.Epsilon)
            return;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            _lookAt.position = hit.point;
            if (!_lookLock || Input.GetMouseButton(0))
            {
                if (_pointer != _lookAt)
                {
                    _pointer.LookAt(_lookAt);
                    if (GameManager.Instance.Characters.childCount > 0)
                    {
                        Vector3 rotation = _pointer.transform.eulerAngles;
                        rotation.y %= 360 / GameManager.Instance.Characters.childCount;
                        _pointer.transform.eulerAngles = rotation;
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                _pointer.position = _lookAt.position;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
            _lookLock = !_lookLock;
    }
}