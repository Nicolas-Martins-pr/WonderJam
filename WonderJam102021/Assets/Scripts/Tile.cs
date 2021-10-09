using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public int m_positionV;
    public int m_positionH;
    public bool m_mountain;
    public bool m_water;
    public bool m_trap;
    public bool m_player =false;
    public bool m_endPlayer1;
    public bool m_endPlayer2;

    public bool m_active = false;
    public List<Tile> m_nextTiles = new List<Tile>();  //Group of different Tile free to move 

    public GameObject m_selectorIndicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Accessors

    public int getPositionH() 
    {
        return this.m_positionH;
    }

    public int getPositionV() 
    {
        return m_positionV;
    }

    public int[] getPosition()
    {
        int[] position = new int[2] {this.getPositionH(),this.getPositionV()};
        return position;
    }
    public bool hasMountain() 
    {
        return this.m_mountain;
    }

    public bool hasWater() 
    {
        return this.m_water;
    }

    public bool hasTrap() 
    {
        return this.m_trap;
    }

    public bool hasPlayer() 
    {
        return this.m_player;
    }

    public bool isWalkable()
    {
        return !this.hasMountain() && !this.hasWater();
    }
    public List<Tile> getNextTiles()
    {
        return this.m_nextTiles;
    }

    public bool isActive()
    {
        return this.m_active;
    }
    #endregion

    #region Mutators

    public void setPositionH(int value)
    {
        this.m_positionH = value;
    }
    public void setPositionV(int value)
    {
        this.m_positionV = value;
    }
    public void setMountain()
    {
        this.m_mountain = !this.m_mountain;
    }
    public void setPlayer(bool value)
    {
        this.m_player = value;
    }
    public void setWater()
    {
        this.m_water = !this.m_water;
    }
    public void setTrap()
    {
        this.m_trap = !this.m_trap;
    }

    public void setActive()
    {
        this.m_active = !this.m_active;
        Debug.Log("active");
    }
    #endregion

    #region Utils

    public void SetAdjacentTiles(List<Tile> boardtiles) // optimisable
    {
        foreach (Tile tile in boardtiles)
        {
            if (tile.getPositionH() == this.getPositionH() -1 && tile.getPositionV() == this.getPositionV() || tile.getPositionH() == this.getPositionH() +1 && tile.getPositionV() == this.getPositionV() || tile.getPositionV() == this.getPositionV() -1 && tile.getPositionH() == this.getPositionH()||tile.getPositionV() == this.getPositionV() +1 && tile.getPositionH() == this.getPositionH())
            {
                this.m_nextTiles.Add(tile);
            }
        }
    }
    public List<Tile> getMovements()
    {
        List<Tile> walkableTiles = new List<Tile>();
        List<Tile> tiles = this.getNextTiles();
        foreach (Tile tile in tiles)
        {
            if (tile.isWalkable())
            {
                walkableTiles.Add(tile);
                tile.ActivateSelectorIndicator();
                tile.setActive();
            }
            
        }
        return walkableTiles;
    }

    void ActivateSelectorIndicator()
    {   
        if (this.m_selectorIndicator.activeSelf)
        {
            this.m_selectorIndicator.SetActive(false);
        }
        else
            this.m_selectorIndicator.SetActive(true);
    }

    
 
    #endregion
    
    #region OnClick
         
    void OnMouseUp() 
    {
          
        if(isActive())
        {
            this.setPlayer(true);
            Debug.Log("PlayerTrue");  
        } 
    }


    #endregion
}
