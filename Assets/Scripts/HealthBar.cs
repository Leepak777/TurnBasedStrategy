using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public RawImage healthBar;
    public float maxHealth;
    private float currentHealth;
    public float barWidth;

    void Start()
    {
        maxHealth = this.gameObject.GetComponentInParent<StatUpdate>().getMaxHealth();;
        currentHealth = maxHealth;
        healthBar = this.gameObject.GetComponent<RawImage>();
        barWidth = healthBar.rectTransform.rect.width;
        UpdateHealth(currentHealth);
    }

    public void UpdateHealth(float health)
    {
        currentHealth = health;
        if(currentHealth < 0){
            currentHealth = 0;
        }
        

        float fillAmount = currentHealth / maxHealth;
        //healthBar.uvRect = new Rect(0f, 0f, fillAmount, 1f);
        float currentWidth = barWidth * fillAmount;

        healthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);
        if(currentHealth >= maxHealth*0.75f){
            healthBar.color = Color.green;
        }else if(currentHealth >= maxHealth*0.5f){
            healthBar.color = Color.yellow;
        }else{
            healthBar.color = Color.red;
        }
    }
}
