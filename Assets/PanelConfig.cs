using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelConfig : MonoBehaviour
{
    public GameObject currentGO;
    public UI ui;
    public HealthBarPanel hbp;
    public Image icon;
    public Text txt;
    void Start()
    {
        
    }
    public void setCurrentGO(){
        currentGO = ui.getCurrentPlay();
        hbp.setCharacter(currentGO);
        icon.sprite = currentGO.GetComponent<SpriteRenderer>().sprite;
        setTxt();
    }

    public void setTxt(){
        StatUpdate stats = currentGO.GetComponent<StatUpdate>();
        string info = "Energy: "+stats.getDictStats("ene") + " \t Fatigue: " + stats.getDictStats("fat") + "/100\nStability: " + stats.getDictStats("stb");
        txt.text = info;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
