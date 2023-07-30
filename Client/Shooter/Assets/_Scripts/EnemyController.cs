using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    private Vector3 _position;
    private Vector3 _previousPosition;
    private Vector3 _newPosition;
    private Queue<float> _previousPings  = new Queue<float>();
    private float _time;
    private void Start() {
        _position = transform.position;
        _previousPosition = transform.position;
        _newPosition = transform.position;
    }
    internal void OnChange(List<DataChange> changes) {
        foreach (var dataChange in changes){
            switch(dataChange.Field){
                case "x":
                    _previousPosition.x = (float)dataChange.PreviousValue;
                    _position.x = (float)dataChange.Value;
                    break;
                case "y":
                    _previousPosition.z = (float)dataChange.PreviousValue;
                    _position.z = (float)dataChange.Value;
                    break;
                default:
                    Debug.Log("Не обрабатывается изменение поля " + dataChange.Field);
                    break;
            }
        }
        AddPing(Time.time - _time);
        float speed = Vector3.Distance(_position, _previousPosition) / (Time.time - _time);
        float distanceToNewPosition = speed * GetAveragePing();
        
        _newPosition = Vector3.LerpUnclamped(_previousPosition, _position, 1 + distanceToNewPosition);
        _time = Time.time; 
    }
    private void Update() {
            transform.position = Vector3.Lerp(transform.position, _newPosition, 15f * Time.deltaTime);  
    }
    private void AddPing(float ping){
        _previousPings.Enqueue(ping);
        if(_previousPings.Count > 5) _previousPings.Dequeue();
    }
    private float GetAveragePing(){
        float averagePing = 0f;
        foreach (var ping in _previousPings){
            averagePing += ping;
        }
        return averagePing;
    } 
}
