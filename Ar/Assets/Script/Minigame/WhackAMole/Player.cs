using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public static float Health;
    public float MaxHealth;
    public Slider HealthSlider;
    // Start is called before the first frame update
    void Start()
    {
        HealthSlider.maxValue = MaxHealth;
        HealthSlider.value = MaxHealth;
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthSlider.value = Health;
    }
}
