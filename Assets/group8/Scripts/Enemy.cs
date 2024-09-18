using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float phaseLength = 10f;
    public float climbHeight = 5f;

    public float flyLength = 3f;
    public float flyDuration = 2f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClimbUp(float duration)
    {
        // should climb up every wave
        Vector3 upward = Vector3.up * climbHeight;
        StartCoroutine(MoveOverTime(transform.position, transform.position + upward, duration));
    }

    public void TouchedByBomb(int type)
    {
        FlyAway();
    }



    public void FlyAway()
    {
        // activated when affected by bomb; should pretty much fly away in death

        //StartCoroutine(MoveOverTime(transform.position, transform.position + Random.onUnitSphere * flyLength, flyDuration));
        //Destroy(gameObject);
        rb.isKinematic = false;
        rb.velocity = Random.onUnitSphere * flyLength;
        StartCoroutine(DelayedDeath(flyDuration));

    }



    public void RushToCenter()
    {
        // called when enemy reached top
        // make the enemy run to the center, and get to the trap door in the middle
    }

    // Coroutine to move the object from startPosition to endPosition over the given duration
    IEnumerator MoveOverTime(Vector3 start, Vector3 end, float duration)
    {

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Lerp position from start to end based on elapsed time
            transform.position = Vector3.Lerp(start, end, elapsedTime / duration);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches the exact end position after the movement
        transform.position = end;

    }

    IEnumerator DelayedDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
