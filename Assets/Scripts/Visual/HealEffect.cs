using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HealEffect : MonoBehaviour {

    public Sprite Splashe;

    public Image HealImage;

    public CanvasGroup cg;

    public TMP_Text AmountText;

    void Awake()
    {
        HealImage.sprite = Splashe; 
    }
    private IEnumerator ShowHealEffect()
    {
        cg.alpha = 1f;
        yield return new WaitForSeconds(1f);
        while (cg.alpha > 0)
        {
            cg.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(this.gameObject);
    }
   
    public static void CreateHealEffect(Vector3 position, int amount)
    {
        GameObject newHealEffect = new GameObject();
        newHealEffect = GameObject.Instantiate(HealEffectTest.Instance.HealPrefab, position, Quaternion.identity) as GameObject;
        HealEffect de = newHealEffect.GetComponent<HealEffect>();
        de.AmountText.text = "+"+amount.ToString();
        de.StartCoroutine(de.ShowHealEffect());
    }
}
