using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStatistics : MonoBehaviour {

    public static bool firstLaunch = true;
    public static int playerLives = 0;
    public static float timeSpent = 0;
    public static int kills = 0;
    public static int detected = 0;

    public float highestStealthStreak = 0;
    public float stealthStreak = 0;
    
    // Use this for initialization
	void Start () {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            firstLaunch = true;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1 && firstLaunch)
        {
            playerLives = 5;
            firstLaunch = false;
            kills = 0;
            detected = 0;
            timeSpent = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
        timeSpent += Time.deltaTime;
        DisplayOnUI();
    }

    public int GetPlayerLives()
    {
        return playerLives;
    }

    public void DecPlayerLife()
    {
        playerLives -= 1;
    }

    public void IncPlayerLife()
    {
        playerLives += 1;
    }

    public void IncDetected()
    {
        detected += 1;
    }

    public void IncKills()
    {
        kills += 1;
    }

    /* TEMPORARY */
    public void DisplayOnUI()
    {
        if(GameObject.Find("LivesCount") != null)
            GameObject.Find("LivesCount").GetComponent<Text>().text = "Lives: " + playerLives.ToString();
        if (GameObject.Find("TimeSpent") != null)
            GameObject.Find("TimeSpent").GetComponent<Text>().text = "Time: " + ((int)timeSpent).ToString();
    }

}
