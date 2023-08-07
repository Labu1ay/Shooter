using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character {
    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;
    public Vector3 targetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude = 0;
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
    }

    public void SetSpeed(float value) => Speed = value;
    public void SetMaxHP(int value){
        MaxHealth = value;
        _health.SetMax(value);
        _health.SetCurrent(value);
    } 

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval){
        targetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;

        Velocity = velocity;
    }
    public void ApplyDamage(int damage){
        _health.ApplyDamage(damage);
    }

    public void SetRotateX(float value){
        _head.localEulerAngles = new Vector3(value, 0f,0f);
    }
    public void SetRotateY(float value){
        transform.localEulerAngles = new Vector3(0f, value, 0f);
    }
}
