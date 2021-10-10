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
    public enum cardName { One_Case, Two_Case, MoveTP, Create, teleport, Earthquake, Joker,}
    public List<GameObject> myGameObjects;
    public GameObject discard;
    private int i = 0;
    private GameObject card;
    
    void Start()
    {
        deck = this;
    }

    private void fillCards()
    {
        foreach (var name in Enum.GetNames(typeof(cardName)))
        {
            GameObject localGO = Resources.Load<GameObject>(GameTurnSystem.GTS.player.camp+"/"+GameTurnSystem.GTS.player.camp + "_" + name);
            myGameObjects.Add(localGO);

        }


        for (int j = 0; j < 50; j++)
        {
            if (j == 13)
            {
                i++;
            }
            else if (j == 24)
            {
                i++;
            }
            else if (j == 32)
            {
                i++;
            }
            else if (j == 40)
            {
                i++;
            }
            else if (j == 44)
            {
                i++;
            }
            else if (j == 48)
            {
                i++;
            }
            card = Instantiate(myGameObjects[i], new Vector3(0, 0, 0), quaternion.identity);
            card.transform.SetParent(this.transform, false);
        }
    }
    
    public void GiveCard()
    {
        if (myGameObjects.Count == 0)
            fillCards();

        int limit;
        if (GameTurnSystem.GTS.player.myCards.Count <= 0)
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
            GameObject card = this.transform.GetChild(r).gameObject;
            GameTurnSystem.GTS.player.addCard(card);
            //A changer pour les spawn dans les spawn point

        }
    }

    public void DiscardCard(GameObject card)
    {
        card.transform.SetParent(discard.transform);
        card.transform.localPosition = Vector3.zero;
    }

    public void DiscardToDeck()
    {
        for (int j = 0; j < discard.transform.childCount; j++)
        {
            int r = Random.Range(0, discard.transform.childCount);
            discard.transform.GetChild(r).SetParent(this.transform);
        }
    }
}
