using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character {
    private string _sessionId;
    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;
    [SerializeField] private float _rotationSpeed = 20f;
    public Vector3 targetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude = 0;
    private Vector3 _localEulerAnglesX;
    private Vector3 _localEulerAnglesY;
    public void Init(string sessionId){
        _sessionId = sessionId;
    }
    private void Start() {
        targetPosition = transform.position;
    }
    private void Update() {
        if(_velocityMagnitude > .1f){
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistance);
        }else{
            transform.position = targetPosition;
        }
        _head.localRotation = Quaternion.Lerp(_head.localRotation, Quaternion.Euler(_localEulerAnglesX), _rotationSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(_localEulerAnglesY), _rotationSpeed * Time.deltaTime);
    }

    public void SetSpeed(float value) => Speed = value;
    public void SetMaxHP(int value){
        MaxHealth = value;
        _health.SetMax(value);
        _health.SetCurrent(value);
    } 
    public void SetSpeedSquat(float value) => SpeedSquating = value;

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval){
        targetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;

        Velocity = velocity;
    }
    public void ApplyDamage(int damage){
        _health.ApplyDamage(damage);

        Dictionary<string, object> data = new Dictionary<string, object>(){
            { "id", _sessionId },
            { "value", damage }
        };
        MultiplayerManager.Instance.SendMessage("damage", data);
    }

    public void SetRotateX(float value){
        _localEulerAnglesX = new Vector3(value, 0f, 0f);
    }
    public void SetRotateY(float value){
        _localEulerAnglesY = new Vector3(0f, value, 0f);
    }
}
