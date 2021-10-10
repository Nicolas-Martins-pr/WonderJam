using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public enum TurnState {Start, PlayerTurn, Card,End}
public class GameTurnSystem : MonoBehaviour
{
    public static GameTurnSystem GTS;
    public TurnState state;
    public Text phase;
    public Text countdownText;
    public bool win;
    private Coroutine coInst = null;
    public Transform CardOrigin;
    public PhotonPlayer player;
    void Start()
    {
        GTS = this;
        SetupTurn();
        
    }

    // Update is called once per frame

    void SetupTurn()
    {
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
        Deck.deck.GiveCard();
        //Quand les deux joueurs on joué, faire le test de la win
        if (win)
        {
            StopCoroutine(coInst);
            state = TurnState.PlayerTurn;
            PlayCharacter();
            
        }
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
            PlayCharacter();
        }
        else if (state==TurnState.Start)
        {
            PlayCard();
        }

    }
}
