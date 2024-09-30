using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotater : MonoBehaviour
{

    public float speed;
    public float radius; 
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject mainCamera = transform.Find("Main Camera").gameObject;

        if (Input.GetKey(KeyCode.LeftArrow)){
            transform.Rotate(0, -speed*Time.deltaTime, 0); //(x, y ,z )

        }

        if (Input.GetKey(KeyCode.RightArrow)){
            transform.Rotate(0, speed*Time.deltaTime, 0); //(x, y ,z )


        }

        if (Input.GetKey(KeyCode.DownArrow)){

            mainCamera.transform.localPosition += new Vector3(0, radius*Time.deltaTime, -radius*Time.deltaTime); //(x, y ,z )

        }

        if (Input.GetKey(KeyCode.UpArrow)){

            if(mainCamera.transform.localPosition.y >= 24 && mainCamera.transform.localPosition.z <= -12){ 

                mainCamera.transform.localPosition += new Vector3(0, -radius*Time.deltaTime, radius*Time.deltaTime); //(x, y ,z )

            }


        }

        
    }
}
