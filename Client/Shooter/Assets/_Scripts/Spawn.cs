using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    [SerializeField] private Transform [] _points;
    public Transform RandomPoint(){
        return _points[Random.Range(0,3)];
    }
}
