using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun {
    
    [SerializeField] private Transform _bulletPoint;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootDelay = 0.1f;
    private float _lastShootTime;
    
    public bool TryShoot(out ShootInfo info){
        info = new ShootInfo();

        if(Time.time - _lastShootTime < _shootDelay) return false;
        
        Vector3 position = _bulletPoint.position;
        Vector3 velocity = _bulletPoint.forward * _bulletSpeed;

        _lastShootTime = Time.time;
        Instantiate(_bulletPrefab,position, _bulletPoint.rotation).Init(velocity);
        shoot?.Invoke();

        
        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = velocity.x;
        info.dY = velocity.y;
        info.dZ = velocity.z;

        return true;
    } 

        
    
    IEnumerator CreateBullet(float timeDelay){
        yield return new WaitForSeconds(timeDelay);
        Instantiate(_bulletPrefab, _bulletPoint.position, _bulletPoint.rotation).Init(_bulletPoint.forward * _bulletSpeed);
    }
}
