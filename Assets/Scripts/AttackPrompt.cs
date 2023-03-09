using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
public class AttackPrompt : MonoBehaviour
{
    Image attackPromptImage;
    TurnManager turnManager;
    bool inButton = false;
    //public Button attackButton1;
    //public Button attackButton2;
    //public Button attackButton3;
    public List<Button> buttons = new List<Button>();
    private void Start()
    {
        attackPromptImage = this.gameObject.GetComponent<Image>();
        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();

        buttons.Add(GameObject.Find("Attack1").GetComponent<Button>()); 
        buttons.Add(GameObject.Find("Attack2").GetComponent<Button>()); 
        buttons.Add(GameObject.Find("Attack3").GetComponent<Button>());
        buttons.Add(GameObject.Find("Attack4").GetComponent<Button>());
        buttons[0].onClick.AddListener(Attack1);
        buttons[1].onClick.AddListener(Attack2);
        buttons[2].onClick.AddListener(Attack3);
        buttons[3].onClick.AddListener(Attack4);

    }

    void Attack1()
    {
        if(!turnManager.currentPlay.GetComponent<Movement>().getisMoving())
            turnManager.endTurn();
    }

    void Attack2()
    {
        if(!turnManager.currentPlay.GetComponent<Movement>().getisMoving())
            turnManager.currentPlay.GetComponent<Attack>().Attacking("Enemy");
        //Debug.Log("Attack 2");
    }

    void Attack3()
    {
        if(!turnManager.currentPlay.GetComponent<Movement>().getisMoving())
            turnManager.getEvent(3).Invoke();;
        //Quit();
        //Debug.Log("Attack 3");
    }

    void Attack4(){
        if(!turnManager.currentPlay.GetComponent<Movement>().getisMoving())
            turnManager.resetTurn();
    }



    public static void Quit()
     {
         #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
         #elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
         #else
         Application.Quit();
         #endif
     }
    public List<Button> getButtons(){
        return buttons;
    }
    private void Update()
    {
    }
    public void setInButton(bool inB){
        this.inButton = inB;
    }
    public bool checkOnButton(){
        return inButton;
    }

}
