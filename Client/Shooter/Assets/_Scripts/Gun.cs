using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour {
    [SerializeField] protected Bullet _bulletPrefab;
    [SerializeField] private GameObject [] _guns;
    public Action shoot;

    public void SetGun(int value){
        for (int i = 0; i < _guns.Length; i++){
            _guns[i].SetActive(i == value ? true : false);
        }
    }
}

