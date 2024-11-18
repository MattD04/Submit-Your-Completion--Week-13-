using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float horizontalInput;
    private float verticalInput;
    private float speed;
    private float horizontalScreenLimit;
    private float verticalScreenLimit;
    public GameManager gameManager;

    public GameObject explosion;
    public GameObject bullet;
    public GameObject thruster;
    public GameObject shield;
    private int lives;
    private int shooting;
    private bool hasShield;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        speed = 6f;
        shooting = 1;
        hasShield = false;
        horizontalScreenLimit = 11.5f;
        verticalScreenLimit = 7.5f;
        lives = 3;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * speed);
        if (transform.position.x > horizontalScreenLimit || transform.position.x <= -horizontalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }
        if (transform.position.y > verticalScreenLimit || transform.position.y <= -verticalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (shooting)
            {
                case 1:
                    Instantiate(bullet, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(bullet, transform.position + new Vector3(0.5f, 1, 0), Quaternion.identity);
                    Instantiate(bullet, transform.position + new Vector3(-0.5f, 1, 0), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(bullet, transform.position + new Vector3(-0.5f, 1, 0), Quaternion.Euler(0,0,30f));
                    Instantiate(bullet, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    Instantiate(bullet, transform.position + new Vector3(0.5f, 1, 0), Quaternion.Euler(0, 0,-30f));
                    break;
            }
        } 
    }

    public void LoseALife()
    {
        // If the shield is active, lose the shield instead of a life.
        if (hasShield)
        {
            // Deactivate the shield and update hasShield to false.
            shield.gameObject.SetActive(false);
            hasShield = false;

            // Shield Lost Text
            gameManager.PlayPowerDown();
            gameManager.UpdatePowerupText("Shield Lost!");
        }
        else
        {
            // If there's no shield, lose a life.
            gameManager.DecreaseLives();
            lives--;

            // If lives are 0, trigger game over.
            if (lives == 0)
            {
                gameManager.GameOver();
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        speed = 6f;
        thruster.gameObject.SetActive(false);
        gameManager.PlayPowerDown();
        gameManager.UpdatePowerupText("");
    }
    IEnumerator ShootingPowerDown()
    {
        yield return new WaitForSeconds(3f);
        shooting = 1;
        gameManager.PlayPowerDown();
        gameManager.UpdatePowerupText("");
    }
    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if(whatIHit.tag == "PowerUp")
        {
                gameManager.PlayPowerUp();
            int powerupType = Random.Range(1, 5); //between 1 and 4
            switch(powerupType)
            {
                case 1:
                    //speed
                    speed = 9f;
                    gameManager.UpdatePowerupText("Picked up Speed!");
                    thruster.gameObject.SetActive(true);
                    StartCoroutine(SpeedPowerDown());
                    break;
                case 2:
                    //double shot
                    shooting = 2;
                    gameManager.UpdatePowerupText("Picked up Double Shot!");
                    StartCoroutine(ShootingPowerDown());
                    break;
                case 3:
                    //triple shit
                    shooting = 3;
                    gameManager.UpdatePowerupText("Picked up Triple Shot!");
                    StartCoroutine(ShootingPowerDown());
                    break;
                case 4:
                    //shield
                    gameManager.UpdatePowerupText("Picked up Shield!");
                    shield.gameObject.SetActive(true);
                    hasShield = true;
                    break;
            }
            Destroy(whatIHit.gameObject);
        }
    }

}
