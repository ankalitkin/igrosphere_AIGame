using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StereoSound : MonoBehaviour
{
    [SerializeField, HideInInspector] private AudioSource _audioSource;

    private void OnValidate()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioSource.panStereo = GameManager.Instance.Camera.WorldToViewportPoint(transform.position).x * 2 - 1;
    }
}
