using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public RawImage healthBar;
    public float maxHealth;
    private float currentHealth;
    public float barWidth;

    public RawImage healthDiff;
    public RawImage healthLoss;

    void Start()
    {
        maxHealth = this.gameObject.GetComponentInParent<StatUpdate>().getMaxHealth();;
        currentHealth = maxHealth;
        healthBar = this.gameObject.GetComponent<RawImage>();
        barWidth = healthBar.rectTransform.rect.width;

        // Find HealthDiff and HealthLoss children
        Transform healthDiffTransform = transform.Find("HealthDiff");
        Transform healthLossTransform = transform.Find("HealthLoss");

        // Get RawImage components
        healthDiff = healthDiffTransform.GetComponent<RawImage>();
        healthLoss = healthLossTransform.GetComponent<RawImage>();

        UpdateHealth();
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
        float healthLossValue = oldCurrentHealth - currentHealth;
        float healthDiffValue = maxHealth - oldCurrentHealth;

        float healthLossWidth = barWidth * (healthLossValue / maxHealth);
        float healthDiffWidth = barWidth * (healthDiffValue / maxHealth);

        healthLoss.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthLossWidth);
        healthDiff.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthDiffWidth);

        float healthDiffX = barWidth ;
        healthLoss.rectTransform.localPosition = new Vector2(currentWidth+healthLossWidth/2, 0f);
        healthDiff.rectTransform.localPosition = new Vector2(currentWidth+healthLossWidth/2, 0f);
        healthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);

        Color color = Color.Lerp(Color.red, Color.green, fillAmount);
        healthBar.color = color;
        healthDiff.color = Color.blue;
        healthLoss.color = Color.red;
    }
}
