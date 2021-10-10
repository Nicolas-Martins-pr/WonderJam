using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
public class Tile : MonoBehaviour
{

    public int m_positionV;
    public int m_positionH;

    public int state= 0;
    public bool m_mountain;
    public bool m_ruin;
    public bool m_water;
    public bool m_tree;

    public bool m_portal;
    public bool m_player =false;
    public bool m_endPlayer1;
    public bool m_endPlayer2;

    public GameObject mountain;
    public List<GameObject> trees;
    public GameObject ruin;
    public GameObject portal;
    public Portal portalActif;

    public GameObject gateHeaven;
    public GameObject gateHell;
    public PhotonView pv;

    public bool m_active = false;
    public bool m_activePortal = false; // singleton for move one portal position
    public bool selectPortal = false;
    public bool m_activeSetObstacle = false;
    public bool m_activeRemoveObstacle = false;
    public List<Tile> m_AdjacentTiles = new List<Tile>();  //Group of different Tile free to move 
    public List<Tile> m_AdjacentTiles2 = new List<Tile>();  //Group of different Tile free to move distance == 2

    public List<Tile> m_AllObstacles = new List<Tile>();
    public List<Tile> m_AllWakable = new List<Tile>();

    public GameObject m_selectorIndicator;


    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
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

    public Portal GetPortal()
    {
        return this.portal.GetComponent<Portal>();
    }
    public bool hasMountain() 
    {
        return this.m_mountain;
    }

    public bool hasRuin()
    {
        return this.m_ruin;
    }

    public bool hasWater() 
    {
        return this.m_water;
    }

    public bool hasTree() 
    {
        return this.m_tree;
    }

    public bool hasPlayer() 
    {
        return this.m_player;
    }

    public bool hasPortal()
    {
        return this.m_portal;
    }

    public bool isWalkable()
    {
        return !this.hasMountain() && !this.hasWater() && !this.hasRuin() && !this.hasTree();
    }
    public List<Tile> GetAdjacentTiles()
    {
        return this.m_AdjacentTiles;
    }

    public bool isActive()
    {
        return this.m_active;
    }

    public void getState(){
        if(this.isWalkable())
        {
            this.state = 0;
        }
        else if (this.hasMountain())
        {
            this.state = 1;
        }
        else if (this.hasTree())
        {
            this.state = 2;
        }
        else if (this.hasRuin())
        {
            this.state = 3;
        }
    }

    public void setState(int _state){

        clearState();
        Debug.Log(_state);
        if(_state == 0){
            
        }
        else if (_state ==1)
        {
            this.setMountain(true);
        }
        else if (_state ==2)
        {
            this.setTree(true);
        }
        else if (_state ==3)
        {
            this.setRuin(true);
        }




    }
    public void clearState(){
        this.setMountain(false);
        this.setRuin(false);
        this.setTree(false);
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
    public void setMountain(bool value)
    {
        this.m_mountain = value;
        this.mountain.SetActive(value);
        
    }
    public void setRuin(bool value)
    {
        this.m_ruin = value;
        this.ruin.SetActive(value);
    }
    public void setPlayer(bool value)
    {
        this.m_player = value;
    }
    public void setWater(bool value)
    {
        this.m_water = !this.m_water;
    }
    public void setTree(bool value)
    {
        this.m_tree = value;
        int wTree = (int) (UnityEngine.Random.value * 2);
        this.trees[wTree].SetActive(value) ;
        if (value == false)
        {
            this.trees[0].SetActive(false);
            this.trees[1].SetActive(false);
        }
    }

    public void setPortal(bool value)
    {
        this.portal.SetActive(value);
        this.m_portal = value;
    }

    public void setActivePortal(bool value)
    {
        this.m_activePortal = value;
    }

    public void setPortalActif(Portal portal) // 
    {
        this.portalActif = portal;
    }

    public void setGateHeaven(bool value)
    {
        this.gateHeaven.SetActive(value);
    }
    public void setGateHell(bool value)
    {
        this.gateHell.SetActive(value);
    }

    public void setActive(bool value)
    {
        this.m_active = value;
    }
    #endregion

    #region Utils

    public void SetAdjacentTiles(List<Tile> boardtiles, int nbMove = 0) // optimisable
    {
        if (nbMove == 0)
        {
            foreach (Tile tile in boardtiles)
        {
            if (tile.getPositionH() == this.getPositionH() -1 && tile.getPositionV() == this.getPositionV() ||
                    tile.getPositionH() == this.getPositionH() +1 && tile.getPositionV() == this.getPositionV() ||
                    tile.getPositionV() == this.getPositionV() -1 && tile.getPositionH() == this.getPositionH()||
                    tile.getPositionV() == this.getPositionV() +1 && tile.getPositionH() == this.getPositionH())
            {
                this.m_AdjacentTiles.Add(tile);
            }
        }
        }
        else
        {
            foreach (Tile tile in boardtiles)
            {
                if (tile.getPositionH() == this.getPositionH() -1 &&
                        tile.getPositionV() == this.getPositionV() ||
                        tile.getPositionH() == this.getPositionH() +1 && tile.getPositionV() == this.getPositionV() ||
                        tile.getPositionV() == this.getPositionV() -1 && tile.getPositionH() == this.getPositionH() ||
                        tile.getPositionV() == this.getPositionV() +1 && tile.getPositionH() == this.getPositionH())
                {
                    this.m_AdjacentTiles2.Add(tile);
                }
                else if (tile.getPositionH() == this.getPositionH() -2 && tile.getPositionV() == this.getPositionV() ||
                        tile.getPositionH() == this.getPositionH() +2 && tile.getPositionV() == this.getPositionV() ||
                        tile.getPositionV() == this.getPositionV() -2 && tile.getPositionH() == this.getPositionH() ||
                        tile.getPositionV() == this.getPositionV() +2 && tile.getPositionH() == this.getPositionH())
                {
                    this.m_AdjacentTiles2.Add(tile);
                }
                else if (tile.getPositionH() == this.getPositionH() -1 && tile.getPositionV() == this.getPositionV() -1 ||
                        tile.getPositionH() == this.getPositionH() +1 && tile.getPositionV() == this.getPositionV() -1 ||
                        tile.getPositionV() == this.getPositionV() -1 && tile.getPositionH() == this.getPositionH() +1 ||
                        tile.getPositionV() == this.getPositionV() +1 && tile.getPositionH() == this.getPositionH()+1)
                {
                    this.m_AdjacentTiles2.Add(tile);
                }
            }
            getAllWalkable(boardtiles);
        }
        
        setSelectorIndicator(false);
    }
    public List<Tile> getMovements()
    {
        List<Tile> walkableTiles = new List<Tile>();
        List<Tile> tiles = this.GetAdjacentTiles();
        foreach (Tile tile in tiles)
        {
            if (tile.isWalkable())
            {
                walkableTiles.Add(tile);
                tile.setSelectorIndicator(true);
            }
            
        }
        return walkableTiles;
    }
    public List<Tile> getMovements2()
    {
        List<Tile> walkableTiles = new List<Tile>();
        foreach (Tile tile in this.m_AdjacentTiles2)
        {
            if (tile.isWalkable())
            {
                walkableTiles.Add(tile);
                tile.setSelectorIndicator(true);
            }
            
        }
        return walkableTiles;
    }


    public void getAllWalkable(List<Tile> boardtiles)
    {
        foreach (Tile tile in boardtiles)
        {
            if (tile.isWalkable() && (tile.getPositionH() != this.getPositionH() && tile.getPositionV() != this.getPositionV())){
                this.m_AllWakable.Add(tile);
                this.m_activeSetObstacle = true;
            }
            else
            {
                this.m_AllObstacles.Add(tile);
                this.m_activeRemoveObstacle = true;
            }
        }
    }

    public void setSelectorIndicator(bool active)
    {
        this.m_selectorIndicator.SetActive(active);
        this.setActive(active);
    }

    #endregion

    #region OnClick

    void OnMouseUp() 
    {
        
        
        if (this.selectPortal)
        {
            Controller.ctrl.desactivatePortalSelection();
            portal.GetComponent<Portal>().getMovementPortal();
        }
        else if (this.m_activeSetObstacle)
        {
            this.m_activeSetObstacle = false;
            this.setState(2);
            Controller.ctrl.updateState(this);
        }
        else if (this.m_activeRemoveObstacle)
        {
            this.m_activeRemoveObstacle = false;
            this.setState(0);
            Controller.ctrl.updateState(this);
        }
        else if (this.m_activePortal)
        {
            this.portalActif.SetCurrentTile(this);
            this.m_activePortal = false;
            
        }
        else if(this.isActive())
        {
            this.setPlayer(true);
            Controller.ctrl.MovePlayrRec(this);
        }
    }
    #endregion

    #region serialize
    public static object Deserialize(byte[] data)
    {

        byte[] hb = new byte[4];
        Array.Copy(data, 0, hb, 0, hb.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(hb);
        int h = BitConverter.ToInt32(hb,0);

        byte[] vb = new byte[4];
        Array.Copy(data, 4,vb, 0, vb.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(vb);
        int v = BitConverter.ToInt32(vb, 0);

        //Ajout Lucas
        byte[] sb = new byte[4];
        Array.Copy(data, 8, sb, 0, sb.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(sb);
        int s = BitConverter.ToInt32(sb, 0);
        Tile tile = Controller.ctrl.SetTileState(h, v, s);
        return tile;
    }

    public static byte[] Serialize(object obj)
    {

        var tile = (Tile)obj;
        // ajout lucas
        Debug.Log(tile.state);
        tile.getState();
        Debug.Log(tile.state);

        //
        byte[] h = BitConverter.GetBytes(tile.getPositionH());
        if (BitConverter.IsLittleEndian)
            Array.Reverse(h);

        byte[] v = BitConverter.GetBytes(tile.getPositionV());
        if (BitConverter.IsLittleEndian)
            Array.Reverse(v);
        //Ajout Lucas
        byte[] s = BitConverter.GetBytes(tile.state);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(s);
        //
        // Byte[] data = new byte[2*4];
        Byte[] data = new byte[3*4];

        return JoinBytes(JoinBytes(h,v),s);
    }

    private static byte[] JoinBytes(params byte[][] arrays)
    {
        byte[] rv = new byte[arrays.Sum(a => a.Length)];
        int offset = 0;
        foreach (byte[] array in arrays)
        {
            System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
            offset += array.Length;
        }
        return rv;
    }
    #endregion
}
