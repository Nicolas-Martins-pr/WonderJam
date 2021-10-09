using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room info
    public static PhotonRoom room;
    private PhotonView PV;
    public int multiplayerScene;

    public bool isGameLoaded;
    public int currentScene;
    public int playersInRoom;
    public int playerId;

    //Player info
    Player[] photonPlayers;

    private void Awake()
    {
        //set up singleton
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if(PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("joined room !");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        playerId = playersInRoom;
        PhotonNetwork.NickName = playerId.ToString();

        StartGame();
    }

    void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.LoadLevel(multiplayerScene);
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == multiplayerScene)
        {
            CreatePlayer();
        }
    }

    void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity,0);
    }
}
