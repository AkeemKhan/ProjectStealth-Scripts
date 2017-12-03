using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float damage;

	// Use this for initialization
	void Start () {
        transform.GetComponent<Rigidbody2D>().AddForce(transform.right * 10,ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        //transform.GetComponent<SpriteRenderer>().enabled = false;

        if (coll.transform.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerStats>().DamagerPlayer(damage);
        }

        Destroy(gameObject);
    }


}
