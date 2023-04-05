using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;public class Abilities : MonoBehaviour
{
    TileManager tileM;
    public List<UnityEvent<TileManager,GameObject>> StartCheck = new List<UnityEvent<TileManager,GameObject>>();
    public List<UnityEvent<TileManager,GameObject>> EndCheck = new List<UnityEvent<TileManager,GameObject>>();
    public List<UnityEvent<TileManager,GameObject>> MoveCheck = new List<UnityEvent<TileManager,GameObject>>();
    public List<UnityEvent<TileManager,GameObject>> StopCheck = new List<UnityEvent<TileManager,GameObject>>();
    CharacterStat stats;
    AbilitiesData abilitiesData;
    
    public void GameStartCheck(){
        foreach(UnityEvent<TileManager,GameObject> e in StartCheck){
            e.Invoke(tileM,gameObject);
        }
    }
    public void GameEndCheck(){
        foreach(UnityEvent<TileManager,GameObject> e in EndCheck){
            e.Invoke(tileM,gameObject);
        }
    }
    public void GameOnMove(){
        foreach(UnityEvent<TileManager,GameObject> e in MoveCheck){
            e.Invoke(tileM,gameObject);
        }
    }
    public void GameOnStop(){
        foreach(UnityEvent<TileManager,GameObject> e in StopCheck){
            e.Invoke(tileM,gameObject);
        }
    }
    void Start()
    {
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        stats = AssetDatabase.LoadAssetAtPath<CharacterStat>("Assets/Scripts/Data/"+gameObject.name+".asset");
        abilitiesData = AssetDatabase.LoadAssetAtPath<AbilitiesData>("Assets/Scripts/Data/AbilitiesData.asset");
        foreach(KeyValuePair<string,string> ability in stats.getAblst()){
            Debug.Log(ability);
            UnityEvent<TileManager,GameObject> e = abilitiesData.getEvent(ability.Key);
            if(e != null){
                switch (ability.Value){
                case "start":StartCheck.Add(e);break;
                case "end":EndCheck.Add(e);break;
                case "move":MoveCheck.Add(e);break;
                case "stop":StopCheck.Add(e);break;
                }
            }
        }
    }

    

    

    

    
}
