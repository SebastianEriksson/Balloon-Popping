using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{

    private float destroyTime = 3.3f;                                               // Define time until balloon destroys itself

    private float growthRate = 0.2f;                                                // Define how quickly the balloons grow
    private float scale = 0.1f;                                                     // Define balloon size when spawned

    private Spawning Spawning;                                                      // Link to the Spawning class
    public Transform newSpawnPoint;                                                 // Tell the game a new spawn point is available after the previous balloon has been popped

    private bool isDead = false;                                                    // Initialize the animator

    // Use this for initialization
    void Start()
    {
        // Find spawn points and start the destroy timers
        Spawning = GameObject.Find("BalloonSpawn").GetComponent<Spawning>();

        StartCoroutine(DestroyBalloon());
    }

    // Update is called once per frame
    void Update()
    {
        // If the player isn't dead do the following
        if (isDead == false)
        {
            // Define how the balloon will increase in size (speed etc)
            transform.localScale = Vector3.one * scale;

            scale += growthRate * Time.deltaTime;

            // If the player presses their mouse1 button down, pop balloon
            if (Input.GetMouseButtonDown(0))
            {
                // Make it so that only the balloon that the player is clicking gets destroyed
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == gameObject) Destroy(gameObject);

                    // Add score to the player with successful pop
                    BalloonControll.instance.PlayerScored();
                }

                // Make sure we tell the game that a spawn point is free again
                for (int i = 0; i < Spawning.spawnPoints.Length; i++)
                {
                    if (Spawning.spawnPoints[i] == newSpawnPoint)
                    {
                        Spawning.possibleSpawns.Add(Spawning.spawnPoints[i]);
                    }
                }

            }
            // If the player touches their screen, pop balloon
            if (Input.touchCount >= 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    // Make it so that only the balloon that the player is clicking gets destroyed
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject == gameObject) Destroy(gameObject);

                        // Add score to the player with successful pop
                        BalloonControll.instance.PlayerScored();
                    }

                    // Make sure we tell the game that a spawn point is free again
                    for (int i = 0; i < Spawning.spawnPoints.Length; i++)
                    {
                        if (Spawning.spawnPoints[i] == newSpawnPoint)
                        {
                            Spawning.possibleSpawns.Add(Spawning.spawnPoints[i]);
                        }
                    }
                }
            }
        }
    }

    // What to do when the timer reaches its value
    IEnumerator DestroyBalloon()
    {
        yield return new WaitForSeconds(destroyTime);

        // Make sure we tell the game that a spawn point is free again
        for (int i = 0; i < Spawning.spawnPoints.Length; i++)
        {
            if (Spawning.spawnPoints[i] == newSpawnPoint)
            {
                Spawning.possibleSpawns.Add(Spawning.spawnPoints[i]);
            }
        }
        Destroy(gameObject);

        // Remove score from the player if they don't pop it in time
        BalloonControll.instance.BalloonPoppedLate();
    }
}