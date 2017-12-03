using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed = 0f;
    public float rotateSpeed;
    private float movex = 0f;
    private float movey = 0f;
    private GameStatistics gameStatistics;

    public int score = 0;
    public int winCond;

    public Vector3 clickPosition;

    public PlayerStats playerstats;
    bool sneak = false;

    // Use this for initialization
    void Start () {
        gameStatistics = GameObject.Find("GAMESTATS").GetComponent<GameStatistics>();
        playerstats = gameObject.GetComponent<PlayerStats>();
        clickPosition = transform.position;
        winCond = GameObject.FindGameObjectsWithTag("Victory").Length;
	}
	
	// Update is called once per frame
	void Update () {
        /*if (score == winCond)
            SceneManager.LoadScene(1);*/
	}

    void FixedUpdate()
    {
        SetMoveToPoint();
        Movement();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        clickPosition = transform.position;
        if (coll.transform.tag == "Enemy")
        {
            //Take Damage from Enemy
            if (coll.gameObject.GetComponent<EnemyAI>().isInSight)
            {
                playerstats.DamagerPlayer(50);
            }    
            //Destroy Enemy
            else
            {
                //Destroy(coll.gameObject);
                EnemyAI ai = coll.gameObject.GetComponent<EnemyAI>();
                ai.state = EnemyAI.State.Disabled;
                Rigidbody2D rb = coll.gameObject.GetComponent<Rigidbody2D>();
                Destroy(rb);
                coll.gameObject.GetComponent<SpriteRenderer>().color = Color.grey;

            }
                
        }
        if (coll.transform.tag == "Health")
        {
            Destroy(coll.gameObject);
            playerstats.HealPlayer(20);
            score++;
        }
        if (coll.transform.tag == "Exit")
        {
            GameObject.Find("GAMESTATS").GetComponent<GameStatistics>().IncPlayerLife();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("COLLIDER");
    }

    public void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, clickPosition, speed * Time.deltaTime);
    }

    public void GetRotationPosition()
    {

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
    }

    public void SetMoveToPoint()
    {
        float move = Input.GetAxis("Fire1");
        if (move > 0)
        {
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GetRotationPosition();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                clickPosition.z = transform.position.z;
            }

        }
    }

    public void SignalDefeat()
    {
        GameStatistics gs = GameObject.Find("GAMESTATS").GetComponent<GameStatistics>();
        Debug.Log("Lives :" + gs.GetPlayerLives().ToString() );
        gs.DecPlayerLife();
        if (gs.GetPlayerLives() > 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
            SceneManager.LoadScene(0);

    }


}
