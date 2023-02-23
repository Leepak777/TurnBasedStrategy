using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.UI;


public class StatUpdate : MonoBehaviour
{
    public Dictionary<string,float> stats = new Dictionary<string,float>();
    public void setUp(){
        //Raw stat
        stats.Add("pow",0);//power
        stats.Add("dex",0);//dexterity
        stats.Add("tou",0);//toughness
        stats.Add("acu",0);//accuity
        stats.Add("mid",0);//mind
        stats.Add("ma",0);//melee attack
        stats.Add("ra",0);//range attack
        stats.Add("md",0);//melee defence
        stats.Add("rd",0);//range defence
        stats.Add("sa",0);//spell attack
        stats.Add("mr",0);//magic resistance
        stats.Add("base_hp",0);
        stats.Add("base_mov",0);
        stats.Add("base_init",0);
        stats.Add("base_enc",0);
        stats.Add("base_ene",0);
        //weapon
        stats.Add("wd",0);//weapon damage
        stats.Add("rng",0);//range
        stats.Add("pscal",0);//pow scaling
        stats.Add("dscal",0);//dex scaling
        stats.Add("sp",0);//shield piercing
        stats.Add("ap",0);//armor piercing
        stats.Add("acc",0);//accuracy
        stats.Add("pr",0);//parry rating
        stats.Add("pv",0);//parry value
        stats.Add("w_enc",0);
        stats.Add("attack_num",0);
        //armor
        stats.Add("av",0);//armor value
        stats.Add("sv",0);//shield value
        stats.Add("mdb",0);//melee defence bonus
        stats.Add("rdb",0); //range defence bonus
        stats.Add("eq_mov",0);
        stats.Add("eq_init",0);
        stats.Add("eq_enc",0);
         //calculated stat
        stats.Add("hp",0);//health point
        stats.Add("ene",0);//energy
        stats.Add("fat",0);//fatigue
        stats.Add("stb",0);//stability
        stats.Add("maxstb",0); 
        stats.Add("mov",0);//movement range
        stats.Add("bite",0);//base initiative
        stats.Add("enc",0);//encumbrance
        stats.Add("size",0);//size
    } 

    public void setStat(float pow,float dex,float tou,float acu,float mid, float ma, float ra, float sa, float md, float rd, float mr){
        stats["pow"] += pow;
        stats["dex"] += dex;
        stats["tou"] += tou;
        stats["acu"] += acu;
        stats["mid"] += mid;
        stats["ma"] += ma;
        stats["ra"] += ra;
        stats["md"] += md;
        stats["rd"] += rd;
        stats["sa"] += sa;
        stats["mr"] += mr;
    }
    public void setBaseStat(float base_hp, float base_ene, float base_mov, float base_init, float base_enc){
        stats["base_hp"] += base_hp;
        stats["base_ene"] += base_ene;
        stats["base_mov"] += base_mov;
        stats["base_init"] += base_init;
        stats["base_enc"] += base_enc;    
    }

    public void setWeaponStat(float wd,float pscal,float dscal,float ap,float sp,float rng,float acc, float mdb, float rdb, float w_enc, float attack_num){
        stats["wd"] += wd;
        stats["rng"] += rng;
        stats["pscal"] += pscal;
        stats["dscal"] += dscal;
        stats["sp"] += sp;
        stats["ap"] += ap;
        stats["acc"] += acc;
        stats["mdb"] += mdb;
        stats["rdb"] += rdb;
        stats["attack_num"] += attack_num;
        stats["w_enc"] += w_enc;
    }
    public void setShieldStat(float pr, float pv, float mdb, float rdb, float init, float enc){
        stats["pr"] += pr;
        stats["pv"] += pv;
        stats["mdb"] += mdb;
        stats["rdb"] += rdb;
        stats["eq_init"] += init;
        stats["eq_enc"] += enc;
    }
    public void setBucklerStat(float pr, float pv, float acc, float mdb, float rdb, float enc){
        stats["pr"] += pr;
        stats["pv"] += pv;
        stats["mdb"] += mdb;
        stats["rdb"] += rdb;
        stats["acc"] += acc;
        stats["eq_enc"] += enc;
    }
    public void setMounStat(float base_hp, float mdb, float mov, float init, float enc){
        stats["base_hp"] += base_hp;
        stats["mdb"] += mdb;
        stats["eq_mov"] += mov;
        stats["eq_init"] += init;
        stats["eq_enc"] += enc;

    }
    public void setArmorStat(float av, float sv, float mdb, float rdb, float eq_mov, float eq_init, float eq_enc){
        stats["av"] += av;
        stats["sv"] += sv;
        stats["mdb"] += mdb;
        stats["rdb"] += rdb;
        stats["eq_mov"] += eq_mov;
        stats["eq_init"] += eq_init;
        stats["eq_enc"] += eq_enc;
    }
    public void setCalStat(){
        stats["hp"] = stats["tou"]*5 + stats["pow"] + stats["base_hp"];
        stats["maxHealth"] = stats["hp"];
        stats["ene"] = stats["mid"] * 4 + stats["acu"] + stats["base_ene"];
        stats["fat"] = 0;
        stats["stb"] = stats["mid"]*5 +stats["acu"]*3 + stats["tou"] *2;
        stats["maxStb"] = stats["mid"] * 7 + stats["acu"] * 4 + stats["tou"] * 3;
        stats["mov"] = stats["base_mov"] + stats["pow"]/2 + stats["eq_mov"];
        stats["maxTiles"] = (int)stats["mov"];
        stats["enc"] = stats["w_enc"] + stats["eq_enc"] + stats["base_enc"] - stats["pow"]/4;
        stats["bite"] = stats["base_init"] + stats["dex"] + stats["acu"] + stats["eq_init"] - stats["enc"];
    }

    public bool meleeRoll(GameObject enemy){
        float player_roll = drn.getDRN() + stats["ma"]+ stats["acc"] + stats["pow"]/2 + stats["dex"] + stats["acu"]/2;
        StatUpdate en_stat = enemy.GetComponent<StatUpdate>();
        float enemy_roll = drn.getDRN() + en_stat.getDictStats("md") + en_stat.getDictStats("mdb") + en_stat.getDictStats("tou")/2 + en_stat.getDictStats("dex")/2 + en_stat.getDictStats("acu")/2;
        Debug.Log("Player: "+player_roll);
        Debug.Log("Enemy: " +enemy_roll);
        if(player_roll > enemy_roll){
            return true;
        }
        return false;
    }

    public bool rangeRoll(GameObject enemy){
        Vector3Int enpos = tilemap.WorldToCell(enemy.transform.position);
        Vector3Int playerpos = tilemap.WorldToCell(transform.position);
        float player_roll = drn.getDRN() + stats["ra"]+ stats["acc"] + stats["dex"] + stats["acu"]/2 - movement.GetDistance(enpos, playerpos);
        StatUpdate en_stat = enemy.GetComponent<StatUpdate>();
        float enemy_roll = drn.getDRN() + en_stat.getDictStats("rd") + en_stat.getDictStats("rdb") + en_stat.getDictStats("dex")/2 + en_stat.getDictStats("acu")/2;
        //Debug.Log("Player: "+player_roll);
        //Debug.Log("Enemy: " +enemy_roll);
        if(player_roll > enemy_roll){
            return true;
        }
        return false;
    }

    float currentHealth;
    public float maxHealth = 100f;
    public float Damage = 0;
    public HealthBar healthBar;
    TurnManager tm;
    public List<bool> buff =new List<bool>();
    public bool flag = false;
    public float bonus = 0;
    Tilemap tilemap;
    GridGraph gridGraph;
    Movement movement;
    DRN drn;
    int maxTiles = 3;
    int attackrange = 1;
    Text text;

    public void attackEn(GameObject targetEnemy){
        Vector3Int enpos = tilemap.WorldToCell(targetEnemy.transform.position);
        Vector3Int playerpos = tilemap.WorldToCell(transform.position);
        bool drn_check;
        if(movement.IsAdjacent(enpos,playerpos)){
            drn_check = meleeRoll(targetEnemy);
        }
        else{
            drn_check = rangeRoll(targetEnemy);
        }
        if(drn_check){
            Damage = drn.getDRN() + stats["wd"] + stats["pscal"] * stats["pow"] + stats["dscal"]*stats["dex"];
            attackingFatigue();
            targetEnemy.GetComponent<StatUpdate>().TakeDamage(Damage);
            targetEnemy.GetComponent<StatUpdate>().attackedFatigue();
        }
        
    }



    void Start()
    {   
        drn = this.gameObject.GetComponent<DRN>();
        movement = this.gameObject.GetComponent<Movement>(); 
        text = this.gameObject.GetComponentInChildren<Text>();
        currentHealth = maxHealth;
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        gridGraph = GameObject.Find("Tilemanager").GetComponent<GridGraph>();
        tm = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        //healthBar = this.gameObject.GetComponentInChildren<HealthBar>();
        for(int i = 0; i < 18; i++){
            buff.Add(false);
        }
        
    }
    void Update(){
        if(currentHealth <= 0){
            gridGraph.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position),true);
            tm.turnOrder2.Remove(this.gameObject);
            Destroy(this.gameObject);
        }

        
        
    }

    void showText(){
        text.enabled = true;
        Invoke("disableText",2f);
    }
    void disableText(){
        text.enabled = false;
    }

    public void TakeDamage(float damage)
    {
        
        float protection = drn.getDRN() + stats["sv"] * (100-stats["sp"])/100 + stats["av"]*(100 -stats["ap"])/100 + stats["tou"]/4;
        //Debug.Log("Damage: "+damage);
        //Debug.Log("Protection: " +protection);
        text.text = ""+Damage;
        showText();
        damage -= protection;
        if(damage <= 0){
            damage = 0;
        }
        Debug.Log("hit!" + damage);
        currentHealth -= damage;
        if(currentHealth < 0){
            currentHealth = 0;
        }
        healthBar.UpdateHealth(currentHealth);
        flag = false;
    }

    public void modifyStat(Dictionary<string,float> lst, bool buff){
        if(buff){
            foreach(KeyValuePair<string, float> s in lst){
                stats[s.Key] += lst[s.Key];
            }
        }
        else{
            foreach(KeyValuePair<string, float > s in lst){
                stats[s.Key] -= lst[s.Key];
            }
        }
    }

    public void multiplyStat(Dictionary<string,float> lst, bool buff){
        if(buff){
            foreach(KeyValuePair<string, float> s in lst){
                stats[s.Key] *= lst[s.Key];
            }
        }
        else{
            foreach(KeyValuePair<string, float > s in lst){
                stats[s.Key] /= lst[s.Key];
            }
        }
    }
    
    public void Flagging(){
        this.flag = true;
    }

    public float getDictStats(string name){
        return stats[name];
    }

    public bool getbuff(int i){
        return buff[i];
    }

    public void setbuff(int i, bool b){
        buff[i] = b;
    }

    public void setDamage(int i){
        Damage = i;
    }
    public void saddDamage(int i){
        Damage += i;
    }

    public bool getTextEnabled(){
        return text.enabled;
    }

    public void setTextActive(bool active){
        text.enabled = active;
    }

    public int getMaxTiles(){
        
        return (int)stats["mov"]/4 +1;
    }

    public void setMaxTiles(int x){
        stats["mov"] = (float)x;
    }

    public float getMaxHealth(){
        return maxHealth;
    }

    public void setMaxHealth(float x){
        maxHealth = x;
    }
    
    public void setAttackRange(int x){
        attackrange = x;
    }
    public int getAttackRange(){
        return (int)stats["rng"];
    }

    public void attackingFatigue(){
        stats["fat"] += stats["enc"];
    }

    public void attackedFatigue(){
        stats["fat"]++;
    }

    public void tileFatigue(int tiles){
        
        stats["fat"] += tiles;
        //Debug.Log(this.gameObject.tag + "Fatigue: "+ stats["fat"]);

    }

    public void restoreFatigue(){
        stats["fat"] -= stats["tou"];
        //Debug.Log("Fatigue: "+fat);
        //Debug.Log("Toughness: "+tou);
    if(stats["fat"] <= 0){
            stats["fat"] = 0;
        }
    }

    public void checkFatigue(int tiles){
        tileFatigue(tiles);
        restoreFatigue();
        if(stats["fat"] >= 75){
            stats["rd"] -= stats["fat"] / 5;
            stats["md"] -= stats["fat"] / 10;
            stats["ma"] -= stats["fat"] / 20;
            stats["ra"] -= stats["fat"] / 30;
        }
        if(stats["fat"] >= 100){
            stats["stb"] -= stats["fat"] - 100;
            stats["fat"] = 100;
        }

    }

    public void destablize(){
        
    }

    public void stabilityCheck(){

    }


}
