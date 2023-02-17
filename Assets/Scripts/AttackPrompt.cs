using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AttackPrompt : MonoBehaviour
{
    Image attackPromptImage;
    TurnManager turnManager;
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
        turnManager.currentPlay.GetComponent<Movement>().turn = false;
        turnManager.currentPlay.GetComponent<Movement>().moved = true;
        turnManager.currentPlay.GetComponent<Movement>().origin = false;
        //turnManager.currentPlay.GetComponent<Movement>().isMoving = false;
        //turnManager.currentPlay.GetComponent<Movement>().tilesTraveled = 0;
        //Debug.Log("Attack 1");
    }

    void Attack2()
    {
        turnManager.currentPlay.GetComponent<Movement>().Attack();
        //Debug.Log("Attack 2");
    }

    void Attack3()
    {
        //Debug.Log("Attack 3");
    }

    public List<Button> getButtons(){
        return buttons;
    }
    private void Update()
    {
        if (turnManager.player == true)
        {
            attackPromptImage.enabled = true;
        }
        else
        {
            attackPromptImage.enabled = false;
        }
    }
}
