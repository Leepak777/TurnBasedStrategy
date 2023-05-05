using UnityEngine;
using UnityEngine.UI;

public class HealthBarPanel : MonoBehaviour
{
    public RawImage healthBar;
    public float maxHealth;
    private float currentHealth;
    public float barWidth;
    public GameObject currentGO;
    public Text indicator;
    public RawImage healthDiff;
    public RawImage healthLoss;
    public Vector2 loc;
    void Start()
    {
        healthBar = this.gameObject.GetComponent<RawImage>();
        barWidth = healthBar.rectTransform.rect.width;
        loc = healthBar.rectTransform.localPosition;
    }
    public void setCharacter(GameObject go){
        currentGO = go;
        maxHealth = go.GetComponent<StatUpdate>().getMaxHealth();;
        currentHealth = maxHealth;
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        float oldCurrentHealth = currentHealth;
        if(currentGO.GetComponentInParent<StatUpdate>() != null){
            currentHealth  = currentGO.GetComponentInParent<StatUpdate>().currentHealth;
            maxHealth = currentGO.GetComponentInParent<StatUpdate>().getMaxHealth();
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
        healthBar.rectTransform.localPosition = loc;
        Color color = Color.Lerp(Color.red, Color.green, fillAmount);
        healthBar.color = color;
        healthDiff.color = Color.blue;
        healthLoss.color = Color.red;
        indicator.text = currentHealth + " / " + maxHealth;
    }
}
