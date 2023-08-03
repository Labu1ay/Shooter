using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour {
    private const string shoot = "Shoot";
    [SerializeField] private Animator _gunAnimator;
    [SerializeField] private Gun _gun;
    private void Start() {
        _gun.shoot += Shoot;
    }
    private void Shoot() => _gunAnimator.SetTrigger(shoot);
    private void OnDestroy() {
        _gun.shoot -= Shoot;
    }
    
    
}
