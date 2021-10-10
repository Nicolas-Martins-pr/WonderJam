using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;
    public Hashtable myCards;
    public string camp;


    // Start is called before the first frame update
    void Start()
    {
        myCards = new Hashtable();
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            // define camp
            if (PhotonRoom.room.playerId == 1)
            {
                camp = "Heaven";
            }
            else
            {
                camp = "Hell";
            }

            // spawn cam and script
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                GameSetup.GS.spawnPoints[1].position,
                GameSetup.GS.spawnPoints[1].rotation, 0);
            myAvatar.transform.parent = GameSetup.GS.spawnPoints[1].transform.parent;

            // attach script
            GameTurnSystem.GTS.player = this;

            // spawn placholders
            string path = GameTurnSystem.GTS.player.camp + "/" + GameTurnSystem.GTS.player.camp + "_Border";
            GameObject PH = Resources.Load<GameObject>(path);

            for (int i = 0; i < 4; i++)
            {
                GameObject ph = Instantiate(PH, new Vector3(0, 0, 0), quaternion.identity);
                ph.transform.SetParent(GameTurnSystem.GTS.CardOrigin.GetChild(i));
                ph.transform.localPosition = Vector3.zero;
                ph.transform.localRotation = Quaternion.identity;
                ph.transform.localScale = new Vector3(1, 1, 1);
                GameTurnSystem.GTS.CardOrigin.GetChild(i).gameObject.SetActive(false);
            }

            //get firts cards
            Deck.deck.GiveCard();
        }
        
        
    }
    //return the key of the card, return -1 if hand is full
    public void addCard(GameObject card)
    {
        int key = -1;
        int i = 0;
        while (key ==-1 && i<4)
        {
            if (myCards.ContainsKey(i)==false)
            {
                myCards.Add(i, card);
                key = i;
                GameTurnSystem.GTS.CardOrigin.GetChild(key).gameObject.SetActive(true);
            }
            i++;
        }
        card.transform.SetParent(GameTurnSystem.GTS.CardOrigin.GetChild(key));
        card.transform.localPosition = Vector3.zero;
        if (camp == "Heaven")
            card.transform.localRotation = new Quaternion(0f, 180f, 0f,0f);
        else
            card.transform.localRotation = Quaternion.identity;
        card.transform.localScale = new Vector3(1,1,1);
    }

    public void removeCard(int cardKey)
    {
        GameTurnSystem.GTS.CardOrigin.GetChild(cardKey).gameObject.SetActive(false);
        GameObject card = (GameObject)myCards[cardKey];
        Deck.deck.DiscardCard(card);
        myCards.Remove(cardKey);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
