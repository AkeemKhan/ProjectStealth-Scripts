using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameState : MonoBehaviour {

    // Use this for initialization
    public Text gameStateLabel;
    public List<EnemyAI> listOfEnemies; 

    void Awake() {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
    }

	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        SetGameStateLabel();
    }

    public void SetGameStateLabel()
    {

        gameStateLabel.text = GameStatus.currentState.ToString();
        Debug.Log(GameStatus.currentState.ToString());
    }

}
