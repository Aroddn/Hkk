using UnityEngine;
using System.Collections;

public class DamageEffectTest : MonoBehaviour {

    public GameObject DamagePrefab;
    public static DamageEffectTest Instance;

    void Awake()
    {
        Instance = this;
    }


    //for test
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //        DamageEffect.CreateDamageEffect(transform.position, Random.Range(1, 7));
    //}
}
