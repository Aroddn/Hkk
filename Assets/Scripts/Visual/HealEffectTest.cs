using UnityEngine;
using System.Collections;

public class HealEffectTest : MonoBehaviour {

    public GameObject HealPrefab;
    public static HealEffectTest Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            HealEffect.CreateHealEffect(transform.position, Random.Range(1, 7));

        Debug.Log("here");
    }
}
