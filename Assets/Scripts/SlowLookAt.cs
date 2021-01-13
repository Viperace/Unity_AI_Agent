using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowLookAt : MonoBehaviour
{
    public float speed = 0.2f;

    [SerializeField] Vector3 targetPos;

    //[SerializeField]  bool _isDone = true;

    public Transform target;

    public void InitiateLookAt(Vector3 targetPos)
    {
        //_isDone = false;
        this.targetPos = targetPos;
    }
    public void InitiateLookAt(Transform target)
    {
        this.target = target;
    }
    public void StopLooking()
    {
        this.target = null;
    }

    // Update is called once per frame
    Vector3 _direction;
    Quaternion _toRotation;
    void Update()
    {
        /*
        if (!_isDone )
        {
            _direction = targetPos - transform.position;
            _toRotation = Quaternion.FromToRotation(transform.forward, _direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, _toRotation, speed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, _toRotation) <= 0.01f)
            {
                _isDone = true;
            }
        }
        */

        if (target)
        {
            _direction = target.position - transform.position;
            _toRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, _toRotation, speed * Time.deltaTime);
        }
    }
}
