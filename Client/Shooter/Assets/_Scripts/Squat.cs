using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squat : MonoBehaviour{
    public bool IsSquating {get; private set;}
    [SerializeField] private Transform _body;
    private float _standartScale;
    private Character _character;
    private void Start() {
        _character = GetComponent<Character>();
        _standartScale = transform.localScale.z;
    }
    private void Update() {
        if(!IsSquating) BodyScale(_standartScale);
        else BodyScale(_standartScale/2f);
    }
    public void BodyScale(float value){
        _body.localScale = Vector3.MoveTowards(_body.localScale, new Vector3(_body.localScale.x, _body.localScale.y, value), Time.deltaTime * _character.SpeedSquating);
    }
    public void SetSquatState(bool value) => IsSquating = value;
}
