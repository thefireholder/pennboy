using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotater : MonoBehaviour
{

    public float speed;
    public float radius;
    // Start is called before the first frame update

    GameObject mainCamera;

    void Start()
    {
        mainCamera = transform.GetChild(0).gameObject; // for optimizaton purpose do not put this inside update
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKey(KeyCode.A)){
            transform.Rotate(0, -speed*Time.deltaTime, 0); //(x, y ,z )

        }

        if (Input.GetKey(KeyCode.D)){
            transform.Rotate(0, speed*Time.deltaTime, 0); //(x, y ,z )


        }

        if (Input.GetKey(KeyCode.S)){

            mainCamera.transform.localPosition += new Vector3(0, radius*Time.deltaTime, -radius*Time.deltaTime); //(x, y ,z )

        }

        if (Input.GetKey(KeyCode.W)){

            if(mainCamera.transform.localPosition.y >= 24 && mainCamera.transform.localPosition.z <= -12){ 

                mainCamera.transform.localPosition += new Vector3(0, -radius*Time.deltaTime, radius*Time.deltaTime); //(x, y ,z )

            }


        }

        
    }
}
