using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    private int[] m_positionP1; // coordonnées portail 1

    public Tile m_currentTile;

    public List<Tile> m_freePortalMovement = new List<Tile>();

    public GameObject m_board;

    public Portal m_ExitPortal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Accessors

    public Tile GetCurrentTile()
    {
        return this.m_currentTile;
    }

    public Portal getExitPortal()
    {
        return this.m_ExitPortal;
    }
        
    #endregion

    #region Mutators
    
     public void SetCurrentTile(Tile tile)
    {
        if (m_currentTile != null)
        {
            this.m_currentTile.setPortal(false);    // old portal removed
        }
           
        this.m_currentTile = tile;
        tile.setPortal(true);                       // new portal appear
        SetFreePortalMovement();
        Controller.ctrl.DesactiveAllTileSelectorIndicator();
    }
    #endregion

    public void ClearPortalMovement() 
    {
        this.m_freePortalMovement.Clear();
    }

    public void SetMovePortal()
    {
        foreach (Tile tile in m_board.GetComponent<Controller>().m_gameBoardI)
        {
            if (tile.getPositionH() == this.m_currentTile.getPositionH() -1 && tile.getPositionV() == this.m_currentTile.getPositionV() || tile.getPositionH() == this.m_currentTile.getPositionH() +1 && tile.getPositionV() == this.m_currentTile.getPositionV() || tile.getPositionV() == this.m_currentTile.getPositionV() -1 && tile.getPositionH() == this.m_currentTile.getPositionH() || tile.getPositionV() == this.m_currentTile.getPositionV() +1 && tile.getPositionH() == this.m_currentTile.getPositionH())
            {
                this.m_freePortalMovement.Add(tile);
            }
            else if (tile.getPositionH() == this.m_currentTile.getPositionH() -2 && tile.getPositionV() == this.m_currentTile.getPositionV() || tile.getPositionH() == this.m_currentTile.getPositionH() +2 && tile.getPositionV() == this.m_currentTile.getPositionV() || tile.getPositionV() == this.m_currentTile.getPositionV() -2 && tile.getPositionH() == this.m_currentTile.getPositionH() || tile.getPositionV() == this.m_currentTile.getPositionV() +2 && tile.getPositionH() == this.m_currentTile.getPositionH())
            {
                this.m_freePortalMovement.Add(tile);
            }
            else if (tile.getPositionH() == this.m_currentTile.getPositionH() -1 && tile.getPositionV() == this.m_currentTile.getPositionV() -1 || tile.getPositionH() == this.m_currentTile.getPositionH() +1 && tile.getPositionV() == this.m_currentTile.getPositionV() -1 || tile.getPositionV() == this.m_currentTile.getPositionV() -1 && tile.getPositionH() == this.m_currentTile.getPositionH() +1 || tile.getPositionV() == this.m_currentTile.getPositionV() +1 && tile.getPositionH() == this.m_currentTile.getPositionH()+1)
            {
                this.m_freePortalMovement.Add(tile);
            }
        }
    }

    public List<Tile> getMovementPortal()
    {
        List<Tile> walkableTiles = new List<Tile>();
        List<Tile> tiles = this.m_freePortalMovement;
        foreach (Tile tile in tiles)
        {
            if (tile.isWalkable() || !tile.hasPortal())
            {
                walkableTiles.Add(tile);
                tile.setSelectorIndicator(true);
                tile.setPortalActif(this);
            }
            
        }
        return walkableTiles;
    }


    public void SetFreePortalMovement() // must be call when portal move
    {
        ClearPortalMovement();
        SetMovePortal();
    }



    public Transform GetTransformExitPortal()
    {
        return this.m_ExitPortal.transform;
    }
}
