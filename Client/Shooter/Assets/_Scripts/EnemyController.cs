using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

[RequireComponent(typeof(EnemyCharacter))]
public class EnemyController : MonoBehaviour {
    private EnemyCharacter _enemyCharacter;
    private Queue<float> _receiveTimeInterval = new Queue<float>();
    private float AverageInterval{
        get{
            float averageDelay = 0f;
            foreach (var delay in _receiveTimeInterval){
                averageDelay += delay;
            }
            return averageDelay / _receiveTimeInterval.Count;
        }
    } 
    private float _lastReceiveTime = 0f;

    private void Start() {
        _enemyCharacter = GetComponent<EnemyCharacter>();
    }

    private void SaveReceiveTime(float delay){
        _receiveTimeInterval.Enqueue(delay);
        if(_receiveTimeInterval.Count > 5) _receiveTimeInterval.Dequeue();
    }

    internal void OnChange(List<DataChange> changes) {
        SaveReceiveTime(Time.time -_lastReceiveTime);

        Vector3 position = _enemyCharacter.transform.position;
        Vector3 velocity = Vector3.zero;

        foreach (var dataChange in changes){
            switch(dataChange.Field){
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    velocity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;
                default:
                    Debug.Log("Не обрабатывается изменение поля " + dataChange.Field);
                    break;
            }
        }
        _enemyCharacter.SetMovement(position, velocity, AverageInterval);
        _lastReceiveTime = Time.time;
    }
}
