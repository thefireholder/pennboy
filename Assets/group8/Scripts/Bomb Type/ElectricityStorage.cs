using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ElectricityStorage : MonoBehaviour
{
    public static ElectricityStorage Instance { get; private set; }

    public GameObject electricity;
    public int n_electricity = 10;

    [SerializeField]
    List<GameObject> electricitys;

    int index = 0;

    void Awake()
    {
        // Check if there is already an instance of this class
        if (Instance == null)
        {
            Instance = this;   // Set this instance as the singleton instance
            DontDestroyOnLoad(gameObject); // Optional: keep it between scenes

            // prepare electricity
            ReadyElectricity();
        }
        else
        {
            Destroy(gameObject); // Destroy this object if an instance already exists
        }

    }


    //Start is called before the first frame update
    public void ReadyElectricity()
    {
        for (int i = 0; i < n_electricity; i++)
        {
            GameObject l = Instantiate(electricity);
            l.SetActive(false);
            electricitys.Add(l);
        }
    }

    public void EffectElectricity(GameObject enemy)
    {
        // get tuple of affected enemies
        List<(GameObject, GameObject)> pairOfEnemies = ChooseEnemies(enemy);

        // create elctric ar between them
        foreach ((GameObject first, GameObject second) in pairOfEnemies)
        {
            CreateElectricity(index, first, second, true);
            index += 1;
        }
        
    }


    public List<(GameObject, GameObject)> ChooseEnemies(GameObject enemy)
    {
        /* 
         * get closest enemies around me
         * choose one depending on 
         */

        //return:
        List<(GameObject, GameObject)> gameObjectPairs = new List<(GameObject, GameObject)>();

        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        HashSet<GameObject> setEnemies = new HashSet<GameObject>();
        foreach (GameObject other in allEnemies) setEnemies.Add(other);

        int n_split = 3;
        int i_split = 1;
        int n_trav = 5;
        int n_consider = 2;

        while (n_trav > 0)
        {
            Vector3 enemyPos = enemy.transform.position;
            List<GameObject> filtered = setEnemies
                .Where(obj => obj.transform.position.y > enemyPos.y)
                .OrderBy(obj => Vector3.Distance(enemyPos, obj.transform.position))
                .Take(n_consider)
                .ToList();
            if (filtered.Count == 0) break;
            GameObject chosen = filtered[Random.Range(0, Mathf.Min(n_consider, filtered.Count))];
            gameObjectPairs.Add((enemy, chosen));
            setEnemies.Remove(chosen);
            enemy = chosen;
            n_trav -= 1;
        }

        return gameObjectPairs;
    }

    public void CreateElectricity(int index, GameObject first, GameObject second, bool straight)
    {
        Debug.Log("index " + index);
        electricitys[index].SetActive(true);

        Transform electricity = electricitys[index].transform;

        Transform Pos1 = electricity.Find("Pos1");
        if (Pos1 == null) Debug.Log("pos1, ", Pos1);
        if (first == null) Debug.Log("first, ", first);
        Pos1.SetParent(first.transform);
        Pos1.localPosition = Vector3.zero;

        Transform Pos4 = electricity.Find("Pos4");
        Pos4.SetParent(second.transform);
        Pos4.localPosition = Vector3.zero;

        if (straight)
        {
            Transform Pos2 = electricity.Find("Pos2");
            Pos2.SetParent(first.transform);
            Pos2.localPosition = Vector3.zero;

            Transform Pos3 = electricity.Find("Pos3");
            Pos3.SetParent(second.transform);
            Pos3.localPosition = Vector3.zero;
        }
    }


    //public void FindNearbyEnemy(float radius, List<GameObject> filteredEnemies)
    //{

    //    // get all enemies
    //    if (filteredEnemies == null)
    //        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    //        filteredEnemies = new List<GameObject>();
            

    //    // then filter by height
    //    foreach (GameObject enemy in allEnemies)
    //    {
    //        // Check height on y-axis
    //        if (enemy.transform.position.y <= maxHeight)
    //        {
    //            // Calculate the distance on the xz-plane from the origin
    //            Vector3 positionXZ = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
    //            float distanceFromOriginXZ = positionXZ.magnitude;

    //            // Check if the distance is within the radius threshold
    //            if (distanceFromOriginXZ <= radiusThreshold)
    //            {
    //                filteredEnemies.Add(enemy);
    //            }
    //        }
    //    }
        
    //    // then filter by nearby

    //    return ;
    //}
}
