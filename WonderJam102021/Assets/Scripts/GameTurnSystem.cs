using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public enum TurnState {Start, PlayerTurn, Card, Wait, Roll}
public class GameTurnSystem : MonoBehaviour
{
    public static GameTurnSystem GTS;
    public TurnState state;
    public Text phase;
    public Text countdownText;
    public Text oponnentWaitText;
    private Coroutine coInst = null;
    public Transform CardOrigin;
    public PhotonPlayer player;
    public int typeCardSelected;
    public int KeyCardSelected;
    public int oponnentTypeCardSelected;
    public PhotonView PV;
    public bool oponnentIsWaiting = false;
    public Button rollButton;
    public int result;
    public int oponnentResult;
    public bool succes;
    public bool oponnentSucces;
    public bool asWin;
    public bool oponnentAsWin;
    public GameObject windowResult;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        GTS = this;
        SetupTurn();
        
    }

    // Update is called once per frame

    void SetupTurn()
    {
        typeCardSelected = -1;
        KeyCardSelected = -1;
        oponnentTypeCardSelected = -1;
        state = TurnState.Start;
        phase.text = "Phase: " + state;
        Debug.Log(phase.text);
        coInst=StartCoroutine(Timer(5));
        //il faut implémenter l'animator pour terminer cette phase du jeu
    }

    [PunRPC]
    void PlayCard()
    {

        try
        {
            StopCoroutine(coInst);
        }
        catch (Exception e)
        {
            Debug.Log("no timer to stop");
        }
        state = TurnState.Card;
        phase.text = "Phase: "+state;
        Debug.Log(phase.text);
        coInst = StartCoroutine(Timer(120));
        Deck.deck.GiveCard(PhotonRoom.room.playerId);
    }

    void waitOponnent()
    {
        state = TurnState.Wait;
        phase.text = "Phase: " + state;
        Debug.Log(phase.text);
    }
    [PunRPC]
    void DrawChance()
    {
        setOponnentWait(false);
        try
        {
            StopCoroutine(coInst);
        }catch(Exception e)
        {
            Debug.Log("no timer to stop");
        }
        state = TurnState.Roll;
        phase.text = "Phase: " + state;
        Debug.Log(phase.text);
        coInst = StartCoroutine(Timer(5));
        //show border and interior of oposite card
        GameSetup.GS.spawnPoints[0].GetChild(1).GetChild(0).gameObject.SetActive(true);
        GameSetup.GS.spawnPoints[0].GetChild(1).GetChild(1).gameObject.SetActive(true);
        rollButton.gameObject.SetActive(true);
    }

    public void RollDices()
    {
        try
        {
            StopCoroutine(coInst);
        }
        catch (Exception e)
        {
            Debug.Log("no timer to stop");
        }
        rollButton.gameObject.SetActive(false);
        int r = UnityEngine.Random.Range(0, 100);
        PV.RPC("setResult", RpcTarget.AllBuffered, r,  PhotonRoom.room.playerId);
    }
    [PunRPC]
    public void setResult(int _result, int playerId)
    {
        if (playerId == PhotonRoom.room.playerId)
        {
            result = _result;
            if (oponnentIsWaiting)
            {
                PV.RPC("PlayCharacter", RpcTarget.AllBuffered);
            }
        }
        else
        {
            oponnentResult = _result;
            setOponnentWait(true);
        }
    }

    public void ComputeResult()
    {
        asWin = false;
        oponnentAsWin = false;
        Card card = GameSetup.GS.spawnPoints[0].GetChild(0).GetChild(1).GetComponent<Card>();
        Card oponnentCard = GameSetup.GS.spawnPoints[0].GetChild(1).GetChild(1).GetComponent<Card>();
        succes = setSucces(result, card.cardType);
        oponnentSucces = setSucces(oponnentResult, oponnentCard.cardType);
        if ((succes && oponnentSucces && result > oponnentResult)||(succes && !oponnentSucces))
        {
            asWin = true;
        }
        else if ((succes && oponnentSucces && result < oponnentResult) || (!succes && oponnentSucces))
        {
            oponnentAsWin = true;
        }
    }

    public bool setSucces(int r, int cardType)
    {
        bool gg = false;
        if (cardType == 0 && r >= 10)
        {
            gg = true;
        }
        else if (cardType == 1 && r >= 40)
        {
            gg = true;
        }
        else if (cardType == 2 && r >= 40)
        {
            gg = true;
        }
        else if (cardType == 3 && r >= 20)
        {
            gg = true;
        }
        else if (cardType == 4 && r >= 60)
        {
            gg = true;
        }
        else if (cardType == 5 && r >= 60)
        {
            gg = true;
        }
        else if (cardType == 6 && result >= 90)
        {
            gg = true;
        }
        return gg;
    }

    public int getGoal(int CardType)
    {
        int goal = 0;
        switch (CardType)
        {
            case 0:
                goal = 10;
                break;
            case 1:
                goal = 40;
                break;
            case 2:
                goal = 40;
                break;
            case 3:
                goal = 20;
                break;
            case 4:
                goal = 60;
                break;
            case 5:
                goal = 60;
                break;
            case 6:
                goal = 90;
                break;
        }
        return goal;
    }

    [PunRPC]
    public void PlayCharacter()
    {
        try
        {
            StopCoroutine(coInst);
        }
        catch (Exception e)
        {
            Debug.Log("no timer to stop");
        }

        //discard card
        if (KeyCardSelected != -1)
            player.removeCard(KeyCardSelected);
        //Hide placeholders
        GameSetup.GS.spawnPoints[0].GetChild(0).GetChild(0).gameObject.SetActive(false);
        GameSetup.GS.spawnPoints[0].GetChild(1).GetChild(0).gameObject.SetActive(false);
        //destroy cardSelected
        Destroy(GameSetup.GS.spawnPoints[0].GetChild(0).GetChild(1).gameObject);
        Destroy(GameSetup.GS.spawnPoints[0].GetChild(1).GetChild(1).gameObject);

        setOponnentWait(false);

        ComputeResult();
        
        if (false) //(asWin)
        {
            state = TurnState.PlayerTurn;
            phase.text = "Phase: " + state;
            Debug.Log(phase.text);
            coInst = StartCoroutine(Timer(120));
        }
        else if (false) //(oponnentAsWin)
        {
            state = TurnState.Wait;
            phase.text = "Phase: " + state;
            Debug.Log(phase.text);
            coInst = StartCoroutine(Timer(120));
        }
        else
        {
            SetupTurn();
        }
        setWindowResult();
        
        
    }

    public void setWindowResult()
    {
        windowResult.transform.GetChild(3).GetComponent<Text>().text = result.ToString();
        windowResult.transform.GetChild(4).GetComponent<Text>().text = oponnentResult.ToString();

        windowResult.transform.GetChild(6).GetComponent<Text>().text = getGoal(typeCardSelected).ToString();
        windowResult.transform.GetChild(7).GetComponent<Text>().text = getGoal(oponnentTypeCardSelected).ToString();

        windowResult.transform.GetChild(9).GetComponent<Text>().text = asWin.ToString();
        windowResult.transform.GetChild(10).GetComponent<Text>().text = oponnentAsWin.ToString();

        windowResult.SetActive(true);
    }

    public void closeWindowResult()
    {
        windowResult.SetActive(false);
    }
    
    public void selectCardRec(int cardIndex)
    {
        if (state == TurnState.Card)
        {
            GameObject card = (GameObject)player.myCards[cardIndex];
            int cardValue = card.GetComponent<Card>().cardType;
            PV.RPC("selectCard", RpcTarget.AllBuffered, cardValue, PhotonRoom.room.playerId, cardIndex);
        }
    }


    [PunRPC]
    public void selectCard(int cardType, int playerId, int keyCard)
    {
        if (PhotonRoom.room.playerId == playerId)
        {
            typeCardSelected = cardType;
            KeyCardSelected = keyCard;
            waitOponnent();
            GameObject cardSelected = Instantiate(Deck.deck.myGameObjects[cardType], new Vector3(0, 0, 0), quaternion.identity);
            //get origin0
            Transform ph = GameSetup.GS.spawnPoints[0].GetChild(0);
            cardSelected.transform.SetParent(ph);
            cardSelected.transform.localPosition = Vector3.zero;
            cardSelected.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
            cardSelected.transform.localScale = new Vector3(1, 1, 1);
            ph.GetChild(0).gameObject.SetActive(true);
            if (oponnentIsWaiting)
            {
                PV.RPC("DrawChance", RpcTarget.AllBuffered);
            }
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
            typeCardSelected = -1;
            state = TurnState.Card;
            phase.text = "Phase: " + state;
            Debug.Log(phase.text);
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

        }else if(state==TurnState.Card)
        {
            oponnentIsWaiting = false;
            DrawChance();
        }
        else if (state==TurnState.Start)
        {
            PlayCard();
        }
        else if (state == TurnState.Roll)
        {
            RollDices();
        }
    }
}
