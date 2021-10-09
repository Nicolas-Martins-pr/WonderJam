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
    public bool m_player;
    public Tile[] m_nextTiles;  //Group of different Tile free to move 

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

    int getPositionH() 
    {
        return this.m_positionH;
    }

    int getPositionV() 
    {
        return m_positionV;
    }

    bool hasMountain() 
    {
        return this.m_mountain;
    }

    bool hasWater() 
    {
        return this.m_water;
    }

    bool hasTrap() 
    {
        return this.m_trap;
    }

    bool hasPlayer() 
    {
        return this.m_player;
    }

    bool isWalkable()
    {
        return !this.hasMountain() && !this.hasWater();
    }
    Tile[] getNextTiles()
    {
        return this.m_nextTiles;
    }

    #endregion

    #region Mutators

    void setPositionH(int value)
    {
        this.m_positionH = value;
    }
    void setPositionV(int value)
    {
        this.m_positionV = value;
    }
    void setMountain()
    {
        this.m_mountain = !this.m_mountain;
    }
    void setPlayer()
    {
        this.m_player = !this.m_player;
    }
    void setWater()
    {
        this.m_water = !this.m_water;
    }
    void setTrap()
    {
        this.m_trap = !this.m_trap;
    }
    #endregion

    #region Utils
    List<Tile> getMovements()
    {
        List<Tile> walkableTiles = new List<Tile>();
        Tile[] tiles = this.getNextTiles();
        for (int i = 0; i <  this.getNextTiles().Length; i++)
        {
            if (tiles[i].isWalkable())
            {
                walkableTiles.Add(tiles[i]);
                tiles[i].ActivateSelectorIndicator();
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
        Debug.Log("click");    
        this.getMovements();
    }


    #endregion
}
