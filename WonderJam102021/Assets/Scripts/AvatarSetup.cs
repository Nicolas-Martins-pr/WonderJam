using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public Camera myCamera;
    public AudioListener myAL;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            Destroy(myCamera);
            Destroy(myAL);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
