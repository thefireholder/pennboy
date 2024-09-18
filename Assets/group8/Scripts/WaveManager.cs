using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase { EnemyPhase, EnemyPhaseEnded, BombPhase, BombPhaseEnded, PlayerPhase, PlayerPhaseEnded };

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    int waveNumber = 0;
    
    private float playerPhaseLength = 5f;
    private float bombPhaseLength = 5f;
    private float enemyPhaseLength = 5f;

    // during player phase, player plays combining game
    // during bombphase, bomb starts falling
    // during enemy phase, enemy starts climbing

    public Phase currentPhase = Phase.BombPhaseEnded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if space is pressed and the object is not already moving
        switch (currentPhase)
        {
            case Phase.EnemyPhaseEnded:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    currentPhase = Phase.PlayerPhase;
                    StartCoroutine(StartPlayerPhase());
                    
                }
                break;
            case Phase.PlayerPhaseEnded:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    currentPhase = Phase.BombPhase;
                    StartCoroutine(StartBombPhase());
                }
                break;
            case Phase.BombPhaseEnded:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    currentPhase = Phase.EnemyPhase;
                    StartCoroutine(StartEnemyPhase());
                }
                break;
        }
        
    }

    IEnumerator StartPlayerPhase()
    {
        Debug.Log("Player Phase started");
        yield return new WaitForSeconds(playerPhaseLength);
        currentPhase = Phase.PlayerPhaseEnded;
        Debug.Log("Player Phase ended");
    }

    IEnumerator StartBombPhase()
    {
        Debug.Log("Bomb Phase started");
        yield return new WaitForSeconds(bombPhaseLength);
        currentPhase = Phase.BombPhaseEnded;
        Debug.Log("Bomb Phase ended");
    }

    IEnumerator StartEnemyPhase()
    {
        Debug.Log("Enmey Phase started");

        // increase your wave track
        waveNumber += 1;

        // make enemy climb up
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().ClimbUp(enemyPhaseLength / 2);
        }

        // spawn new enemy


        // wait for certain seconds and change the state
        yield return new WaitForSeconds(enemyPhaseLength);
        currentPhase = Phase.EnemyPhaseEnded;
        Debug.Log("Enmey Phase ended");
    }
}
