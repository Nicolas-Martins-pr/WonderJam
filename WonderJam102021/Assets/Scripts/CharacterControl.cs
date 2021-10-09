using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    public GameObject m_tile; 
    public GameObject m_character;
    public GameObject m_animator;
    public Animator animState;
    public Animator animMovement;
    public GameObject playerpos;


    // Start is called before the first frame update
    void Start()
    {
        animState = m_animator.GetComponent<Animator>();
        animMovement = GetComponent<Animator>(); ;
        StartMove("north");
}

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Accessors
         
    #endregion

    #region Mutators
         
    #endregion

    void OnMouseUp() 
    {
        Debug.Log("click");    
    }

    void MoveAtPointerSelection(GameObject tile)
    {
        m_character.transform.position = tile.transform.position;
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
