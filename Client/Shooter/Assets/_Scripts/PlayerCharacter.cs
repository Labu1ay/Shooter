using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacter : MonoBehaviour {
    private Rigidbody _rigidbody;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _maxHeadAngle = 90f;
    [SerializeField] private float _minHeadAngle = -90f;
    [SerializeField] private float _jumpForce = 5f;
    private float _inputH;
    private float _inputV;
    private float _rotateY;
    private float _currentRotateX;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody>();
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

        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH) * _speed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
    }

    private void RotateY(){
        _rigidbody.angularVelocity = new Vector3(0f, _rotateY, 0f);
        _rotateY = 0;
    }

    public void RotateX(float value){
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0f, 0f);

    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity){
        position = transform.position;
        velocity = _rigidbody.velocity;
    } 

    private bool _isFly = true;
    private void OnCollisionStay(Collision other) {
        var contactPoints = other.contacts;
        for (int i = 0; i < contactPoints.Length; i++){
            if(contactPoints[i].normal.y > .45f) _isFly = false;
        }
    }

    private void OnCollisionExit(Collision other) {
        _isFly = true;
    }
    public void Jump(){
        if(_isFly) return;
        _rigidbody.AddForce(0f, _jumpForce, 0f, ForceMode.VelocityChange);
    }
}
