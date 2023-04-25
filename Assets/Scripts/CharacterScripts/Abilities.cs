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
using Random = UnityEngine.Random;

public class Abilities : MonoBehaviour
{
    TileManager tileM;
    public List<UnityEvent<GameObject>> UniversalCheck = new List<UnityEvent<GameObject>>();
    public List<UnityEvent<GameObject>> IndividualCheck = new List<UnityEvent<GameObject>>();
    CharacterStat stats;
    AbilitiesData abilitiesData;
    
    public void GameCheck(){
        foreach(UnityEvent<GameObject> e in UniversalCheck){
            e.Invoke(gameObject);
        }
    }
    public void CharacterCheck(){
        foreach(UnityEvent<GameObject> e in IndividualCheck){
            e.Invoke(gameObject);
        }
    }
    void Start()
    {
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        stats = AssetDatabase.LoadAssetAtPath<CharacterStat>("Assets/Scripts/Data/"+gameObject.name+".asset");
        abilitiesData = AssetDatabase.LoadAssetAtPath<AbilitiesData>("Assets/Scripts/Data/AbilitiesData.asset");
        foreach(KeyValuePair<string,string> ability in stats.getAbilities()){
            UnityEvent<GameObject> e = abilitiesData.getEvent(ability.Key);
            switch(ability.Value){
                case"Universal":
                    UniversalCheck.Add(e);
                    break;
                case "Individual":
                    IndividualCheck.Add(e);
                    break;
            }
        }
    }

    

    

    

    
}
