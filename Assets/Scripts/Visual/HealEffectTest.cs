using UnityEngine;
using System.Collections;

public class HealEffectTest : MonoBehaviour {

    public GameObject HealPrefab;
    public static HealEffectTest Instance;

    void Awake()
    {
        Instance = this;
    }
}
