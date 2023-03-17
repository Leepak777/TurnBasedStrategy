using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class EventTrig: MonoBehaviour
{
    public UnityEvent mapClick;
    bool clicked = false;
    bool inButton = false;
    public void setClicked(bool click){
        this.clicked = click;
    }
    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse && e.button == 0 )
        {
            if(!clicked){
                mapClick.Invoke();
                clicked = true;
            }
            else{
                clicked = false;
            }
        }
        
    }
    public void setInButton(bool inB){
        this.inButton = inB;
    }
    public bool checkOnButton(){
        return inButton;
    }
}