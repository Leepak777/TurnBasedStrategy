using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class EventTrig: MonoBehaviour
{
    public UnityEvent mapClick;
    bool clicked = false;
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
}