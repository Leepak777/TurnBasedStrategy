using UnityEngine;
using UnityEngine.UI;

public class BubbleBar : MonoBehaviour
{
    public RawImage healthBar;
    public float maxHealth;
    public float currentHealth;
    public float barWidth;
    StatUpdate chStat;
    float sv;
    float av;

    void Start()
    {
        chStat = this.gameObject.GetComponentInParent<StatUpdate>();
        maxHealth = chStat.getDictStats("mid")*10;
        currentHealth = maxHealth;
        healthBar = this.gameObject.GetComponent<RawImage>();
        barWidth = healthBar.rectTransform.rect.width;
        sv = chStat.getDictStats("mid")*2 + chStat.getDictStats("acu");
        sv = chStat.getDictStats("mid"); 
        UpdateHealth();
    }
    public void restore(){
        currentHealth +=  chStat.getDictStats("mid");
    }

    public void UpdateHealth()
    {
        float oldCurrentHealth = currentHealth;
        if(this.gameObject.GetComponentInParent<StatUpdate>() != null){
            currentHealth  = this.gameObject.GetComponentInParent<StatUpdate>().currentHealth;
            maxHealth = this.gameObject.GetComponentInParent<StatUpdate>().getMaxHealth();

            
        }
        else{
            currentHealth = 0;
            maxHealth = 1;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        float fillAmount = currentHealth / maxHealth;
        float currentWidth = barWidth * fillAmount;
        
        healthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);

        Color color = Color.Lerp(Color.red, Color.green, fillAmount);
        healthBar.color = color;
    }
}
