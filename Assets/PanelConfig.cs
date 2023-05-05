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
    void Start()
    {
        
    }
    public void setCurrentGO(){
        currentGO = ui.getCurrentPlay();
        hbp.setCharacter(currentGO);
        icon.sprite = currentGO.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
