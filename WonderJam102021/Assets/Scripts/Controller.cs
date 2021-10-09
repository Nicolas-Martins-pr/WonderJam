using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Controller : MonoBehaviour {

    public int m_side = 7; // must be odd

    public GameObject m_board;
    
    public Tile[] gameBoard;

    public List<Tile> m_gameBoardI;
    public CharacterControl m_player;
    public GameObject prefabTileG;
    public GameObject prefabTileW;

    private bool displayCalledOnce = false;


    private void Start() {
        SetAllBoardTiles();
        SetAllNextTiles();
    }

    private void Update() 
    {
        if(this.GetPlayer().getPlayerClicked())
        {
            if (!displayCalledOnce)
            this.GetPlayer().getTile().getMovements();
            this.displayCalledOnce = true;
        }

        if (this.displayCalledOnce)
        {
            Debug.Log("update");
            MovePlayer();
        }
    }

    #region Accessors

    CharacterControl GetPlayer()
    {
        return this.m_player;
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
                    this.m_player.setTile(newTileVar);

                    
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

    void DesactiveAllTileSelectorIndicator()
    {
        foreach (Tile tile in m_gameBoardI)
        {
            tile.DesactivateSelectorIndicator();
        }
    }

    void MovePlayer()
    {
        foreach (Tile tile in this.m_gameBoardI)
        {
            if (tile.hasPlayer() && (GetPlayer().getTile().getPositionH() != tile.getPositionH() || GetPlayer().getTile().getPositionV() != tile.getPositionV()))
            {
                Debug.Log("tiles" + tile.getPositionH() + tile.getPositionV());
                int child =(((int) tile.getPosition().GetValue(0)) -1)  * this.m_side + ((int) tile.getPosition().GetValue(1)) -1  ;
                GetPlayer().getTile().setPlayer(false);
                GetPlayer().MoveAtPointerSelection(this.transform.GetChild(child).gameObject);
                ResetBeforeAction();
            }
        }
    }
    void ResetBeforeAction()
    {
        DesactiveAllTileSelectorIndicator();
        displayCalledOnce = false;
        GetPlayer().setClicked(false);
    }
}