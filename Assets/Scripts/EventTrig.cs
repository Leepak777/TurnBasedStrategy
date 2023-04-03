using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class EventTrig: MonoBehaviour
{
    public UnityEvent mapClick;
    bool inButton = false;
    bool navigate = false;
    public GameObject end, attack;
    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.MouseUp && e.button  == 0 && !navigate)
        {
            mapClick.Invoke();
        }
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
        {
            navigate = !navigate;
            attack.SetActive(!attack.activeInHierarchy);
            end.SetActive(!end.activeInHierarchy);
            if(!navigate){
                inButton = false;
            }
        }
        
    }
    public void setInButton(bool inB){
        this.inButton = inB;
    }
    public bool checkOnButton(){
        return inButton;
    }

    public bool isNavigate(){
        return navigate;
    }
}