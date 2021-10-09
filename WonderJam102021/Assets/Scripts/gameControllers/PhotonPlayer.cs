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
            Debug.Log(PhotonRoom.room.playerId);
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                GameSetup.GS.spawnPoints[PhotonRoom.room.playerId].position,
                GameSetup.GS.spawnPoints[PhotonRoom.room.playerId].rotation, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
