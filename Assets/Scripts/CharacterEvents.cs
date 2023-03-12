using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents : MonoBehaviour
{
    public UnityEvent<int> onDamage;
    public UnityEvent onStart;
    public UnityEvent<int> onEnd;
    public UnityEvent onDuring;
    public UnityEvent<Vector3> onPlayerAttack;
    public UnityEvent onEnemyAttack;
    public UnityEvent<Vector3> onPlayerMove;
    public UnityEvent onEnemyMove;
    public UnityEvent onMoving;
    public UnityEvent<int> saveStat;
}
