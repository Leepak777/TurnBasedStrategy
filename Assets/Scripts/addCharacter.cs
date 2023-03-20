using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;
public class addCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    public void addPlayer(){
        GameObject goParent = GameObject.Find("scrollPanel");
        GameObject prefab = Resources.Load<GameObject>("PlayerSetup") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.name = "Player" + (goParent.transform.childCount+1);
        player.transform.SetParent(goParent.transform);
        player.transform.localScale = new Vector3(1, 1, 1);
    }

    public void addEnemy(){
        GameObject goParent = GameObject.Find("scrollPanel_en");
        GameObject prefab = Resources.Load<GameObject>("EnemySetup") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.name = "Enemy" + (goParent.transform.childCount+1);
        player.transform.SetParent(goParent.transform);
        player.transform.localScale = new Vector3(1, 1, 1);
    }

    public void removePlayer(){
       string name ="Player"+GameObject.Find("scrollPanel").transform.childCount;
       GameObject player = GameObject.Find(name);
       Destroy(player);
       InGameData data =AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
       data.characterlst.Remove(name);
       data.sprites.Remove(name);
    }
     public void removeEnemy(){
        string name = "Enemy"+GameObject.Find("scrollPanel_en").transform.childCount;
        GameObject player = GameObject.Find(name);
        Destroy(player);
        InGameData data =AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
       data.characterlst.Remove(name);
       data.sprites.Remove(name);
    }
    
}
