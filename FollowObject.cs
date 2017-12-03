using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
	    target = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position - new Vector3(0,0,1);
	}
}
