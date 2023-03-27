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
    public UnityEvent onSetAttack;
    public UnityEvent<Vector3> onPlayerAttack;
    public UnityEvent<GameObject> onAttacking;
    public UnityEvent onEnemyAttack;
    public UnityEvent<Vector3> onPlayerMove;
    public UnityEvent onEnemyMove;
    public UnityEvent<int> saveStat;
    public UnityEvent<int> onUndo;
    public UnityEvent onReset;
    public UnityEvent onMoveStop;
    public UnityEvent onDeath;
    public UnityEvent onHighLight;
    public UnityEvent onUnHighLight;
    public UnityEvent onClick;
    public UnityEvent<Vector3> setTargetTile;
    
}
