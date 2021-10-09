using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    public GameObject m_tile; 
    public GameObject m_character;


    // Start is called before the first frame update
    void Start()
    {
        
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


}
