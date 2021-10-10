using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder : MonoBehaviour
{
    public bool isSelection = false;
    private void OnMouseUp()
    {
        if (isSelection)
        {
            GameTurnSystem.GTS.cancelCardRec();
        }
        else
        {
            int parentIndex = this.transform.parent.GetSiblingIndex();
            GameTurnSystem.GTS.selectCardRec(parentIndex);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
