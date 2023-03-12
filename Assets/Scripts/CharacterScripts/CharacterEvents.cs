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
    public UnityEvent<GameObject> onAttacking;
    public UnityEvent onEnemyAttack;
    public UnityEvent<Vector3> onPlayerMove;
    public UnityEvent onEnemyMove;
    public UnityEvent onMoving;
    public UnityEvent<int> saveStat;
    public UnityEvent<int> onUndo;
    public UnityEvent<int> onReset;
    public UnityEvent onMoveStart;
    public UnityEvent onMoveStop;
    public UnityEvent onDeath;
    public UnityEvent onHover;//later UI for attack and stat showcase
    public UnityEvent onAttackTrue;
    public UnityEvent onAttackFalse;
    public UnityEvent<Vector3Int> onHighLight;
    public UnityEvent<List<Vector3Int>> onUnHighLight;
}
