using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public enum TurnState {Start, PlayerTurn, Card, Wait}
public class GameTurnSystem : MonoBehaviour
{
    public static GameTurnSystem GTS;
    public TurnState state;
    public Text phase;
    public Text countdownText;
    public Text oponnentWaitText;
    public bool win;
    private Coroutine coInst = null;
    public Transform CardOrigin;
    public PhotonPlayer player;
    public int typecardSelected;
    public int oponnentTypeCardSelected;
    public PhotonView PV;
    public bool oponnentIsWaiting = false;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        GTS = this;
        SetupTurn();
        
    }

    // Update is called once per frame

    void SetupTurn()
    {
        typecardSelected = -1;
        oponnentTypeCardSelected = -1;
        state = TurnState.Start;
        phase.text = "Phase: " + state;
        coInst=StartCoroutine(Timer(10));
        //il faut implémenter l'animator pour terminer cette phase du jeu
    }

    void PlayCard()
    {
        state = TurnState.Card;
        phase.text = "Phase: "+state;
        coInst = StartCoroutine(Timer(120));
        Deck.deck.GiveCard(PhotonRoom.room.playerId);
        //Quand les deux joueurs on joué, faire le test de la win
        if (win)
        {
            StopCoroutine(coInst);
            state = TurnState.PlayerTurn;
            PlayCharacter();
            
        }
    }

    void waitOponnent()
    {
        state = TurnState.Wait;
        phase.text = "Phase: " + state;
    }

    public void PlayCharacter()
    {
        state = TurnState.PlayerTurn;
        phase.text = "Phase: "+state;
        coInst = StartCoroutine(Timer(120));
    }
    public void OnEndTurn()
    {
        if (state != TurnState.PlayerTurn)
        {
            phase.text = "Not your turn";
            return;
        }
        StopCoroutine(coInst);
        state = TurnState.Start;
        SetupTurn();
    }
    
    public void selectCardRec(int cardIndex)
    {
        if (state == TurnState.Card)
        {
            GameObject card = (GameObject)player.myCards[cardIndex];
            int cardValue = card.GetComponent<Card>().cardType;
            PV.RPC("selectCard", RpcTarget.AllBuffered, cardValue, PhotonRoom.room.playerId);
        }
    }


    [PunRPC]
    public void selectCard(int cardType, int playerId)
    {
        if (PhotonRoom.room.playerId == playerId)
        {
            typecardSelected = cardType;
            waitOponnent();
            GameObject cardSelected = Instantiate(Deck.deck.myGameObjects[cardType], new Vector3(0, 0, 0), quaternion.identity);
            //get origin0
            Transform ph = GameSetup.GS.spawnPoints[0].GetChild(0);
            cardSelected.transform.SetParent(ph);
            cardSelected.transform.localPosition = Vector3.zero;
            cardSelected.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
            cardSelected.transform.localScale = new Vector3(1, 1, 1);
            ph.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            oponnentTypeCardSelected = cardType;
            setOponnentWait(true);
            string path = "";
            if (GameTurnSystem.GTS.player.camp == "Heaven")
            {
                path = "Hell/Hell_";
            }
            else
            {
                path = "Heaven/Heaven_";
            }
            GameObject localGO = Resources.Load<GameObject>(path + Enum.GetNames(typeof(Deck.cardName))[cardType]);
            GameObject cardSelected = Instantiate(localGO, new Vector3(0, 0, 0), quaternion.identity);
            //get origin1
            Transform ph = GameSetup.GS.spawnPoints[0].GetChild(1);
            cardSelected.transform.SetParent(ph);
            cardSelected.transform.localPosition = Vector3.zero;
            cardSelected.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
            cardSelected.transform.localScale = new Vector3(1, 1, 1);
            cardSelected.gameObject.SetActive(false);
        }
    }

    public void cancelCardRec()
    {
        PV.RPC("cancelCard", RpcTarget.AllBuffered, PhotonRoom.room.playerId);
    }
    [PunRPC]
    public void cancelCard(int playerId)
    {
        if (PhotonRoom.room.playerId == playerId)
        {
            typecardSelected = -1;
            state = TurnState.Card;
            phase.text = "Phase: " + state;
            //get origin0
            Transform ph = GameSetup.GS.spawnPoints[0].GetChild(0);
            Destroy(ph.GetChild(1).gameObject);
            ph.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            oponnentTypeCardSelected = -1;
            setOponnentWait(false);
            //get origin1
            Transform ph = GameSetup.GS.spawnPoints[0].GetChild(1);
            Destroy(ph.GetChild(1).gameObject);
        }
    }

    public void setOponnentWait(bool waiting)
    {
        oponnentIsWaiting = waiting;
        oponnentWaitText.gameObject.SetActive(waiting);
    }

    IEnumerator Timer(int countdown)
    {
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        if (state == TurnState.PlayerTurn)
        {
            SetupTurn();

        }else if(state==TurnState.Card)
        {
            oponnentIsWaiting = false;
            PlayCharacter();
        }
        else if (state==TurnState.Start)
        {
            PlayCard();
        }

    }
}
