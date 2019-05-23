using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PointerController : MonoBehaviour
{
    [SerializeField, HideInInspector] private Camera _camera;
    private GameObject _oldTarget;

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
            bool isStatic = hit.transform.gameObject.isStatic;
            if (isStatic)
                _lookAt.position = hit.point;
            if (Input.GetMouseButton(0) && isStatic || Input.GetMouseButtonDown(0) && hit.transform.gameObject == _oldTarget)
                _pointer.position = _lookAt.position;
        }

        if (_pointer != _lookAt)
            _pointer.LookAt(_lookAt);
        _oldTarget = GameManager.Instance.target;
    }
}