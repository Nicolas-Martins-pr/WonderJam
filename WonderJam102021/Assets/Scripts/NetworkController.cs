using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public static NetworkController lobby;

    public GameObject battlebutton;
    public GameObject waitinginfo;
    public GameObject cancelbutton;

    private void Awake()
    {
        lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();// Connect to master server
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("We are now connected to the "+ PhotonNetwork.CloudRegion+" server!");
        PhotonNetwork.AutomaticallySyncScene = true;
        battlebutton.SetActive(true);
    }

    public void onbattleButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
        battlebutton.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        waitinginfo.SetActive(true);
        cancelbutton.SetActive(true);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Tried to join a random game but failed. There must be no open games available");
        CreateRoom();
    }

    void CreateRoom()
    {
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2
        };
        PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Tried to create a new room but failed, there must already be no open games available");
        CreateRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onCancelButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
        battlebutton.SetActive(true);
        waitinginfo.SetActive(false);
        cancelbutton.SetActive(false);
    }
}
