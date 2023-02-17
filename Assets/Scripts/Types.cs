using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Types:MonoBehaviour
{
    /*
    0. pow
    1. dex
    2. tou
    3. acu
    4. mid
    5. base HP
    6. base ene
    7. base mov
    8. base init
    9. base enc
    10. ma
    11. ra
    12. sa
    13. md
    14. rd
    15. mr
    */
    float[] Praetorian_Guard = {8,6,7,4,3,10,0,1,10,0,15,9,3,16,16,13};
    float[] Imperial_Legionary={7,5,6,2,3,10,0,1,10,0,13,9,3,15,14,10};
    float[] Imperial_Conscript = {5,5,5,1,2,0,0,1,10,0,10,7,3,10,10,10};
    float[] Mecenary = {6,6,5,2,2,5,0,1,11,0,12,11,3,13,12,10};
    float[] Brigand = {4,5,4,1,2,0,0,2,10,0,9,11,3,9,9,10};

    public float[] getTypeStat(string type){
        switch(type){
            case "Praetorian Guard":
                return Praetorian_Guard;
            case "Imperial Legionary":
                return Imperial_Legionary;
            case "Imperial Conscript":
                return Imperial_Conscript;
            case "Mecenary":
                return Mecenary;
            case "Brigand":
                return Brigand;
        }
        return null;
    }
}
