using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SmoothCam : MonoBehaviour
{
    
    private Vector3 velocity = Vector3.zero;
    public GameObject spawnPoint;
    int mDelta = 10;
    public float timeRemaining = 5;
   
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine(test());

        
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                if (Input.mousePosition.y >= Screen.height - mDelta)
                {

                    transform.position = Vector3.SmoothDamp(transform.position,
                        GameSetup.GS.spawnPoints[0].transform.position, ref velocity, 2);
                    //transform.position = Vector3.MoveTowards(transform.position,GameSetup.GS.spawnPoints[0].transform.position,mDelta);
                }

                if (Input.mousePosition.y <= 0 + mDelta)
                {
                    // Move the camera
                    transform.position = Vector3.SmoothDamp(transform.position,
                        GameSetup.GS.spawnPoints[1].transform.position, ref velocity, 2);
                    //transform.position = Vector3.MoveTowards(transform.position,GameSetup.GS.spawnPoints[1].position,mDelta);
                }
            }

            IEnumerator test()
        {
            if (transform.position != spawnPoint.transform.position)
            {
                transform.position =
                    Vector3.SmoothDamp(transform.position, spawnPoint.transform.position, ref velocity, 2);
                //transform.position = Vector3.MoveTowards(transform.position,spawnPoint.transform.position,Time.deltaTime);

            }

            yield break;
        }
    }
}
    