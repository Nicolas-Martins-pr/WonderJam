using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    public GameObject m_platform; 
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

    private void OnMouseUp() {
        Debug.Log("click");    
    }

    void MoveAtPointerSelection()
    {
        // m_character.transform.position On click
    }


}
