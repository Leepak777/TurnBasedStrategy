using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;

using Object = UnityEngine.Object;

public class EquipmentSetup : MonoBehaviour
{
    // Start is called before the first frame update
    public Dropdown weapon;
    public Dropdown armor;
    public Dropdown shield;
    public Dropdown buckler;
    public Dropdown mount;
    public Image img;
    public InGameData data;
    public Text txt;
    public Equipments eq;
    public UDictionary<string,string> attributes = new UDictionary<string,string>();
    void Start()
    {
        
    }
    public void setScene(){
        AssetDatabase.Refresh();
        data =AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
        attributes.Clear();
        weapon.ClearOptions();
        armor.ClearOptions();
        shield.ClearOptions();
        buckler.ClearOptions();
        mount.ClearOptions();
        weapon.AddOptions(eq.weapon);
        armor.AddOptions(eq.armor);
        shield.AddOptions(eq.shield);
        buckler.AddOptions(eq.buckler);
        mount.AddOptions(eq.mount);
        UDictionary<string, UDictionary<string,string>> chlst = data.characterlst;
        if(chlst.ContainsKey(data.currentSetCh)){
            UDictionary<string,string> curlst = chlst[data.currentSetCh];
            if(curlst.ContainsKey("Weapon")){
                weapon.value = weapon.options.FindIndex(x => x.text == curlst["Weapon"]);
            }
            if(curlst.ContainsKey("Armor")){
                armor.value = armor.options.FindIndex(x => x.text == curlst["Armor"]);
            }
            if(curlst.ContainsKey("Shield")){
                shield.value = shield.options.FindIndex(x => x.text == curlst["Shield"]);
            }
            if(curlst.ContainsKey("Buckler")){
                buckler.value = buckler.options.FindIndex(x => x.text == curlst["Buckler"]);
            }
            if(curlst.ContainsKey("Mount")){
                mount.value = mount.options.FindIndex(x => x.text == curlst["Mount"]);
            }
        }
        txt.text = data.currentSetCh;
        if(data.sprites.ContainsKey(txt.text)){
            img.sprite = data.sprites[txt.text]; 
        }
    }
    public void setCharacterWeapon(int option) {
        if(attributes.ContainsKey("Weapon")){
            attributes["Weapon"] = eq.weapon[option];
        }
        else{
            attributes.Add("Weapon", eq.weapon[option]);
        }
    }
    public void setCharacterShield(int option) {
        if(attributes.ContainsKey("Shield")){
            attributes["Shield"] = eq.shield[option];
        }
        else{
            attributes.Add("Shield", eq.shield[option]);
        }
    }
    public void setCharacterArmor(int option) {
        if(attributes.ContainsKey("Armor")){
            attributes["Armor"] = eq.armor[option];
        }
        else{
            attributes.Add("Armor", eq.armor[option]);
        }
    }
    public void setCharacterBuckler(int option) {
        if(attributes.ContainsKey("Buckler")){
            attributes["Buckler"] = eq.buckler[option];
        }
        else{
            attributes.Add("Buckler", eq.buckler[option]);
        }
    }
    public void setCharacterMount(int option) {
        if(attributes.ContainsKey("Mount")){
            attributes["Mount"] = eq.mount[option];
        }
        else{
            attributes.Add("Mount", eq.mount[option]);
        }
    }
    public void Confirm(){
        if(data.characterlst.ContainsKey(data.currentSetCh)){
            for (int i = 0; i < attributes.Count; i++)
            {
                var at = attributes.ElementAt(i);
                if(data.characterlst[data.currentSetCh].ContainsKey(at.Key)){
                    data.characterlst[data.currentSetCh][at.Key] = at.Value;
                }
                else{
                    data.characterlst[data.currentSetCh].Add(at);
                }
            }
        }
        else{
            data.characterlst.Add(data.currentSetCh, attributes);
        }
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        returnCheck();
    }
    public void returnCheck(){
        if(GameObject.Find(data.currentSetCh+"_info")){
                GameObject.Find(data.currentSetCh+"_info").GetComponent<SetInfo>().updateInfo();
        }
    }
}
