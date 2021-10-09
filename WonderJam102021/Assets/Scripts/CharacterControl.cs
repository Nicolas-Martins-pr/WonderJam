using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    //Todo list : création de map avec rotation aléatoire des objects qui compose le terrain
    public Tile m_tile; 
    public GameObject m_player;
    public GameObject m_character;
    public GameObject m_animator;
    public Animator animState;
    public Animator animMovement;
    public GameObject playerpos;

    // public GameObject m_gameBoard;

    private bool m_clicked = false;


    // Start is called before the first frame update
    void Start()
    {
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
    // ["south","east","north","ouest"]
    void StartMove(string direction)
    {
        animMovement.SetTrigger(direction);
        animState.SetBool("isWalking", true);
    }

    void FinishMove(string direction)
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
