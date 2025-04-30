using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

//attached to the PlayerPortaits

public class PlayerPortraitVisual : MonoBehaviour {

    public CharacterAsset charAsset;
    [Header("Text Component References")]
    public Text HealthText;
    [Header("Image References")]
    public Image PortraitImage;
    public Image PortraitBackgroundImage;

    void Awake()
	{
		if(charAsset != null)
			ApplyLookFromAsset();
	}
	
	public void ApplyLookFromAsset()
    {
        HealthText.text = charAsset.MaxHealth.ToString();
        PortraitImage.sprite = charAsset.AvatarImage;
    }

    public void TakeDamage(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount);
            HealthText.text = healthAfter.ToString();
            if (healthAfter < charAsset.MaxHealth)
                HealthText.color = Color.red;
            else if(healthAfter > 20)
                HealthText.color = Color.green;
            else HealthText.color = Color.white;
        }
    }

    public void Heal(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            HealEffect.CreateHealEffect(transform.position, amount);
            if (healthAfter > charAsset.MaxHealth)
            {
                HealthText.text = charAsset.MaxHealth.ToString();
                HealthText.color = Color.white;
            }
            else
            {
                HealthText.text = healthAfter.ToString();
            }
        }
    }

    public void Explode()
    {
        Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
        Sequence s = DOTween.Sequence();
        s.PrependInterval(2f);
        s.OnComplete(() => GlobalSettings.Instance.GameOverCanvas.SetActive(true)); 
    }
}
