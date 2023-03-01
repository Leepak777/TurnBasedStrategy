using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;



public class txtReader : MonoBehaviour
{ 
    Dictionary<string,float> tst;
    Dictionary<string,float> wst;
    Dictionary<string,float> sst;
    Dictionary<string,float> ast;
    Dictionary<string,float> bst;
    Dictionary<string,float> mst;
    public Tilemap tilemap;
    Dictionary<string,Sprite> Daemons = new Dictionary<string,Sprite>();
    public Equipments equipments;
    public Types types;
    // Start is called before the first frame update
    void Start()
    { 
        Sprite[] allsprites = Resources.LoadAll<Sprite>("Daemons");
        foreach(Sprite s in allsprites){
            //Debug.Log(s.name);
            Daemons.Add(s.name,s);
        }
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        setStage();
        
    }

    //words
    /*
    0. Player/Enemy
    1. Type
    2. Weapon
    3. shield
    4. armor
    5. buckler
    6. mount
    */

    void setStats(GameObject character, string type, string weapon, string shield, string armor, string buckler, string mount){
        
        tst = types.getTypeStat(type);
        wst = equipments.getWeaponStat(weapon);
        sst = equipments.getShieldStat(shield);
        ast = equipments.getArmorStat(armor);
        bst = equipments.getBucklerStat(buckler);
        mst = equipments.getMountStat(mount);
        if(tst!=null){
            character.GetComponent<StatUpdate>().setStats(tst);
        }
        if(wst!=null){
            character.GetComponent<StatUpdate>().setStats(wst);
        }
        if(sst!=null){
            character.GetComponent<StatUpdate>().setStats(sst);
        }
        if(bst!= null){
            character.GetComponent<StatUpdate>().setStats(bst);
        }
        if(ast!=null){
            character.GetComponent<StatUpdate>().setStats(ast);
        }
        if(mst!=null){
            character.GetComponent<StatUpdate>().setStats(mst);
        }
    }

    public void setStage(){
        List<string> lst = ReadInputFileAsList();
        for(int i = 0; i < lst.Count; i++){
            string[] words = lst[i].Split(',');
            if(words[0] == "Poggers"){
                GameObject prefab = Resources.Load<GameObject>("PlayerCh") as GameObject;
                GameObject player = Instantiate(prefab) as GameObject;
                player.transform.SetParent(transform);
                player.GetComponent<SpriteRenderer>().sprite = Daemons["Daemons_9"];
                player.GetComponentInChildren<Ghost>().setSprite(player.GetComponent<SpriteRenderer>().sprite);
                player.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(20+i,12,0));
                //Praetorian Guard, plate, light glaive
                player.GetComponent<StatUpdate>().setUp();
                setStats(player,words[1],words[2],words[3],words[4],words[5],words[6]);
                player.GetComponent<StatUpdate>().setCalStat();
            }
            else if(words[0] == "早上好中國"){
                GameObject prefab = Resources.Load<GameObject>("PlayerCh") as GameObject;
                GameObject enemy = Instantiate(prefab) as GameObject;
                enemy.tag = "Enemy";
                enemy.GetComponent<SpriteRenderer>().sprite = Daemons["Daemons_5"];
                enemy.transform.SetParent(transform);
                enemy.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(24,12+i,0));
                //imperial legionary, synthe armor, pike
                enemy.GetComponent<StatUpdate>().setUp();
                setStats(enemy,words[1],words[2],words[3],words[4],words[5],words[6]);
                enemy.GetComponent<StatUpdate>().setCalStat();
            }
        }

    }

   

    public static List<string> ReadInputFileAsList() {
        string filePath = Application.dataPath + "/Character.txt";

        if (File.Exists(filePath)) {
            return File.ReadAllLines(filePath).ToList();
        } else {
            return new List<string>() { "File not found." };
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
