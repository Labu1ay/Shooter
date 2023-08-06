using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    [SerializeField] private float _lifeTime = 5f;
    private Rigidbody _rigidbody;
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void Init(Vector3 velocity){
        _rigidbody.velocity = velocity;
        StartCoroutine(DelayDestroy(_lifeTime));
    }

    private IEnumerator DelayDestroy(float delay){
        yield return new WaitForSecondsRealtime(delay);
        Destroy();
    }
    private void Destroy(){
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other) {
        Destroy();
    }
}