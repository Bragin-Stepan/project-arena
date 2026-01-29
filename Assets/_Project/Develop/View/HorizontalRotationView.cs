using System;
using System.Collections;
using UnityEngine;

public class HorizontalRotationView: MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isPlayAwake;

    private void Awake()
    {
        if (_isPlayAwake)
            Play();
    }

    private IEnumerator Process()
    {
        while (true)
        {
            _target.Rotate(Vector3.up * (_speed * Time.deltaTime));
            yield return null;
        }
    }

    public void Play()
    {
        StartCoroutine(Process());
    }
    
    public void Stop ()
    {
        StopCoroutine(Process());
    }
}
