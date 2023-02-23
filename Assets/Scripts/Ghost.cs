using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    SpriteRenderer Ghost_render;
    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSprite(Sprite s){
        Ghost_render = this.gameObject.GetComponent<SpriteRenderer>();
        Ghost_render.sprite = s;
        Ghost_render.color = new Color(1f,1f,1f,.5f);
    }
    public void setLocation(Vector3Int pos){
        transform.position = pos;
    }
    public void setOnOff(bool active){
        this.enabled = active;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = active;
    }
}
