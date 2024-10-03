using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    /* 
     * bomb of same level can combine
     * Should create new bomb in its existant place
     * */

    // Constants
    public GameObject childBomb;

    

    // changeable variables
    public int level = 0;
    public float newBombPositionParameter = 0.5f; // must be between 0 and 1

    // private variable
    private float[]  bombSizes = new float[] { 1f, 1.5f, 2f, 2.5f, 3f };
    private System.DateTime born = System.DateTime.Now;
    public int parentsLevel = -1; // -1 means had no parent bomb
    ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        if (parentsLevel != -1)
            level = parentsLevel + 1;
        transform.localScale = Vector3.one * bombSizes[level];

        //Debug.Log("my level" + level);
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // general collision detection
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;

        //Debug.Log("something collided" + position);

        // Bomb collision detection
        if (collision.gameObject.tag == "Bomb")
        {
            Bomb collidedBomb = collision.gameObject.GetComponent<Bomb>();

            //Debug.Log("bombLevels " + level + " " + collidedBomb.level);

            // bomb of same level can combine
            if (level == collidedBomb.level)
            {
                // destroy both bomb and create new bomb in its place
                

                // only procreate when you are born first
                if (born < collidedBomb.born)
                {
                    // determine new position based on which one is born first
                    Vector3 bornFirstPosition = born < collidedBomb.born ? transform.localPosition : collidedBomb.transform.localPosition;
                    Vector3 bornSecondPosition = born > collidedBomb.born ? transform.localPosition : collidedBomb.transform.localPosition;
                    float alpha = newBombPositionParameter;
                    Vector3 newPosition = alpha * bornFirstPosition + (1 - alpha) * bornSecondPosition;

                    // create bomb and set its parentslevel
                    var newBomb = Instantiate(childBomb, newPosition, Quaternion.identity);
                    newBomb.GetComponent<Bomb>().parentsLevel = level;

                    // add score
                    int score = (int)Mathf.Pow(2,(level)) * 100;
                    scoreManager.addScore(score);
                }
                Destroy(gameObject);
                
            }

        }
        if (collision.gameObject.tag == "Enemy")
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, bombSizes[level] + 1);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "Enemy")
                {
                    hitCollider.GetComponent<Enemy>().TouchedByBomb(0);
                }
                Destroy(gameObject);
            }
        }
    }

    void Combine()
    {

    }


}
