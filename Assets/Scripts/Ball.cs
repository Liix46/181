using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject Arrow;
    private Vector3 _arrowStartPosition;
    private Quaternion _arrowStarRotation;


    private Rigidbody _rb;
    private Vector3 _forceDirection;
    private float _forceMagnitude = 1000;
    private Vector3 _startPosition;
    private bool _isBallReady;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _forceDirection.Set(0, 0, 2000);
        _startPosition = this.transform.position;
        _isBallReady = true;
        _arrowStartPosition = Arrow.transform.position;
        _arrowStarRotation = Arrow.transform.rotation;
    }

    [System.Obsolete]
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _isBallReady)
        {
            //_rb.AddForce(_forceDirection);
            Arrow.active = false;

            _rb.AddForce(_forceMagnitude * Arrow.transform.forward);
            _isBallReady = false;

        }

        if (_rb.velocity.magnitude < 1 && _rb.velocity.magnitude > 0.5)
        {
            Arrow.active = true;
            Arrow.transform.position = _arrowStartPosition;
            Arrow.transform.rotation = _arrowStarRotation;


            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            Debug.Log("Strike");
            //Debug.Log(_rb.velocity.magnitude);

            this.transform.position = _startPosition;
            _isBallReady = true;
        }
    }
}
