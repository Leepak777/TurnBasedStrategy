using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DRN
{
    int dice_1;
    int dice_2;

    Random rnd;

    private static DRN instance = new DRN();

    private DRN()
    {
        dice_1 = 0;
        dice_2 = 0;
        rnd = new Random((int)Time.time*1000);
    }

    public static DRN getInstance()
    {
        return instance;
    }

    public int getDRN(){
        int retVal = 0;
        
        dice_1 = rnd.Next(1,7);
        dice_2 = rnd.Next(1,7);
        retVal = dice_1 + dice_2;
        while(dice_1 == 6 || dice_2 == 6){
            if(dice_1 == 6){
                retVal--;
                dice_1 = rnd.Next(1,6);
                retVal += dice_1;
            }
            if(dice_2 == 6){
                retVal--;
                dice_2 = rnd.Next(1,6);
                retVal += dice_2;
            }
        }
        return retVal;
    }
    
}
