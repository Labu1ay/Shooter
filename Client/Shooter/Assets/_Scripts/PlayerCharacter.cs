using System.Collections;
using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CheckFly))]
public class PlayerCharacter : Character {
    [SerializeField] private Health _health;
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
    private float _rotateX;
    private float _currentRotateX;
    private float _jumpTime;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _checkFly = GetComponent<CheckFly>();
    }
    private void Start() {
        CameraInit();

        _health.SetMax(MaxHealth);
        _health.SetCurrent(MaxHealth);
    }
    private void CameraInit(){
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
        
    }
    public void SetInput(float h, float v, float RotateY, float RotateX){
        _inputH = h;
        _inputV = v;
        _rotateY += RotateY;
        _rotateX = RotateX;
    }
    private void Update() {
        RotateX(_rotateX);
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
    internal void OnChange(List<DataChange> changes){
         foreach (var dataChange in changes){
            switch(dataChange.Field){
                case "loss":
                MultiplayerManager.Instance.LossCounter.SetPlayerLoss((byte)dataChange.Value);
                    break;
                case "currentHP":
                    _health.SetCurrent((sbyte)dataChange.Value);
                    break;
                default:
                    Debug.Log("Не обрабатывается изменение поля " + dataChange.Field);
                    break;
            }
        }
    }
}
