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
using Random = System.Random;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
public class TitleScreen : MonoBehaviour
{
    SceneLoader sceneLoader;
    void Start()
    {
        sceneLoader = GetComponent<SceneLoader>();
    }


    public void ToTypeEditor()
    {
        sceneLoader.LoadScene("TypeEditor");
    }
    public void ToGameScene()
    {
        sceneLoader.LoadScene("GameScene");
    }
    public void ToEquipmentEditor()
    {
        sceneLoader.LoadScene("EquipmentEditor");
    }
    public void toCharacterSelect()
    {
        sceneLoader.LoadScene("CharacterSelection");
    }
    public void ToTitle()
    {
        sceneLoader.LoadScene("Title");
    }
    
}
