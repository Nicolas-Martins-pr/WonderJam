using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public enum cardName {Cell, Jump, Portal,Trap, Joker}
    public List<GameObject> myGameObjects;
    private int i = 0;
    void Start()
    {
        foreach (var name in Enum.GetNames(typeof(cardName)))
        {
            GameObject localGO =Resources.Load<GameObject>("Card"+name);
            myGameObjects.Add(localGO);
            print(myGameObjects[i]);
            i++;
        }
        myGameObjects.RemoveAt(0);
        
        foreach (var obj in myGameObjects)
        {
            Debug.Log("test");
            GameObject card = Instantiate(obj, new Vector3(0,0,0), quaternion.identity);
            card.transform.SetParent(this.transform,false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
