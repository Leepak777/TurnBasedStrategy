using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;

using Object = UnityEngine.Object;
public class CameraManager : MonoBehaviour
{

    //0:player 1:enemy 2:equipment 
    int prev = 0;
    SceneLoader sceneLoader;
    public InGameData data;
    void Start()
    {
        data =AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
        data.positions.Clear();
        data.characterlst.Clear();
        data.sprites.Clear();
        data.currentSetCh = null;
        sceneLoader = GetComponent<SceneLoader>();
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    public void setprev(int i){
        prev = i;
    }
    public void toPrev(){
        if(prev == 0){
            GameObject.Find("Enemy").SetActive(false);
        }
        else{
            GameObject.Find("Main Camera").SetActive(false);
        }
    }
    public void LoadMyScene()
    {
        if(data.sprites.Count == 0 || data.characterlst.Count == 0){
            return;
        }
        foreach(KeyValuePair<string,UDictionary<string,string>> val in data.characterlst){
            if(!val.Value.ContainsKey("Type")){
                return;
            }
        }
        if(!data.characterlst.ContainsKey("Player1") || !data.characterlst.ContainsKey("Enemy1")){
            return;
        }
        sceneLoader.LoadScene("MapSelection");
    }
}
