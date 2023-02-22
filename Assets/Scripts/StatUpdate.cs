using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;


public class StatUpdate : MonoBehaviour
{  
    //Raw stat 
    public float pow = 0;  //power
    public float dex = 0;  //dexterity
    public float tou = 0;  //toughness
    public float acu = 0;  //accuity
    public float mid = 0;  //mind
    public float ma = 0;   //melee attack
    public float ra = 0;   //range attack
    public float md = 0;   //melee defence
    public float rd = 0;   //range defence
    public float sa = 0;   //spell attack
    public float mr = 0;   //magic resistance
    public float base_hp = 0;
    public float base_mov = 0;
    public float base_init = 0;
    public float base_enc = 0;
    public float base_ene = 0;
    //weapon
    public float wd = 0;   //weapon damage
    public float rng = 0;  //range
    public float pscal = 0;//pow scaling
    public float dscal = 0;//dex scaling
    public float sp = 0;   //shield piercing
    public float ap = 0;   //armor piercing
    public float acc = 0;  //accuracy
    public float pr = 0;   //parry rating
    public float pv = 0;   //parry value
    public float w_enc = 0;
    public float attack_num = 0;
    //armor
    public float av = 0;   //armor value
    public float sv = 0;   //shield value
    public float mdb = 0;  //melee defence bonus
    public float rdb = 0;  //range defence bonus
    public float eq_mov = 0;
    public float eq_init = 0;
    public float eq_enc = 0;
    //calculated stat
    public float hp = 0;   //health point
    public float ene = 0;  //energy
    public float fat = 0;  //fatigue
    public float stb = 0;  //stability
    public float maxStb = 0;
    public float mov = 0;  //movement range
    public float bite = 0; //base initiative
    public float enc = 0;  //encumbrance
    public float size = 0; //size

    public void setStat(float pow,float dex,float tou,float acu,float mid, float ma, float ra, float sa, float md, float rd, float mr){
        this.pow += pow;
        this.dex += dex;
        this.tou += tou;
        this.acu += acu;
        this.mid += mid;
        this.ma += ma;
        this.ra += ra;
        this.md += md;
        this.rd += rd;
        this.sa += sa;
        this.mr += mr;
    }
    public void setBaseStat(float base_hp, float base_ene, float base_mov, float base_init, float base_enc){
        this.base_hp += base_hp;
        this.base_ene += base_ene;
        this.base_mov += base_mov;
        this.base_init += base_init;
        this.base_enc += base_enc;    
    }

    public void setWeaponStat(float wd,float pscal,float dscal,float ap,float sp,float rng,float acc, float mdb, float rdb, float w_enc, float attack_num){
        this.wd += wd;
        this.rng += rng;
        this.pscal += pscal;
        this.dscal += dscal;
        this.sp += sp;
        this.ap += ap;
        this.acc += acc;
        this.mdb += mdb;
        this.rdb += rdb;
        this.attack_num += attack_num;
        this.w_enc += w_enc;
    }
    public void setShieldStat(float pr, float pv, float mdb, float rdb, float init, float enc){
        this.pr += pr;
        this.pv += pv;
        this.mdb += mdb;
        this.rdb += rdb;
        this.eq_init += init;
        this.eq_enc += enc;
    }
    public void setBucklerStat(float pr, float pv, float acc, float mdb, float rdb, float enc){
        this.pr += pr;
        this.pv += pv;
        this.mdb += mdb;
        this.rdb += rdb;
        this.acc += acc;
        this.eq_enc += enc;
    }
    public void setMounStat(float base_hp, float mdb, float mov, float init, float enc){
        this.base_hp += base_hp;
        this.mdb += mdb;
        this.eq_mov += mov;
        this.eq_init += init;
        this.eq_enc += enc;

    }
    public void setArmorStat(float av, float sv, float mdb, float rdb, float eq_mov, float eq_init, float eq_enc){
        this.av += av;
        this.sv += sv;
        this.mdb += mdb;
        this.rdb += rdb;
        this.eq_mov += eq_mov;
        this.eq_init += eq_init;
        this.eq_enc += eq_enc;
    }
    public void setCalStat(){
        hp = tou*5 + pow + base_hp;
        maxHealth = hp;
        ene = mid * 4 + acu + base_ene;
        fat = 0;
        stb = mid*5 +acu*3 + tou *2;
        maxStb = mid * 7 + acu * 4 + tou * 3;
        mov = base_mov + pow/2 + eq_mov;
        maxTiles = (int)mov;
        enc = w_enc + eq_enc + base_enc - pow/4;
        bite = base_init + dex +acu + eq_init -enc;
    }

    public bool meleeRoll(GameObject enemy){
        float player_roll = drn.getDRN() + ma+ acc + pow/2 + dex + acu/2;
        StatUpdate en_stat = enemy.GetComponent<StatUpdate>();
        float enemy_roll = drn.getDRN() + en_stat.md + en_stat.mdb + en_stat.tou/2 + en_stat.dex/2 + en_stat.acu/2;
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
        float player_roll = drn.getDRN() + ra+ acc + dex + acu/2 - movement.GetDistance(enpos, playerpos);
        StatUpdate en_stat = enemy.GetComponent<StatUpdate>();
        float enemy_roll = drn.getDRN() + en_stat.rd + en_stat.rdb + en_stat.dex/2 + en_stat.acu/2;
        //Debug.Log("Player: "+player_roll);
        //Debug.Log("Enemy: " +enemy_roll);
        if(player_roll > enemy_roll){
            return true;
        }
        return false;
    }

    float currentHealth;
    public float maxHealth = 100f;
    public float Damage = 5;
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
            Damage = drn.getDRN() + wd + pscal * pow + dscal*dex;
            attackingFatigue();
            targetEnemy.GetComponent<StatUpdate>().TakeDamage(Damage);
            targetEnemy.GetComponent<StatUpdate>().attackedFatigue();
        }
        
    }
    /*if(!buff[3]){
            targetEnemy.GetComponent<StatUpdate>().flag = false;
        }
        if(targetEnemy.GetComponent<StatUpdate>().currentHealth <= 0 && this.gameObject.GetComponent<skills>().bloodlust){
            if(!this.gameObject.GetComponent<StatUpdate>().getbuff(14)){
                this.gameObject.GetComponent<StatUpdate>().setbuff(14,true);
                
            }
        }*/
    public void  attackPl(GameObject player){
         Vector3Int enpos = tilemap.WorldToCell(player.transform.position);
        Vector3Int playerpos = tilemap.WorldToCell(transform.position);
        bool drn_check;
        if(movement.IsAdjacent(enpos,playerpos)){
            drn_check = meleeRoll(player);
        }
        else{
            drn_check = rangeRoll(player);
        }
        if(drn_check){
            Damage = drn.getDRN() + wd + pscal * pow + dscal*dex;
            player.GetComponent<StatUpdate>().TakeDamage(Damage);
        }
    }

    void Start()
    {   
        drn = this.gameObject.GetComponent<DRN>();
        movement = this.gameObject.GetComponent<Movement>(); 
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

    public void TakeDamage(float damage)
    {
        float protection = drn.getDRN() + sv * (100-sp)/100 + av*(100 -ap)/100 + tou/4;
        Debug.Log("Damage: "+damage);
        Debug.Log("Protection: " +protection);
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
    }

    public bool getbuff(int i){
        return buff[i];
    }

    public void setbuff(int i, bool b){
        buff[i] = b;
    }

    public int getMaxTiles(){
        
        return maxTiles/4 +1;
    }

    public void setMaxTiles(int x){
        maxTiles = x;
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
        return (int)rng;
    }

    public void attackingFatigue(){
        fat += enc;
    }

    public void attackedFatigue(){
        fat++;
    }

    public void tileFatigue(int tiles){
        
        fat += tiles;
        Debug.Log("Fatigue: "+fat);

    }

    public void restoreFatigue(){
        fat -= tou;
        //Debug.Log("Fatigue: "+fat);
        //Debug.Log("Toughness: "+tou);
        if(fat <= 0){
            fat = 0;
        }
    }

    public void checkFatigue(int tiles){
        tileFatigue(tiles);
        restoreFatigue();
        if(fat >= 75){
            rd -=fat / 5;
            md -= fat / 10;
            ma -= fat / 20;
            ra -= fat / 30;
        }
        if(fat >= 100){
            stb -= fat - 100;
            fat = 100;
        }

    }

    public void destablize(){
        
    }

    public void stabilityCheck(){

    }


}
