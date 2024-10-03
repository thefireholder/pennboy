using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReachingPlane : MonoBehaviour
{
    public bool enemyReachedPlane;

    // Start is called before the first frame update
    void Start()
    {
        enemyReachedPlane = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().RushToCenter();
            TriggerGameOver();
            Debug.Log("Enemy Reached Surface: Game Over");
        }
    }

    private void TriggerGameOver()
    {
        enemyReachedPlane = true;
    }
}
