using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class OneCardManager : MonoBehaviour {

    public CardAsset cardAsset;
    public OneCardManager PreviewManager;
    [Header("Text Component References")]
    public TMP_Text ManaCostText;
    public TMP_Text HealthText;
    public TMP_Text AttackText;
    [Header("Image References")]
    public Image CardGraphicImage;
    public Image CardFaceGlowImage;

    void Awake()
    {
        if (cardAsset != null)
            ReadCardFromAsset();
    }

    private bool canBePlayedNow = false;
    public bool CanBePlayedNow
    {
        get
        {
            return canBePlayedNow;
        }

        set
        {
            canBePlayedNow = value;

            CardFaceGlowImage.enabled = value;
        }
    }

    public void ReadCardFromAsset()
    {
        ManaCostText.text = cardAsset.ManaCost.ToString();
        CardGraphicImage.sprite = cardAsset.CardImage;

        if (cardAsset.MaxHealth != 0)
        {
            // this is a creature
            AttackText.text = cardAsset.Attack.ToString();
            HealthText.text = cardAsset.MaxHealth.ToString();
        }

        if (PreviewManager != null)
        {
            PreviewManager.cardAsset = cardAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }
}
