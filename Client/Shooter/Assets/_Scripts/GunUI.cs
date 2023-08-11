using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour {
    [SerializeField] private Image [] _guns;
    [SerializeField] private PlayerGun _playerGun;
    private void OnEnable() => _playerGun.ChangedGun += SelectGun;
    private void Start() {
        SelectGun(_playerGun.SelectedGun);
    }
    private void SelectGun(int value){
        for (int i = 0; i < _guns.Length; i++){
            if(i == value){
                _guns[i].color = Color.white;
            }else{
                _guns[i].color = Color.grey;
            }
        }
    }
     private void OnDisable() => _playerGun.ChangedGun -= SelectGun;
}
