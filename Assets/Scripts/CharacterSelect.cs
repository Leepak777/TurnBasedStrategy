using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;

using Object = UnityEngine.Object;
public class CharacterSelect : MonoBehaviour
{
    public InGameData data;
    Dictionary<string,Sprite> Daemons = new Dictionary<string,Sprite>();
    Random rnd = new Random();
    public Dropdown dropdown;
    public Image img;
    public static List<string> types;
    public UDictionary<string,string> attributes = new UDictionary<string,string>(); 
    private void Start()
    {
        types = new List<string>(){"none","Praetorian Guard", "Imperial Legionary", "Imperial Conscript", "Mecenary", "Brigand"};
        data =AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
        Sprite[] allsprites = Resources.LoadAll<Sprite>("Daemons");
        foreach(Sprite s in allsprites){
            //Debug.Log(s.name);
            Daemons.Add(s.name,s);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(types);
    }
    public Sprite getRandomSprite(){
        return Daemons.ElementAt(rnd.Next(0,Daemons.Count)).Value;
    }
    public void setCharacterType(int option) {
        img.sprite = getRandomSprite();
        if(attributes.ContainsKey("Type")){
            attributes["Type"] = types[option];
        }
        else{
            attributes.Add("Type", types[option]);
        }
    }
    public void reset(){
        img.sprite = null;
        if(data.characterlst.ContainsKey(gameObject.name)){
            data.characterlst.Remove(gameObject.name);
        }
        attributes.Clear();
    }
    public void setCurrent(){
        if(data.sprites.ContainsKey(gameObject.name)){
            data.sprites[gameObject.name] = img.sprite;
        }
        else{
            data.sprites.Add(gameObject.name, img.sprite);
        }
        data.currentSetCh = gameObject.name;
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void Confirm(){
        if(data.characterlst.ContainsKey(gameObject.name)){
            for (int i = 0; i < attributes.Count; i++)
            {
                var at = attributes.ElementAt(i);
                if(data.characterlst[gameObject.name].ContainsKey(at.Key)){
                    data.characterlst[gameObject.name][at.Key] = at.Value;
                }
                else{
                    data.characterlst[gameObject.name].Add(at);
                }
            }
            /*foreach(KeyValuePair<string,string> at in attributes){
                if(data.characterlst[gameObject.name].ContainsKey(at.Key)){
                    data.characterlst[gameObject.name][at.Key] = at.Value;
                }
                else{
                    data.characterlst[gameObject.name].Add(at);
                }
            }*/
        }
        else{
            data.characterlst.Add(gameObject.name, attributes);
        }
        if(data.sprites.ContainsKey(gameObject.name)){
            data.sprites[gameObject.name] = img.sprite;
        }
        else{
            data.sprites.Add(gameObject.name, img.sprite);
        }
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
