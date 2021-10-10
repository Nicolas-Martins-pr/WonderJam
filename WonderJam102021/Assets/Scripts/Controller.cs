using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Controller : MonoBehaviour {

    public static Controller ctrl;

    public int m_side = 7; // must be odd

    public GameObject m_board;
    
    public Tile[] gameBoard;

    public List<Tile> m_gameBoardI;
    public CharacterControl m_character;
    public GameObject prefabTileG;
    public GameObject prefabTileW;

    private PhotonView PV;

    private bool displayCalledOnce = false;
    public  bool tileClicked = false;


    private void Start() {
        PV = GetComponent<PhotonView>();
        ctrl = this;
        SetAllBoardTiles();
        SetAllNextTiles();
    }

    private void Update() 
    {

    }

    #region Accessors

    CharacterControl GetPlayer()
    {
        return this.m_character;
    }

    public Tile GetTile(int h, int v)
    {
        Tile tile = null;
        foreach (Tile temp in m_gameBoardI)
        {
            if (temp.getPositionH().Equals(h) && temp.getPositionV().Equals(v))
                tile = temp;
        }
        return tile;
    }
    #endregion

    void SetAllBoardTiles()
    {
        Vector3 position = new Vector3(0,0,0);
        GameObject newTile;
        int half = (this.m_side - 1)/2; 
        for (int i = 0; i < this.m_side; i++)
        {
            for (int j = 0; j < m_side; j++)
            {
                position = new Vector3(j-half,0,i);
                newTile = Instantiate(prefabTileG,Vector3.zero, Quaternion.identity,this.m_board.transform);
                newTile.transform.SetParent(m_board.transform);
                newTile.transform.localPosition = position;
                newTile.name = "TileG " + i + j;
                Tile newTileVar = newTile.GetComponent<Tile>();
                newTileVar.m_positionH = i+1;
                newTileVar.m_positionV = j+1;
                // Instantiate()
                if(j== half && i == 0)
                {
                    newTile.name = "TileG_PlayerStart " + i + j;
                    newTileVar.setPlayer(true);
                    this.m_character.setTile(newTileVar);

                    
                }
                if(j== 0 && i ==this.m_side - 1)
                {
                    newTile.name = "TileG_EndPlayer1 " + i + j;
                    newTileVar.m_endPlayer1 = true;
                }
                if (j== m_side-1 && i == m_side -1)
                {
                    newTile.name = "TileG_EndPlayer2 " + i + j;
                    newTileVar.m_endPlayer2 = true;
                }
                this.m_gameBoardI.Add(newTileVar);
            }
        }
    }

    void SetAllNextTiles()
    {
        foreach (Tile tile in m_gameBoardI)
        {
            tile.SetAdjacentTiles(m_gameBoardI);
        }
    }

    public void DesactiveAllTileSelectorIndicator()
    {
        foreach (Tile tile in m_gameBoardI)
        {
            tile.setSelectorIndicator(false);
        }
    }
    
    public void MovePlayrRec(Tile tilePlayer)
    {
        PV.RPC("MovePlayer", RpcTarget.AllBuffered, tilePlayer);
    }

    [PunRPC]
    public void MovePlayer(Tile tilePlayer)
    {
        Vector3 posTile = tilePlayer.transform.position;
        Vector3 posCharacter = GetPlayer().transform.position;
        Vector3 dir = posTile - posCharacter;
        string direction = "";
        if (dir.Equals(new Vector3(1, 0, 0)))
        {
            direction = "east";
        }
        if (dir.Equals(new Vector3(0, 0, -1)))
        {
            direction = "south";
        }
        if (dir.Equals(new Vector3(-1, 0, 0)))
        {
            direction = "ouest";
        }
        if (dir.Equals(new Vector3(0, 0, 1)))
        {
            direction = "north";
        }
        Debug.Log(direction);
        GetPlayer().getTile().setPlayer(false);
        GetPlayer().setTile(tilePlayer);
        GetPlayer().StartMove(direction);
        ResetBeforeAction();
    }
    void ResetBeforeAction()
    {
        DesactiveAllTileSelectorIndicator();
        displayCalledOnce = false;
        GetPlayer().setClicked(false);
    }
}