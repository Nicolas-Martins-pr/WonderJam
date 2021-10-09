using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    //Todo list : création de map avec rotation aléatoire des objects qui compose le terrain
    public Tile m_tile; 
    public GameObject m_player;

    // public GameObject m_gameBoard;

    private bool m_clicked = false;


    // Start is called before the first frame update
    void Start()
    {
        
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

    public void setClicked(bool value)
    {
        this.m_clicked = value;
    }
    #endregion

    void OnMouseUp() 
    {
        Debug.Log("click");  
        this.setClicked(true);  
    }

    public void MoveAtPointerSelection(GameObject tile)
    {
        m_player.transform.position = tile.transform.position;
        setTile(tile.GetComponent<Tile>());
    }


}
