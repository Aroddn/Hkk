using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


//creates random effect where the damage takes place

public class DamageEffect : MonoBehaviour {

    public Sprite[] Splashes;
    public Image DamageImage;
    public CanvasGroup cg;

    public TMP_Text AmountText;

    void Awake()
    {
        DamageImage.sprite = Splashes[Random.Range(0, Splashes.Length)];  
    }

    private IEnumerator ShowDamageEffect()
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
   
    public static void CreateDamageEffect(Vector3 position, int amount)
    {
        GameObject newDamageEffect = new GameObject();
        newDamageEffect = GameObject.Instantiate(DamageEffectTest.Instance.DamagePrefab, position, Quaternion.identity) as GameObject;
        DamageEffect de = newDamageEffect.GetComponent<DamageEffect>();
        de.AmountText.text = "-"+amount.ToString();
        de.StartCoroutine(de.ShowDamageEffect());
    }
}
