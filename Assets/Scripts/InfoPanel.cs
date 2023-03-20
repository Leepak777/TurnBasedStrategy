using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;
public class InfoPanel : MonoBehaviour
{
    public void toggle(){
        if(GameObject.Find(gameObject.name+"_info") == null){
            addInfo();
        }
        else{
            removeInfo();
        }
    }
    public void addInfo(){
        GameObject goParent = GameObject.Find("scrollPanel");
        if(gameObject.name.Contains("Enemy")){
            goParent = GameObject.Find("scrollPanel_en");
        }
        GameObject prefab = Resources.Load<GameObject>("character_Info") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.GetComponent<SetInfo>().settext(this.gameObject.name);
        player.name = gameObject.name+"_info";
        player.transform.SetParent(goParent.transform);
        player.transform.localScale = new Vector3(1, 1, 1);
        player.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex()+1);
        
    }
    public void removeInfo(){
       string name = gameObject.name+"_info";
       GameObject player = GameObject.Find(name);
       Destroy(player);
    }
    
}
