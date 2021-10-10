using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;



    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {

            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                GameSetup.GS.spawnPoints[0].position,
                GameSetup.GS.spawnPoints[0].rotation, 0);
            myAvatar.transform.parent = GameSetup.GS.spawnPoints[0].transform.parent;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
