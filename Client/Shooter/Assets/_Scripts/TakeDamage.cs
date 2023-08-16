using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour {
    [SerializeField] private EnemyCharacter _enemyCharacter;
    [SerializeField] private bool _head;

    public void ApplyDamage(int damage) => _enemyCharacter.ApplyDamage(damage * (_head ? 10 : 1));
}
