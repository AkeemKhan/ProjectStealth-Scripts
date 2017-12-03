using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISlider : MonoBehaviour {
    
    private Slider healthSlider;
    private PlayerStats playerStats;

	// Use this for initialization
	void Start () {
        healthSlider = transform.GetComponent<Slider>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
	}
	
	// Update is called once per frame
	void Update () {
        healthSlider.value = playerStats.health;
	}
}
