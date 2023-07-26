using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    Vector3 Temp ;
    Vector3 previousPosition;
    internal void OnChange(List<DataChange> changes) {
        Vector3 position = transform.position;
        // Vector3 previousPosition = transform.position;
        foreach (var dataChange in changes){
            switch(dataChange.Field){
                case "x":
                    previousPosition.x = (float)dataChange.PreviousValue;
                    position.x = (float)dataChange.Value;
                    break;
                case "y":
                    previousPosition.z = (float)dataChange.PreviousValue;
                    position.z = (float)dataChange.Value;
                    break;
                default:
                    Debug.Log("Не обрабатывается изменение поля " + dataChange.Field);
                    break;
            }
        }
       
        //transform.position = position;
        Temp = Vector3.LerpUnclamped(previousPosition, position, 2f);
        previousPosition = transform.position;
       // transform.position = Temp;
    }
    private void Update() {
        transform.position = Vector3.Lerp(transform.position, Temp, Time.deltaTime * 10f);
    }
}
