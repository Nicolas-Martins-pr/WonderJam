using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviourPunCallbacks
{
    //Todo list : création de map avec rotation aléatoire des objects qui compose le terrain
    public Tile m_tile; 
    public GameObject m_character;
    public GameObject m_animator;
    public Animator animState;
    public Animator animMovement;
    public GameObject playerpos;
    public bool haswin;
    private PhotonView PV;

    // public GameObject m_gameBoard;

    private bool m_clicked = false;


    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        animState = m_animator.GetComponent<Animator>();
        animMovement = GetComponent<Animator>(); ;
}

    // Update is called once per frame
    void Update()
    {

    }

    #region Accessors

    public bool getPlayerClicked()
    {
        return this.m_clicked;
    }
         
    public Tile getTile()
    {
        return this.m_tile;
    }
    #endregion

    #region Mutators
    public void setTile(Tile tile) //set current tile
    {
        this.m_tile = tile;
    }
    [PunRPC]
    public void SetHasWin(bool value)
    {
        this.haswin = value;
    }
    #endregion
    
    public void setClicked(bool value)
    {
        this.m_clicked = value;
    }

    void OnMouseUp() 
    {
        if (!this.getPlayerClicked())
        {
            this.getTile().getMovements();
            this.setClicked(true);
        }
        else
        {
            Controller.ctrl.DesactiveAllTileSelectorIndicator();
            this.setClicked(false);
        }
    }

    [PunRPC]
    public void EndGame()
    {
        GameSetup.GS.playerUI.transform.GetChild(0).gameObject.SetActive(false);
        GameSetup.GS.playerUI.transform.GetChild(1).gameObject.SetActive(true);
        Text title = GameSetup.GS.playerUI.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
        Text Explenation = GameSetup.GS.playerUI.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Text>();
        Debug.Log(PhotonRoom.room.playersInRoom);
        if (haswin || PhotonRoom.room.playersInRoom < 2)
        {
            title.text = "VICTORY !!!";
            if (PhotonRoom.room.playersInRoom <2)
            {
                Explenation.text = "You oponnent has disconnected";
            }
            else
            {
                Explenation.text = "You reached your objective or the oponnent forfeited";
            }
        }
        else
        {
            title.text = "DEFEAT ...";
            Explenation.text = "Your oponnent reached his objective or you forfeited";
        }
    }

    public void Forfeit()
    {
        haswin = false;
        PV.RPC("SetHasWin", RpcTarget.OthersBuffered, true);
        PV.RPC("EndGame", RpcTarget.AllBuffered);
    }



    // ["south","east","north","ouest"]
    public void StartMove(string direction)
    {
        animMovement.SetTrigger(direction);
        animState.SetBool("isWalking", true);
    }

    public void FinishMove(string direction)
    {
        animState.SetBool("isWalking", false);
        Vector3 newpost = new Vector3(playerpos.transform.position.x, playerpos.transform.position.y, playerpos.transform.position.z);
        if (direction.Equals("east"))
        {
            newpost += new Vector3(1, 0, 0);
        }
        if (direction.Equals("south"))
        {
            newpost += new Vector3(0, 0, -1);
        }
        if (direction.Equals("ouest"))
        {
            newpost += new Vector3(-1, 0, 0);
        }
        if (direction.Equals("north"))
        {
            newpost += new Vector3(0, 0, 1);
        }
        playerpos.transform.position = newpost ;
        //StartMove("north");
    }

}
