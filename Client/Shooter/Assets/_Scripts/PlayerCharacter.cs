using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CheckFly))]
public class PlayerCharacter : Character {
    private Rigidbody _rigidbody;
    private CheckFly _checkFly;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _maxHeadAngle = 90f;
    [SerializeField] private float _minHeadAngle = -90f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _jumpDelay = 0.2f;
    private float _inputH;
    private float _inputV;
    private float _rotateY;
    private float _currentRotateX;
    private float _jumpTime;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _checkFly = GetComponent<CheckFly>();
    }
    private void Start() {
        CameraInitialization();
    }
    private void CameraInitialization(){
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
        
    }
    public void SetInput(float h, float v, float RotateY){
        _inputH = h;
        _inputV = v;
        _rotateY += RotateY;
    }
    
    private void FixedUpdate() {
        Move();
        RotateY();
    }
    private void Move(){
        // Vector3 direction = new Vector3(_inputH, 0, _inputV).normalized;
        // transform.position += direction * Time.deltaTime * _speed;

        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH) * Speed;
        velocity.y = _rigidbody.velocity.y;
        Velocity = velocity;
        _rigidbody.velocity = Velocity;
    }

    private void RotateY(){
        _rigidbody.angularVelocity = new Vector3(0f, _rotateY, 0f);
        _rotateY = 0;
    }

    public void RotateX(float value){
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0f, 0f);

    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY){
        position = transform.position;
        velocity = _rigidbody.velocity;

        rotateX = _head.localEulerAngles.x;
        rotateY = transform.eulerAngles.y;
    } 

    public void Jump(){
        if(_checkFly.IsFly) return;
        if(Time.time - _jumpTime < _jumpDelay) return;
        _jumpTime = Time.time;
        _rigidbody.AddForce(0f, _jumpForce, 0f, ForceMode.VelocityChange);
    }
}
