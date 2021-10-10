using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Controller : MonoBehaviour {

    public static Controller ctrl;

    public int m_side = 7; // must be odd

    public int m_nbTrees = 10;
    public int m_nbMountain = 10;
    public int m_nbRuins = 10;
    public int m_nbPortals = 2;
    public GameObject m_board;

    public Portal m_portal1;
    public Portal m_portal2;

    public Tile[] gameBoard;

    public List<Tile> m_gameBoardI;
    public CharacterControl m_character;
    public GameObject prefabTileG;
    public GameObject prefabTileW;
    private Tile tile;

    private PhotonView PV;

    private bool displayCalledOnce = false;
    public bool tileClicked = false;


    private void Start() {
        PV = GetComponent<PhotonView>();
        ctrl = this;
        SetAllBoardTiles();
        SetAllNextTiles();
        GenerateObstacleZone();
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
        Vector3 position = new Vector3(0, 0, 0);
        GameObject newTile;
        int half = (this.m_side - 1) / 2;
        for (int i = 0; i < this.m_side; i++)
        {
            for (int j = 0; j < m_side; j++)
            {
                position = new Vector3(j - half, 0, i);
                newTile = Instantiate(prefabTileG, Vector3.zero, Quaternion.identity, this.m_board.transform);
                newTile.transform.SetParent(m_board.transform);
                newTile.transform.localPosition = position;
                newTile.name = "TileG " + i + j;
                Tile newTileVar = newTile.GetComponent<Tile>();
                newTileVar.m_positionH = i + 1;
                newTileVar.m_positionV = j + 1;
                // Instantiate()
                if (j == half && i == 0)
                {

                    newTile.name = "TileG_PlayerStart " + i + j;
                    newTileVar.setPlayer(true);
                    this.m_character.setTile(newTileVar);


                }
                else if (j == 0 && i == this.m_side - 1)
                {
                    newTile.name = "TileG_EndPlayer1 " + i + j;
                    newTileVar.m_endPlayer1 = true;
                    newTileVar.setGateHell(true);
                }
                else if (j == m_side - 1 && i == m_side - 1)
                {
                    newTile.name = "TileG_EndPlayer2 " + i + j;
                    newTileVar.m_endPlayer2 = true;
                    newTileVar.setGateHeaven(true);
                }
                else
                {
                    RotateTile(newTile);
                }
                this.m_gameBoardI.Add(newTileVar);
            }
        }
    }
    void RotateTile(GameObject tile)
    {
        int nbRotation = (int)(Random.value * 4);
        if (nbRotation == 0)
        {
            tile.transform.Rotate(new Vector3(0, 90, 0));
        }
        else if (nbRotation == 1)
        {
            tile.transform.Rotate(new Vector3(0, 180, 0));

        }
        else if (nbRotation == 2)
        {
            tile.transform.Rotate(new Vector3(0, 270, 0));

        }

    }

    public void DisplayMove()
    {
        m_character.getTile().getMovements();
    }

    public void DisplayMove2()
    {
        m_character.getTile().getMovements2();
    }

    public void selectPortalToMove()
    {
        m_portal1.m_currentTile.selectPortal = true;
        m_portal1.m_currentTile.setActive(true);

        m_portal1.m_currentTile.selectPortal = true;
        m_portal1.m_currentTile.setActive(true);
    }

    public void useJoker()
    {
        GameTurnSystem.GTS.jokerWindow.SetActive(true);
    }


    public void desactivatePortalSelection()
    {
        m_portal1.m_currentTile.selectPortal = false;
        m_portal1.m_currentTile.setActive(false);

        m_portal1.m_currentTile.selectPortal = false;
        m_portal1.m_currentTile.setActive(false);
    }
    public void GenerateObstacleZone() // Set all tree + mountains + ruins and the two portals
    {
        int nbtree = this.m_nbTrees, nbmountain = this.m_nbMountain, nbruin = this.m_nbRuins, nbportal = this.m_nbPortals;
        int rand3 = (int)(Random.value * 3);

        int row = (int)(Random.value * m_side), column = (int)(Random.value * m_side);

        Tile tile;
        while (nbtree != 0 || nbruin != 0 || nbmountain != 0)
        {
            row = (int)(Random.value * m_side);
            column = (int)(Random.value * m_side);

            rand3 = (int)(Random.value * 3);
            if ((column == m_side / 2 && row == 0) || (column == 0 && row == m_side - 1) || (column == m_side - 1 && row == m_side - 1)) { }
            else
            {
                tile = (m_gameBoardI[row * m_side + column]);
                if (tile.isWalkable())
                {
                    if (rand3 == 0)
                    {
                        if (nbtree != 0)
                        {
                            tile.setTree(true);
                            nbtree -= 1;
                        }
                        else
                        {
                            rand3 = 1;
                        }

                    }
                    if (rand3 == 1)
                    {
                        if (nbmountain != 0)
                        {
                            tile.setMountain(true);
                            nbmountain -= 1;
                        }
                        else
                        {
                            rand3 = 2;
                        }

                    }
                    if (rand3 == 2)
                    {
                        if (nbruin != 0)
                        {
                            tile.setRuin(true);
                            nbruin -= 1;
                        }
                        else
                        {
                            rand3 = 0;
                        }
                    }

                }
            }
        }


        // set portal while

        while (nbportal != 0)
        {
            row = (int)(Random.value * m_side);
            column = (int)(Random.value * m_side);

            if ((column == m_side / 2 && row == 0) || (column == 0 && row == m_side - 1) || (column == m_side - 1 && row == m_side - 1)) { }
            else
            {
                tile = (m_gameBoardI[row * m_side + column]);
                if (tile.isWalkable())
                {
                    if (this.m_portal1.m_currentTile == null)
                    {
                        this.m_portal1.SetCurrentTile(tile);
                        nbportal -= 1;
                    }
                    else
                    {
                        if (!tile.hasPortal())
                        {
                            this.m_portal2.SetCurrentTile(tile);
                            nbportal -= 1;
                        }

                    }
                }

            }
        }

    }

    public void earthquake()
    {
        foreach (var ti in m_gameBoardI)
        {
            ti.setMountain(false);
            ti.setRuin(false);
            ti.setTree(false);

        }
        GenerateObstacleZone();
        foreach (Tile tile in m_gameBoardI)
        {
            tile.getState();
            updateStateRec(tile);
        }
        ResetBeforeAction();
        if (GameTurnSystem.GTS.state == TurnState.PlayerTurn)
            GameTurnSystem.GTS.FinishTurn();
    }

    public void updateStateRec(Tile tile) {
        PV.RPC("updateState", RpcTarget.AllBuffered, tile);
    }

    [PunRPC]
    public void updateState(Tile tile) {
        tile.setState(tile.state);
    }

    public void cardCreateObstacle() {
        this.m_character.CreateObstacle();
    }

    public void cardRemoveObstacle()
    {
        this.m_character.CreateObstacle(1);
    }

    void SetAllNextTiles()
    {
        foreach (Tile tile in m_gameBoardI)
        {
            tile.SetAdjacentTiles(m_gameBoardI);
            tile.SetAdjacentTiles(m_gameBoardI, 1);
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

    public Tile SetTileState(int h, int v, int s)
    {
        Tile tile = GetTile(h, v);
        tile.clearState();
        if (s == 0)
        {

        }
        else if (s == 1)
        {
            tile.setMountain(true);
        }
        else if (s == 2)
        {
            tile.setTree(true);
        }
        else if (s == 3)
        {
            tile.setRuin(true);
        }
        tile.state =s;
        return tile;
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

   
    public void ResetBeforeAction()
    {
        DesactiveAllTileSelectorIndicator();
        displayCalledOnce = false;
        GetPlayer().setClicked(false);
    }
}