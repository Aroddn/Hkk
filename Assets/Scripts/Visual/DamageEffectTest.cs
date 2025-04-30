using UnityEngine;
using System.Collections;

public class DamageEffectTest : MonoBehaviour {

    public GameObject DamagePrefab;
    public static DamageEffectTest Instance;

    void Awake()
    {
        Instance = this;
    }
}
