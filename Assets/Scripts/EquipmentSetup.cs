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
    public UDictionary<string,string> attributes = new UDictionary<string,string>();
    List<string> wea = new List<string>(){"none","Light Glaive", "Gladius", "Power Sword", "Great Sword", "Pike","Mace","Pistol","Rifle"};
    List<string> arm = new List<string>(){"none","Plate","Half Plate","Synthe Armor","Legionary Armor","Praetorian_Armor","Flak Suit","Assault Vest","Personal Shield MK I","Personal Shield MK II","Personal Shield MK III"};
    List<string> shie = new List<string>(){"none","Kite Shield","Tower Shield"};
    List<string> buc = new List<string>(){"none","Buckler","Buckler2"};
    List<string> mou = new List<string>(){"none","Pack Horse","War Horse"};
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
        weapon.AddOptions(wea);
        armor.AddOptions(arm);
        shield.AddOptions(shie);
        buckler.AddOptions(buc);
        mount.AddOptions(mou);
        /*if(data.characterlst.ContainsKey(data.currentSetCh)){
            if(data.characterlst[data.currentSetCh].ContainsKey("Weapon")){
                weapon.value = weapon.options.FindIndex(x => x.text == data.characterlst[data.currentSetCh]["Weapon"]);
            }
            if(data.characterlst[data.currentSetCh].ContainsKey("Armor")){
                armor.value = armor.options.FindIndex(x => x.text == data.characterlst[data.currentSetCh]["Armor"]);
            }
            if(data.characterlst[data.currentSetCh].ContainsKey("Shield")){
                shield.value = shield.options.FindIndex(x => x.text == data.characterlst[data.currentSetCh]["Shield"]);
            }
            if(data.characterlst[data.currentSetCh].ContainsKey("Buckler")){
                buckler.value = buckler.options.FindIndex(x => x.text == data.characterlst[data.currentSetCh]["Buckler"]);
            }
            if(data.characterlst[data.currentSetCh].ContainsKey("Mount")){
                mount.value = mount.options.FindIndex(x => x.text == data.characterlst[data.currentSetCh]["Mount"]);
            }
        }*/
        txt.text = data.currentSetCh;
        if(data.sprites.ContainsKey(txt.text)){
            img.sprite = data.sprites[txt.text]; 
        }
    }

    public void setCharacterWeapon(int option) {
        if(attributes.ContainsKey("Weapon")){
            attributes["Weapon"] = wea[option];
        }
        else{
            attributes.Add("Weapon", wea[option]);
        }
    }
    public void setCharacterShield(int option) {
        if(attributes.ContainsKey("Shield")){
            attributes["Shield"] = shie[option];
        }
        else{
            attributes.Add("Shield", shie[option]);
        }
    }
    public void setCharacterArmor(int option) {
        if(attributes.ContainsKey("Armor")){
            attributes["Armor"] = arm[option];
        }
        else{
            attributes.Add("Armor", arm[option]);
        }
    }
    public void setCharacterBuckler(int option) {
        if(attributes.ContainsKey("Buckler")){
            attributes["Buckler"] = buc[option];
        }
        else{
            attributes.Add("Buckler", buc[option]);
        }
    }
    public void setCharacterMount(int option) {
        if(attributes.ContainsKey("Mount")){
            attributes["Mount"] = mou[option];
        }
        else{
            attributes.Add("Mount", mou[option]);
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
    }
}
