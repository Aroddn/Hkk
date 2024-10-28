using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

/// <summary>
/// This class will show damage dealt to creatures or payers
/// </summary>

public class HealEffect : MonoBehaviour {

    // an array of sprites with different blood splash graphics
    public Sprite Splashe;

    // a UI image to show the blood splashes
    public Image HealImage;

    // CanvasGropup should be attached to the Canvas of this damage effect
    // It is used to fade away the alpha value of this effect
    public CanvasGroup cg;

    // The text component to show the amount of damage taken by target like: "-2"
    public TMP_Text AmountText;

    void Awake()
    {
        // pick a random image
        HealImage.sprite = Splashe; 
    }

    // A Coroutine to control the fading of this damage effect
    private IEnumerator ShowHealEffect()
    {
        // make this effect non-transparent
        cg.alpha = 1f;
        // wait for 1 second before fading
        yield return new WaitForSeconds(1f);
        // gradually fade the effect by changing its alpha value
        while (cg.alpha > 0)
        {
            cg.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        // after the effect is shown it gets destroyed.
        Destroy(this.gameObject);
    }
    /// <summary>
    /// Creates the damage effect.
    /// This is a static method, so it should be called like this: DamageEffect.CreateDamageEffect(transform.position, 5);
    /// </summary>
    /// <param name="position">Position.</param>
    /// <param name="amount">Amount.</param>
   
    public static void CreateHealEffect(Vector3 position, int amount)
    {
        // Instantiate a DamageEffect from prefab
        GameObject newHealEffect = new GameObject();
        newHealEffect = GameObject.Instantiate(HealEffectTest.Instance.HealPrefab, position, Quaternion.identity) as GameObject;
        // Get DamageEffect component in this new game object
        HealEffect de = newHealEffect.GetComponent<HealEffect>();
        // Change the amount text to reflect the amount of damage dealt
        de.AmountText.text = "+"+amount.ToString();
        // start a coroutine to fade away and delete this effect after a certain time
        de.StartCoroutine(de.ShowHealEffect());
    }
}
