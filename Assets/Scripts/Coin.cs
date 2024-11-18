using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private float timeToLive = 5f;  // Time before the coin disappears (5 seconds)

    // Start is called before the first frame update
    void Start()
    {
        // Start the countdown to destroy the coin after 'timeToLive' seconds
        Destroy(gameObject, timeToLive);
    }

    // Called when the coin collides with another object
    private void OnTriggerEnter2D(Collider2D whatDidIHit)
    {
        if (whatDidIHit.CompareTag("Player"))
        {
            // Add score when the player interacts with the coin
            GameObject.Find("GameManager").GetComponent<GameManager>().EarnScore(1);

            Destroy(this.gameObject); // Destroy the coin when it’s collected
        }
    }
}
