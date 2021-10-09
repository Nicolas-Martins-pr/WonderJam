using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<GameObject> myGameObjects = new List<GameObject>();
    public Canvas can;
    void Start()
    {
        
        myGameObjects=Resources.LoadAll<GameObject>("Card").ToList();
        foreach (var pref in myGameObjects)
        {
            print(pref);
        }
        for (int i = 0; i < myGameObjects.Count; i++)
        {
            print("oui");
            GameObject card = Instantiate(myGameObjects[i], new Vector3(0,0,0), quaternion.identity);
            card.transform.SetParent(this.transform,false);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
