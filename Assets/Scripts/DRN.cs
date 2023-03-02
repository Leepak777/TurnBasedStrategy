using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DRN
{
    // Start is called before the first frame update
    
    int dice_1 = 0;
    int dice_2 = 0;
    int drn = 0;


    public int getDRN(){
        drn = 0;
        Random rnd = new Random();
        dice_1 = rnd.Next(1,7);
        dice_2 = rnd.Next(1,7);
        drn = dice_1 + dice_2;
        while(dice_1 == 6 || dice_2 == 6){
            if(dice_1 == 6){
                drn--;
                dice_1 = rnd.Next(1,6);
                drn += dice_1;
            }
            if(dice_2 == 6){
                drn--;
                dice_2 = rnd.Next(1,6);
                drn += dice_2;
            }
        }
        return drn;
    }
    // Update is called once per frame
    
}
