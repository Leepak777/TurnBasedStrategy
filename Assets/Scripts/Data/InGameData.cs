using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InGameData", order = 3)]
[System.Serializable]
public class InGameData : ScriptableObject
{
    public UDictionary<string, UDictionary<string,string>> characterlst = new UDictionary<string, UDictionary<string,string>>();
    public UDictionary<string, Vector3Int> positions = new UDictionary<string, Vector3Int>();
    public UDictionary<string, Sprite> sprites = new UDictionary<string, Sprite>();
    public string map;
    public string currentSetCh;
    
}
