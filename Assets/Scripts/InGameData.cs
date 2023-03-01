using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InGameData", order = 3)]
public class InGameData : ScriptableObject
{
    public Dictionary<string, Dictionary<string,string>> characterlst = new Dictionary<string, Dictionary<string,string>>();
}
