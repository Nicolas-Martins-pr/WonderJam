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
    public GameObject goDeck;
    public GameObject player;
    public GameObject ennemy;
    public GameObject character;
    private CanvasScaler.Unit playerUnit;
    private CanvasScaler.Unit ennemyUnit;
    public Text phase;
    public Text countdownText;
    public bool win;
    private Coroutine coInst = null;
    void Start()
    {
        
        SetupTurn();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void SetupTurn()
    {
        state = TurnState.Start;
        phase.text = "Phase: " + state;
        coInst=StartCoroutine(Timer(5));
        
        
    }

    void PlayindCard()
    {
        state = TurnState.Card;
        phase.text = "Phase: "+state;
        coInst = StartCoroutine(Timer(30));
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
