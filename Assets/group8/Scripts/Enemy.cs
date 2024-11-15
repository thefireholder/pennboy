using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float phaseLength = 10f;
    public float climbHeight = 3f;
    public float initialClimbHeight = 3f;
    public float variation = 0.5f;
    public float initalVariation = 2f;
    private bool isFirstClimb = true;

    public float flyLength = 3f;
    public float flyDuration = 2f;
    private ScoreManager scoreManager;
    public Material[] materials;
    public int colorChoice = 0;
    public float health = 3f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        scoreManager = FindObjectOfType<ScoreManager>();
        //if (colorChoice == -1)
        //    ChooseColor(Random.Range(0,2));
        //else
        //ChooseColor(colorChoice);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClimbUp(float duration)
    {
        // should climb up every wave
        float upMagnitude = (climbHeight + Random.Range(-1f, 1f) * variation);
        if (isFirstClimb)
        {
            upMagnitude = (initialClimbHeight + Random.Range(-1f, 1f) * initalVariation);
            isFirstClimb = false;
        }   
        Vector3 upward = Vector3.up * upMagnitude;
        StartCoroutine(MoveOverTime(transform.position, transform.position + upward, duration));
    }

    public void TouchedByBomb(int type, float damage=3)
    {
        //Debug.Log("what type" + type);
        if (type == 4)
        {
            // electricity type
            if (ElectricityStorage.Instance != null)
                ElectricityStorage.Instance.EffectElectricity(gameObject);
        }
        else if (type == 5)
        {
            // ice type
            if (IceStorage.Instance != null)
                IceStorage.Instance.EffectIce(gameObject);
        }
        else
        {
            Damage(damage, type);
        }
    }

    public void Damage(float damage, int type=0)
    {
        // skip killing if health is already below. not good to trigger destroy twice
        if (health <= 0) return;

        health -= damage;
        if (health <= 0)
        {
            FlyAway();
        }
    }


    public void FlyAway()
    {
        // activated when affected by bomb; should pretty much fly away in death

        //StartCoroutine(MoveOverTime(transform.position, transform.position + Random.onUnitSphere * flyLength, flyDuration));
        //Destroy(gameObject);
        rb.isKinematic = false;
        rb.velocity = Random.onUnitSphere * flyLength;
        StartCoroutine(DelayedDeath(flyDuration));

        // add score
        int score = 100;
        if (scoreManager != null) scoreManager.addScore(score);
    }


    public void RushToCenter()
    {
        // called when enemy reached top
        // make the enemy run to the center, and get to the trap door in the middle
        Vector3 start = transform.position;
        Vector3 end = new Vector3(0,transform.position.y,0);
        float duration = 1.36f;
        float clipDuration = 0.36f;
        StartCoroutine(MoveOverTime(start, end, duration, clipDuration));
    }

    // Coroutine to move the object from startPosition to endPosition over the given duration
    IEnumerator MoveOverTime(Vector3 start, Vector3 end, float duration, float clipDuration = 0)
    {

        float elapsedTime = 0f;

        while (elapsedTime < (duration - clipDuration))
        {
            // Lerp position from start to end based on elapsed time
            transform.position = Vector3.Lerp(start, end, elapsedTime / duration);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches the exact end position after the movement
        if (clipDuration == 0) transform.position = end;

    }

    // Function to change the object's material
    public void ChooseColor(int choice=0)
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = materials[choice];
        }
        else
        {
            Debug.LogWarning("Renderer component not found on this GameObject.");
        }
    }

    IEnumerator DelayedDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        // it seems its possible to access destroyed enemy due to race condition, hence this delay
        //yield return new WaitForSeconds(delay + 1);
        //Destroy(gameObject);
    }
}
