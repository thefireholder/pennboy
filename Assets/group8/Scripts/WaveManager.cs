using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Phase { EnemyPhase, EnemyPhaseEnded, BombPhase, BombPhaseEnded, PlayerPhase, PlayerPhaseEnded };

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    int waveNumber = 0;

    public int maximumBomb = 10;
    public bool RequirePressBarForNextPhase = false;
    public TMP_Text scoreText;
    public TMP_Text bombText;
    public TMP_Text timerText;
    public TMP_Text gameStateText;
    public bool textReady = false; // depends on existance of text objects

    private int score = 0;

    [SerializeField]
    int[] numberOfSpawnEnemy = {2,2,2,3,3,3,4,4,4,5,5,5};

    private float playerPhaseLength = 25f;
    private float bombPhaseLength = 10f;
    private float enemyPhaseLength = 2f;
    private float bombCleanUpHeight = 3f;
    private OutwardForce bombPushingObject;
    private SpawnSurface[] spawnEnemySurfaces;
    private Throw hand;
    private OverflowDetector overflowDetector;
    private CameraView cameraView;
    private ScoreManager scoreManager;
    private float phaseStartedAt;
    
    

    // during player phase, player plays combining game
    // during bombphase, bomb starts falling
    // during enemy phase, enemy starts climbing

    public Phase currentPhase = Phase.EnemyPhaseEnded;

    // Start is called before the first frame update
    void Start()
    {
        bombPushingObject = FindObjectOfType<OutwardForce>();
        spawnEnemySurfaces = FindObjectsOfType<SpawnSurface>();
        hand = FindObjectOfType<Throw>();
        overflowDetector = FindObjectOfType<OverflowDetector>();
        cameraView = FindObjectOfType<CameraView>();
        scoreManager = FindObjectOfType<ScoreManager>();
        phaseStartedAt = Time.time;


        // display text only if text objects are all set correctly
        textReady = ((scoreText != null) &&
            (bombText != null) &&
            (timerText != null) &&
            (gameStateText != null));
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        int nBomb = 0;
        string timerValue = "x";

        // Check if space is pressed and the object is not already moving
        switch (currentPhase)
        {
            case Phase.EnemyPhaseEnded:
                if (!RequirePressBarForNextPhase || Input.GetKeyDown(KeyCode.Space))
                {
                    StartPlayerPhase();
                    
                }
                break;
            case Phase.PlayerPhaseEnded:
                if (!RequirePressBarForNextPhase || Input.GetKeyDown(KeyCode.Space))
                {
                    StartBombPhase();
                }
                break;
            case Phase.BombPhaseEnded:
                if (!RequirePressBarForNextPhase || Input.GetKeyDown(KeyCode.Space))
                {
                    StartEnemyPhase();
                }
                break;
        }

        // manually ending phase by condition
        if (currentPhase == Phase.PlayerPhase)
        {
            // calculate time left (+1 is so that the count down begins with maximum number 14.9s -> 15s
            timerValue = ((int)(playerPhaseLength - (currentTime - phaseStartedAt) + 1)).ToString();
            nBomb = GameObject.FindGameObjectsWithTag("Bomb").Length;
            if (nBomb == maximumBomb)
            {
                endPhase(Phase.PlayerPhase, "Bomb exceeded Length (" + nBomb + "/" + maximumBomb + ")");
                /* needs to be called if terminate Detection no longer used */
                if (overflowDetector != null) overflowDetector.TerminateDetection();
            }
            else if (currentTime > phaseStartedAt + playerPhaseLength)
            {
                endPhase(Phase.PlayerPhase, "Time is up");
                if (overflowDetector != null) overflowDetector.TerminateDetection();
            }
            else if (overflowDetector.BombOverflowDetected())
            {
                endPhase(Phase.PlayerPhase, "Overflow detected");
                if (overflowDetector != null) overflowDetector.TerminateDetection();
            }

        }
        if (currentPhase == Phase.BombPhase)
        {
            if (currentTime > phaseStartedAt + bombPhaseLength)
                endPhase(Phase.BombPhase, "Time is up");
        }
        if (currentPhase == Phase.EnemyPhase)
        {
            if (currentTime > phaseStartedAt + enemyPhaseLength)
                endPhase(Phase.EnemyPhase, "Time is up");
        }

        // handle scores
        if (scoreManager != null) score = scoreManager.getScore();
        if (textReady)
        {
            scoreText.text = "Score: " + score.ToString();
            bombText.text = "MAX Bomb: " + nBomb + " / " + maximumBomb;
            timerText.text = "Time: " + timerValue.ToString();
        }

    }

    void StartPlayerPhase()
    {
        /* runs exactly once at the start of enemy phase */
        currentPhase = Phase.PlayerPhase;
        phaseStartedAt = Time.time;
        Debug.Log("Player Phase started");

        // activate hand if it exists
        if (hand != null) hand.activateHand(true);
        if (cameraView != null) cameraView.ZoomIn();

        // get rid of bombs not on tower
        foreach (GameObject bomb in GameObject.FindGameObjectsWithTag("Bomb"))
            // code block to be executed
            if (bomb.transform.position.y < bombCleanUpHeight)
                Destroy(bomb);

        // start bomb overflow detection
        if (overflowDetector != null) overflowDetector.InitiateDetection();
    }

    void StartBombPhase()
    {
        /* runs exactly once at the start of enemy phase */
        currentPhase = Phase.BombPhase;
        phaseStartedAt = Time.time;
        Debug.Log("Bomb Phase started");

        // deactivate hand if it exists
        if (hand != null) hand.activateHand(false);
        if (cameraView != null) cameraView.ZoomOut();

        // push bomnbs off
        if (bombPushingObject != null)
        {
            bombPushingObject.pushBombsOff(bombPhaseLength * 2/3);
        }
    }

    void StartEnemyPhase()
    {
        /* runs exactly once at the start of enemy phase */
        currentPhase = Phase.EnemyPhase;
        phaseStartedAt = Time.time;
        Debug.Log("Enemy Phase started");

        // deactivate hand if it exists
        if (hand != null) hand.activateHand(false);

        // increase your wave track
        waveNumber += 1;

        // make enemy climb up
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().ClimbUp(enemyPhaseLength);
        }

        // spawn new enemy
        int nEnemy = numberOfSpawnEnemy[waveNumber];
        for (int i = 0; i < spawnEnemySurfaces.Length; i++)
            spawnEnemySurfaces[i].Spawn(nEnemy);
            
    }

    void endPhase(Phase phase, string why)
    {
        switch (phase)
        {
            case Phase.PlayerPhase:
                if (currentPhase == Phase.PlayerPhaseEnded)
                    Debug.Log("Player phase already ended, ignoring " + why);
                else
                {
                    currentPhase = Phase.PlayerPhaseEnded;
                    Debug.Log("Player phase ended " + why);
                }
                break;
            case Phase.EnemyPhase:
                if (currentPhase == Phase.EnemyPhaseEnded)
                    Debug.Log("Enemy phase already ended, ignoring " + why);
                else
                {
                    currentPhase = Phase.EnemyPhaseEnded;
                    Debug.Log("Enemy phase ended " + why);
                }
                break;
            case Phase.BombPhase:
                if (currentPhase == Phase.BombPhaseEnded)
                    Debug.Log("Bomb phase already ended, ignoring " + why);
                else
                {
                    currentPhase = Phase.BombPhaseEnded;
                    Debug.Log("Bomb phase ended " + why);
                }
                break;
        }
        
    }
}
