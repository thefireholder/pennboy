using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameStorage : MonoBehaviour
{
    public static FlameStorage Instance { get; private set; }

    public GameObject flameVFX;

    [SerializeField]
    List<GameObject> flames = new List<GameObject>();

    void Awake()
    {
        // Check if there is already an instance of this class
        if (Instance == null)
        {
            Instance = this;   // Set this instance as the singleton instance
            DontDestroyOnLoad(gameObject); // Optional: keep it between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy this object if an instance already exists
        }
    }

    // Update is called once per frame
    public void EffectFlame(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().RemoveAllVFX();
        GenerateVFX(enemy.transform);

        // damage on Enemies
        enemy.GetComponent<Enemy>().flameBurn(3);
        enemy.GetComponent<Enemy>().TakeDamage(3);
    }


    void GenerateVFX(Transform target)
    {
        flames.RemoveAll(item => item == null);

        GameObject vfx = Instantiate(flameVFX);
        vfx.transform.SetParent(target);
        vfx.transform.localPosition = Vector3.zero;
        flames.Add(vfx);
    }

    void LateUpdate()
    {
        // Make the flame follow the position of the object without inheriting rotation
        foreach (GameObject flame in flames)
        {
            if (flame != null)
            {
                flame.transform.rotation = Quaternion.Euler(-90, 0, 0); // Always keep it upright
            }
        }
    }
}
