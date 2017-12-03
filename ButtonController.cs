using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

    // Use this for initialization
    public int scene;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        SceneManager.LoadScene(scene);
    }

    public void Help()
    {
        SceneManager.LoadScene("Help");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
