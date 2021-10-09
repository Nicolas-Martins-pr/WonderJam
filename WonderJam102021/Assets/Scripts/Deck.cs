using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public static Deck deck;
    public enum cardName {Cell1, Cell2, TpMove, Create, Portal, Earthquake, Joker,}
    public List<GameObject> myGameObjects;
    private int i = 0;
    private GameObject card;
    
    void Start()
    {
        deck = this;
        foreach (var name in Enum.GetNames(typeof(cardName)))
        {
            GameObject localGO =Resources.Load<GameObject>("Card"+name);
            myGameObjects.Add(localGO);
            
        }
        //myGameObjects.RemoveAt(0);
        
       
        for (int j = 0; j < 50; j++)
        {
            if (j==13)
            {
                i++;
            }
            else if (j == 24)
            {
                i++;
            }else if (j==32)
            {
                i++;
            }else if(j==40)
            {
                i++;
            }else if (j==44)
            {
                i++;
            }else if (j==48)
            {
                i++;
            }
            card = Instantiate(myGameObjects[i], new Vector3(0,0,0), quaternion.identity);
            card.transform.SetParent(this.transform,false);
        }
    }
    
    public void GiveCard(Camera camera)
    {
        int limit;
        if (camera.transform.childCount <= 0)
        {
            limit = 3;
        }
        else
        {
            limit = 1;
        }
        for (int j = 0; j < limit; j++)
        {
            int r= Random.Range(0,this.transform.childCount);
            this.transform.GetChild(r).SetParent(camera.transform);
            //A changer pour les spawn dans les spawn point

        }
    }

    public void DiscardedToCard(GameObject discarded)
    {
        for (int j = 0; j < discarded.transform.childCount; j--)
        {
            int r= Random.Range(0,discarded.transform.childCount);
            discarded.transform.GetChild(r).SetParent(this.transform);
        }
        
    }

    public void toDiscard()
    {
        
    }
}
