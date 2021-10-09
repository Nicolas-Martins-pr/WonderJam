using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public enum TurnState {Start, PlayerTurn, Card,End}
public class GameTurnSystem : MonoBehaviour
{
    public TurnState state;
    public GameObject player;
    public GameObject discarded;
    public Text phase;
    public Text countdownText;
    public bool win;
    private Coroutine coInst = null;
    private Deck deckos;
    public Camera camera;
    void Start()
    {
        
        SetupTurn();
        
    }

    // Update is called once per frame

    void SetupTurn()
    {
        state = TurnState.Start;
        phase.text = "Phase: " + state;
        coInst=StartCoroutine(Timer(5));
        Deck.deck.GiveCard(camera);
        //il faut implémenter l'animator pour terminer cette phase du jeu
    }

    void PlayindCard()
    {
        state = TurnState.Card;
        phase.text = "Phase: "+state;
        coInst = StartCoroutine(Timer(30));
        //Quand les deux joueurs on joué, faire le test de la win
        if (win)
        {
            StopCoroutine(coInst);
            state = TurnState.PlayerTurn;
            PlayerPlay();
            
        }
    }

    public void PlayerPlay()
    {
        state = TurnState.PlayerTurn;
        phase.text = "Phase: "+state;
        coInst = StartCoroutine(Timer(30));
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
            PlayerPlay();
        }
        else if (state==TurnState.Start)
        {
            PlayindCard();
        }

    }
}
