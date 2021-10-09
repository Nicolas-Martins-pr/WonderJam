using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Controller : MonoBehaviour {

    public int m_side = 7; // must be odd

    public int m_nbTrees = 10;
    public int m_nbMountain = 10;
    public int m_nbRuins = 10;
    public int m_nbPortals = 2;
    public GameObject m_board;
    
    public Tile[] gameBoard;

    public List<Tile> m_gameBoardI;
    public CharacterControl m_player;
    public GameObject prefabTileG;
    public GameObject prefabTileW;

    private bool displayCalledOnce = false;


    private void Start() {
        SetAllBoardTiles();
        GenerateObstacleZone();
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
                newTile = Instantiate(prefabTileG,position,Quaternion.identity,this.m_board.transform);
                
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
                else if(j== 0 && i ==this.m_side - 1)
                {
                    newTile.name = "TileG_EndPlayer1 " + i + j;
                    newTileVar.m_endPlayer1 = true;
                    newTileVar.setGateHell(true);
                }
                else if (j== m_side-1 && i == m_side -1)
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
        int nbRotation =(int) (Random.value * 4);
        if (nbRotation == 0)
        {
            tile.transform.Rotate(new Vector3(0,90,0));
        }
        else if (nbRotation == 1)
        {
            tile.transform.Rotate(new Vector3(0,180,0));

        }
         else if (nbRotation == 2)
        {
            tile.transform.Rotate(new Vector3(0,270,0));

        }

    }
     void GenerateObstacleZone()
    {
        int nbtree = this.m_nbTrees, nbmountain = this.m_nbMountain, nbruin = this.m_nbRuins;
        int rand3 = (int)(Random.value * 3);

        int row = (int) (Random.value * m_side), column = (int) (Random.value * m_side); 
        while (nbtree != 0 || nbruin != 0 || nbmountain != 0)
        {
            row = (int) (Random.value * m_side);
            column = (int) (Random.value * m_side); 

            rand3 = (int)(Random.value * 3);
            if ((column == m_side/2 && row == 0) || (column == 0 && row == m_side -1) || (column == m_side -1 && row == m_side -1) ){}
            else
            {
                Tile tile = (m_gameBoardI[row*m_side + column]);
                if (tile.isWalkable())
                {
                    if (rand3 == 0)
                    {
                        if (nbtree != 0)
                        {
                            tile.setTree(true);
                            nbtree-=1;
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
                            nbmountain-=1;
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
                            nbruin-=1;
                        }
                        else
                        {
                            rand3 = 0;
                        }
                    }
                
                }
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