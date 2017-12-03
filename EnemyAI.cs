using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {

    public GameObject target;
    public GameObject gunOffset;
    public GameObject bullet;
    public GameStatistics gameStatistics;

    public Vector3 targetPosition;
    public Vector3 playerPosition;
    public List<Vector3> patrolRoute = new List<Vector3>();
    private LayerMask layermask = 1;

    public float speed;
    public float persueSpeed;
    public float alertSpeed;
    public float patrolSpeed;

    public float fovAngleStrong;
    public float fovAngleWeak;
    public float detectRange;
    public float raycastOffset;
    public float patrolRange;

    public int fireCount;
    public float fireCooldown;
    public float fireRate;

    public float alertPhaseCountdown;
    public float alertRate;
    public float alertCounter;
    public float alertRange;
    
    public bool isInSight;
    public bool newPatrolLocation = true;
    public bool isCamera;
    public float camaraRotateRate;

    /*  STATES

        Patrol -
        Alert - 
        Persue -

    */

    public enum State
    {
        Patrol,
        Alert,
        Persue,
        Shoot,
        Disabled
    };

    public State state;

    // Use this for initialization
    void Start () {
        targetPosition = target.transform.position;
        state = State.Patrol;
        isInSight = false;
        InitialisePatrolRoute();
        gameStatistics = GameObject.Find("GAMESTATS").GetComponent<GameStatistics>();
    }
	
	// Update is called once per frame
	void Update () {

        if (state != State.Disabled)
        {
            if (fireCooldown < fireRate)
                fireCooldown += Time.deltaTime;

            if (alertPhaseCountdown > 0)
                alertPhaseCountdown -= Time.deltaTime;

            if (alertCounter <= 10)
                alertCounter += Time.captureFramerate;

            playerPosition = target.transform.position;
            FieldOfVision();
            AIState();
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if(state == State.Patrol)
        {
            if (!isCamera)
                newPatrolLocation = true;
        }
        if (state == State.Alert)
        {
            newPatrolLocation = true;
        }
    }

    public void AIState()
    {
        // Check if in sight
        if (isInSight)
        {
            if(fireCooldown >= fireRate)
            {
                fireCooldown = 0;
                Instantiate(bullet, gunOffset.transform.position, transform.rotation);
            } 
            state = State.Persue;
        } 
        else
        {
            if (alertPhaseCountdown > 0)
                if(!isCamera)
                    state = State.Alert;
            else
                state = State.Patrol;
        }         
        
        // AI Actions depending on state
        if(state == State.Persue) //Persue player if in line of sight
        {
            MovementToPosition();
        }
        if (state == State.Patrol) //Persue player if in line of sight
        {
            if(isCamera)
            {
                CameraRotations();
            }
            else
            {
                speed = patrolSpeed;
                Patrol();
            }
            
            //transform.Rotate(0, 0, Time.deltaTime*100);
        }
        if (state == State.Alert) //Persue player if in line of sight
        {
            speed = alertSpeed;
            Patrol();
            //transform.Rotate(0, 0, Time.deltaTime*100);
        }
    }


    public void MovementToPosition()
    {
        GetRotationPosition();
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void Patrol()
    {
        
        //IF reached patrol location, trigger new location
        if (transform.position == targetPosition)
        {
            newPatrolLocation = true;
        }
        
        //If new location, remove 1st, use new 1st
        //If 0, create new routes
        if(newPatrolLocation)
        {
            if(patrolRoute.Count == 1)    
                InitialisePatrolRoute();
            else
                patrolRoute.Remove(patrolRoute[0]);
            targetPosition = patrolRoute[0];
            newPatrolLocation = false;
        }

        MovementToPosition();
    }

    public void InitialisePatrolRoute()
    {
        patrolRoute = new List<Vector3>();
        patrolRoute.Add(
                new Vector3(
                Random.Range(transform.position.x - patrolRange, transform.position.x + patrolRange),
                Random.Range(transform.position.y - patrolRange, transform.position.y + patrolRange),
                transform.position.z)
            );
        for (int i = 0; i < 3; i++)
        {
            Vector3 temp = patrolRoute[i];
            patrolRoute.Add(
                new Vector3(
                Random.Range(temp.x - patrolRange, temp.x + patrolRange),
                Random.Range(temp.y - patrolRange, temp.y + patrolRange),
                temp.z)
            );
        }
    }

    public void GetRotationPosition()
    {

        Vector3 vectorToTarget = targetPosition - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
        transform.rotation = q;
    }

    public void SetMoveToPoint()
    {
        float move = Input.GetAxis("Fire1");
        if (move > 0)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GetRotationPosition();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                targetPosition.z = transform.position.z;
            }

        }
    }

    public void FieldOfVision()
    {
        Vector3 direction = playerPosition - transform.position;
        float angle = Vector3.Angle(direction, transform.right);

        if (angle < fovAngleStrong/2)
        {
            RaycastHit2D hit = Physics2D.Raycast(gunOffset.transform.position, direction, detectRange, layermask);
            
            if (hit)
            {               
                if (hit.collider.tag == "Player")
                {
                    isInSight = true;

                    speed = persueSpeed;
                    targetPosition = playerPosition;
                    SetAlert();

                    //Initiate Alert Phase.
                    if(alertCounter >= alertRate)
                    {
                        SetAlertStatus();
                        alertCounter = 0;
                    }

                    //GameStatus.currentState = GameStatus.GameState.Alert;
                    
                }

                if (hit.collider.tag == "Enemy")
                {
                    

                    //GameStatus.currentState = GameStatus.GameState.Alert;

                }

                if (hit.collider.tag == "Wall")
                {
                    isInSight = false;
                    if (alertPhaseCountdown <= 0)
                        state = State.Patrol;
                    else
                        state = State.Alert;
                }
                Debug.DrawRay(gunOffset.transform.position, playerPosition - transform.position, Color.blue);
            }
        }  
        else
        {
            isInSight = false;
            speed = 1;
        }           
    }

    public void SetAlertStatus()
    {
        SetAlert();
        int i = 0;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectRange);
        
        foreach(Collider2D item in hitColliders)
        {
            Debug.Log(item.name);
            if(item.tag == "Enemy")
            {
                item.gameObject.GetComponent<EnemyAI>().SetAlert();
            }
            
        }
    }

    public void SetAlert()
    {
        if (state != State.Disabled)
        {
            Debug.Log(gameObject.name);
            state = State.Alert;
            alertPhaseCountdown = 60;
        }
    }

    public void CameraRotations()
    {
        transform.Rotate(0, 0, Time.deltaTime * camaraRotateRate);
    }

}
