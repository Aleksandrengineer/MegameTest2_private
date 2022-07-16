using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0F;
    public float rotateSpeed = 3.0F;
    public Transform upperBody;
    public Transform lowerBody;
    //public GameObject enemy;
    public GameObject text;
    public GameObject rifle;
    public GameObject sword;
    public bool isFinishingPossible = false;
    public GameObject enemyPrefab;
    private Animator _anim;
    private Vector3 _moveDirection;
    private Camera _mainCam;
    private float _mainCamOffset = -127.411f;
    private bool movingActive = false;
    private bool isExecuting = false;
    private bool isEnemyDead = false;
    private Vector3 targetPosition;
    private float rotationAngle;
    private GameObject enemy;
    private Transform target;

    void Start() 
    {
        _mainCam = Camera.main;
        this.transform.Rotate(0f, _mainCamOffset, 0f);
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        PlayerMovements();
        LookAtMousePosition();
        ExecuteFinishing();
        MovingToTarget();
        SpawnEnemy();
    }
    private void PlayerMovements()
    {
        PlayerController controller = GetComponent<PlayerController>();

        if (isExecuting == false)
        {
            // This is to control player movements
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            _moveDirection *= speed;
            controller.transform.Translate(_moveDirection);

            //this is to rotate object
            _moveDirection.Normalize();

            if (_moveDirection != Vector3.zero)
            {
                lowerBody.transform.forward = _moveDirection; //this is trasforming rotation of the lower body to the moving direction
                lowerBody.transform.Rotate(0f, _mainCamOffset, 0f); //this is aligning loverbody to the moving direction
                _anim.SetBool("isRunning", true);
            }
            else
            {
                _anim.SetBool("isRunning", false);
            }
        }
    }

    private void LookAtMousePosition()
    {
        if (isExecuting == false)
        {
            //This to look at mouse point
            //Get the Screen positions of the object
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
            
            //Get the Screen position of the mouse
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
            
            //Get the angle between the points
            float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

            //rotation
            //transform.rotation =  Quaternion.Euler (new Vector3(0f,-angle,0f));
            upperBody.transform.rotation = Quaternion.Euler (new Vector3(0, -angle + 100, -90));
        }

    }
 
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void ExecuteFinishing ()
    {
        if (isFinishingPossible == true && Input.GetKeyDown(KeyCode.Space) && isExecuting == false)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
            isExecuting = true;
            movingActive = true;

            
            _moveDirection = Vector3.zero; //This to stop player from moving so it would register the lower body rotation and moving through entire executing scene
            lowerBody.transform.forward = _moveDirection;
            lowerBody.transform.Rotate(0f, _mainCamOffset, 0f);

            targetPosition = enemy.transform.position - (enemy.transform.forward * 1.5f); //enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z - 1.5f setting up the position where
            isFinishingPossible = false;
            text.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        text.SetActive(true);
        isFinishingPossible = true;
    }
    private void OnTriggerExit(Collider other) 
    {
        isFinishingPossible = false;
        text.SetActive(false);
    }

    private void MovingToTarget()
    {
        if(movingActive == true)
        {
            var step = speed;
            target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
            transform.LookAt(target); //to look at enemy when moving toward its back to execute him
            transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step); //this is for moving player to the target position.
            _anim.SetBool("isRunning", true);   //setting animation to the running one during moving to the target

            ///TODO okay whenever the player is moving away form the enemy and press finish button it starting to moving to the position backwards.

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                movingActive = false;
                rifle.SetActive(false);
                sword.SetActive(true);
                _anim.SetTrigger("isFinishing");
            }
        
            if (transform.position == targetPosition)
            {
                //movingActive = false;
            }

        }
        }
    public void ResetRotation()
    {
        sword.SetActive(false);
        rifle.SetActive(true);
        isExecuting = false; 
        gameObject.transform.rotation = Quaternion.Euler (0f, _mainCamOffset, 0f); // to rotate object back so controller will be natural not inverted
        isEnemyDead = true;
    }

    public void EnemyDieForAnim()
    {
        EnemyDie();
        StartCoroutine(SpawnEnemy());
    }

    private void EnemyDie()
    {
        enemy.GetComponent<EnemyController>().die();
    }
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds (5f);
        Vector3 spawnPosition = new Vector3(Random.Range(-15f, 15f), 0f, Random.Range(-15f, 15f));
        Quaternion spawnRotation = Quaternion.Euler (0f, Random.Range(0,360), 0f);

        Instantiate(enemyPrefab, spawnPosition, spawnRotation);
    }
}

