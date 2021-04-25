using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private float _speed = 2f;

    private Animator _anim;

    public static CameraMovement instance;

    void Awake()
    {
        instance = this;
        _anim = transform.GetChild(0).GetComponent<Animator>();
        transform.position = _target.transform.position;

    }
    private void OnDestroy()
    {

    }

    public void ChangeTarget(GameObject target)
    {
        _target = target;
    }
    public void Shake()
    {
        _anim.SetTrigger("Shake");
    }

    void LateUpdate()
    {
        if (_target == null) return;
        Vector3 pos = Vector3.Lerp(transform.position, _target.transform.position, Time.deltaTime * _speed);
        pos = new Vector3(pos.x, pos.y, -10);
        transform.position = pos;
    }
}
