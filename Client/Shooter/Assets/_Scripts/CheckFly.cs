using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFly : MonoBehaviour {
    public bool IsFly {get; private set;}
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _coyoteTime = 0.15f;
    private Coroutine coroutine;

    private void Update() {
        if(Physics.CheckSphere(transform.position, _radius, _layerMask)){
            IsFly = false;
            if(coroutine != null) StopCoroutine(coroutine);
            
        }else{
            coroutine = StartCoroutine(CoyoteTimeToFly(_coyoteTime));
        }
    }
    private IEnumerator CoyoteTimeToFly(float coyoteTime){
        yield return new WaitForSeconds(coyoteTime);
        IsFly = true;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
#endif
}
