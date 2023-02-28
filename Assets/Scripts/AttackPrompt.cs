using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AttackPrompt : MonoBehaviour
{
    Image attackPromptImage;
    TurnManager turnManager;
    bool ButtonPressed = false;
    //public Button attackButton1;
    //public Button attackButton2;
    //public Button attackButton3;
    public List<Button> buttons = new List<Button>();
    private void Start()
    {
        attackPromptImage = this.gameObject.GetComponent<Image>();
        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        foreach(GameObject b in GameObject.FindGameObjectsWithTag("button")){
            buttons.Add(b.GetComponent<Button>());
        }

        buttons[0].onClick.AddListener(Attack1);
        buttons[1].onClick.AddListener(Attack2);
        buttons[2].onClick.AddListener(Attack3);        
    }

    void Attack1()
    {
        ButtonPressed = true;
        turnManager.currentPlay.GetComponent<ActionCenter>().endTurn();
        turnManager.currentPlay.GetComponent<Movement>().origin = false;
    }

    void Attack2()
    {
        ButtonPressed = true;
        turnManager.currentPlay.GetComponent<Attack>().Attacking("Enemy");
        //Debug.Log("Attack 2");
    }

    void Attack3()
    {
        ButtonPressed = true;
        //Debug.Log("Attack 3");
    }

    public List<Button> getButtons(){
        return buttons;
    }
    private void Update()
    {
        /*if (turnManager.player == true)
        {
            attackPromptImage.enabled = true;
        }
        else
        {
            attackPromptImage.enabled = false;
        }*/
    }

    public bool checkOnButton(){
        foreach(Button b in buttons){
            if(checkMouseInButton(b)){
                return true;
            }
        }
        return false;
    }

    public bool checkMouseInButton(Button b){
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 buttonPos = b.transform.position;
        RectTransform bSize = b.GetComponentInChildren<RectTransform>();
        if(mouse.x > buttonPos.x - bSize.sizeDelta.x/2&& mouse.x < buttonPos.x + bSize.sizeDelta.x/2){
            if(mouse.y > buttonPos.y - bSize.sizeDelta.y/2 && mouse.y < buttonPos.y + bSize.sizeDelta.y/2){
                return true;
            }
        }
        return false;
    }

    public bool getPressed(){
        return ButtonPressed;
    }

    public void resetPressed(){
        ButtonPressed = false;
    }
}
