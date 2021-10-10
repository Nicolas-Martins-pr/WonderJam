using Photon.Pun;
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
    //                         0        1        2       3        4          5         6
    public enum cardName { One_Case, Two_Case, MoveTP, Create, Teleport, Earthquake, Joker,}
    public List<GameObject> myGameObjects;
    public GameObject discard;
    public PhotonView PV;
    private int i = 0;
    private GameObject card;

    
    void Start()
    {
        deck = this;
        PV = GetComponent<PhotonView>();
    }

    public void fillCards()
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
    
    private int getRandomCard()
    {
        int i = 0;
        int r = Random.Range(0, 50);
        Debug.Log(r);
        if(r < 24)
            i = 0;
        else if (r < 32)
            i = 2;
        else if (r < 32)
            i = 3;
        else if (r < 40)
            i = 4;
        else if (r < 44)
            i = 5;
        else if (r < 48)
            i = 6;
        return i;
    }
    public void GiveCard()
    {
        int limit;
        if (GameTurnSystem.GTS.player.myCards.Count <= 0)
        {
            limit = 3;
        }
        else if (GameTurnSystem.GTS.player.myCards.Count == 4)
        {
            limit = 0;
        }
        else
        {
            limit = 1;
        }
        for (int j = 0; j < limit; j++)
        {
            int r = getRandomCard();
            GameObject card = Instantiate(myGameObjects[r], new Vector3(0, 0, 0), quaternion.identity);
            GameTurnSystem.GTS.player.addCard(card);
        }
    }

    [PunRPC]
    public void RemoveCardFromDeck(int CardIndex, int playerId)
    {
        if (PhotonRoom.room.playerId == playerId)
        {
            GameObject card = this.transform.GetChild(CardIndex).gameObject;
            GameTurnSystem.GTS.player.addCard(card);
        }
        else
        {
            GameObject card = this.transform.GetChild(CardIndex).gameObject;
            DiscardCard(card);
        }
    }

    public void DiscardCard(GameObject card)
    {
        card.transform.SetParent(discard.transform);
        card.transform.localPosition = Vector3.zero;
    }

    public void DiscardToDeck()
    {
        //clean discard
        int nbcarddi = discard.transform.childCount;
        for (int j = 0; j < nbcarddi; j++)
        {
            Destroy(discard.transform.GetChild(0));
        }
        //clean deck
        int nbcardde = this.transform.childCount;
        for (int j = 0; j < nbcardde; j++)
        {
            Destroy(this.transform.GetChild(0));
        }
        //refill deck
        fillCards();
    }
}
