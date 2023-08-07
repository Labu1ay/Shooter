using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

[RequireComponent(typeof(EnemyCharacter))]
public class EnemyController : MonoBehaviour {
    private EnemyCharacter _enemyCharacter;
    private Squat _squat;
    [SerializeField] private EnemyGun _enemyGun;
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
    private Player _player;
    private void Awake() {
        _enemyCharacter = GetComponent<EnemyCharacter>();
        _squat = GetComponent<Squat>();
    }
    public void Init(string key, Player player){
        _enemyCharacter.Init(key);

        _player = player;
        _enemyCharacter.SetMaxHP(player.maxHP);
        _enemyCharacter.SetSpeed(player.speed);
        _enemyCharacter.SetSpeedSquat(_player.spSqt);
        player.OnChange += OnChange;
    }

    public void Shoot(in ShootInfo info){
        Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
        Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);

        _enemyGun.Shoot(position, velocity);
    }

    public void Destroy(){
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }
    
    private void SaveReceiveTime(float delay){
        _receiveTimeInterval.Enqueue(delay);
        if(_receiveTimeInterval.Count > 5) _receiveTimeInterval.Dequeue();
    }

    internal void OnChange(List<DataChange> changes) {
        SaveReceiveTime(Time.time -_lastReceiveTime);

        Vector3 position = _enemyCharacter.transform.position;
        Vector3 velocity = _enemyCharacter.Velocity;

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
                case "rX":
                    _enemyCharacter.SetRotateX((float)dataChange.Value);
                    break;
                case "rY":
                    _enemyCharacter.SetRotateY((float)dataChange.Value);
                    break;
                case "sq":
                    _squat.SetSquatState((bool)dataChange.Value);
                    Debug.Log((bool)dataChange.Value);
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
