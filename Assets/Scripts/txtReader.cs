using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;



public class txtReader : MonoBehaviour
{ 
    float[] tst;
    float[] wst;
    float[] sst;
    float[] ast;
    float[] bst;
    float[] mst;
    public Tilemap tilemap;
    Dictionary<string,Sprite> Daemons = new Dictionary<string,Sprite>();
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
        tst = this.gameObject.GetComponent<Types>().getTypeStat(type);
        wst = this.gameObject.GetComponent<Equipments>().getWeaponStat(weapon);
        sst = this.gameObject.GetComponent<Equipments>().getShieldStat(shield);
        ast = this.gameObject.GetComponent<Equipments>().getArmorStat(armor);
        bst = this.gameObject.GetComponent<Equipments>().getBucklerStat(buckler);
        mst = this.gameObject.GetComponent<Equipments>().getMountStat(mount);
        if(tst!=null){
            character.GetComponent<StatUpdate>().setStat(tst[0],tst[1],tst[2],tst[3],tst[4],tst[10],tst[11],tst[12],tst[13],tst[14],tst[15]);
            character.GetComponent<StatUpdate>().setBaseStat(tst[5],tst[6],tst[7],tst[8],tst[9]);
        }
        if(wst!=null){
            character.GetComponent<StatUpdate>().setWeaponStat(wst[0],wst[1],wst[2],wst[3],wst[4],wst[5],wst[6],wst[7],wst[8],wst[9],wst[10]);
        }
        if(sst!=null){
            character.GetComponent<StatUpdate>().setShieldStat(sst[0],sst[1],sst[2],sst[3],sst[4],sst[5]);
        }
        if(bst!= null){
            character.GetComponent<StatUpdate>().setBucklerStat(bst[0],bst[1],bst[2],bst[3],bst[4],bst[5]);
        }
        if(ast!=null){
            character.GetComponent<StatUpdate>().setArmorStat(ast[0],ast[1],ast[2],ast[3],ast[4],ast[5],ast[6]);
        }
        if(mst!=null){
            character.GetComponent<StatUpdate>().setMounStat(mst[0],mst[1],mst[2],mst[3],mst[4]);
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
                player.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(20,12,0));
                //Praetorian Guard, plate, light glaive
                setStats(player,words[1],words[2],words[3],words[4],words[5],words[6]);
                player.GetComponent<StatUpdate>().setCalStat();
            }
            else if(words[0] == "早上好中國"){
                GameObject prefab = Resources.Load<GameObject>("PlayerCh") as GameObject;
                GameObject enemy = Instantiate(prefab) as GameObject;
                enemy.tag = "Enemy";
                enemy.GetComponent<SpriteRenderer>().sprite = Daemons["Daemons_5"];
                enemy.transform.SetParent(transform);
                enemy.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(24,12,0));
                //imperial legionary, synthe armor, pike
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
