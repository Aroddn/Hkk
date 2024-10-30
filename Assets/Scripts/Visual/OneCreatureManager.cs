using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class OneCreatureManager : MonoBehaviour, IPointerClickHandler
{
    public CardAsset cardAsset;
    public OneCardManager PreviewManager;
    public CreatureLogic creatureLogic;
    [Header("Text Component References")]
    public TMP_Text HealthText;
    public TMP_Text AttackText;
    public TMP_Text ManaCostText;
    [Header("Image References")]
    public Image CreatureGraphicImage;
    public Image CreatureGlowImage;
    [Header("UI References")]
    public GameObject rightClickMenu;

    void Awake()
    {
        if (cardAsset != null)
            ReadCreatureFromAsset();
    }

    private bool canAttackNow = false;
    public bool CanAttackNow
    {
        get
        {
            return canAttackNow;
        }

        set
        {
            canAttackNow = value;

            CreatureGlowImage.enabled = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Toggle the right-click menu
            if (rightClickMenu != null)
            {
                rightClickMenu.SetActive(!rightClickMenu.activeSelf);  
            }
        }
    }

    // Sacrifice the card (call this from the Sacrifice button)
    public void Sacrifice()
    {
        if (creatureLogic != null)
        {
            CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].Die(true);
        }
    }

    public void ReadCreatureFromAsset()
    {
        // Change the card graphic sprite
        CreatureGraphicImage.sprite = cardAsset.CardImage;

        AttackText.text = cardAsset.Attack.ToString();
        HealthText.text = cardAsset.MaxHealth.ToString();
        ManaCostText.text = cardAsset.ManaCost.ToString();

        if (PreviewManager != null)
        {
            PreviewManager.cardAsset = cardAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }	

    public void TakeDamage(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            //TODO
            DamageEffect.CreateDamageEffect(transform.position, amount);
            HealthText.text = healthAfter.ToString();
        }
    }

    public void Heal(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            //TODO
            HealEffect.CreateHealEffect(transform.position, amount);
            if (healthAfter > cardAsset.MaxHealth)
            {
                HealthText.text = cardAsset.MaxHealth.ToString();
            }
            else
            {
                HealthText.text = healthAfter.ToString();
            }
            
        }
    }
}
