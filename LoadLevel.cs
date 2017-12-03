using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    public float delay;
    public int levelNum;
    // Use this for initialization
    void Start () {
        Invoke("Load", delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Load()
    {
        SceneManager.LoadScene(levelNum);
    }
}
