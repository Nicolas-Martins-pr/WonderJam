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
            int id = PhotonRoom.room.playerId-1;
            //Debug.Log(id);
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                GameSetup.GS.spawnPoints[id].position,
                GameSetup.GS.spawnPoints[id].rotation, 0);
            myAvatar.transform.parent = GameSetup.GS.spawnPoints[id].transform.parent;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
