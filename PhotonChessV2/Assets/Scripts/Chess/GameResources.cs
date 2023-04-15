using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(FirebaseMatchManager))]
public class GameResources : MonoBehaviour
{
    public static GameResources resources;//singleton

    [Header("GUI Elements")]
    public GameObject winnerPanel;
    public Text winnerText;
    public Text turnText;
    public GameObject promotingPanel;
    public Text instructions;
    public GameObject btnReady;
    [Header("Debuging fields")]
    public GameObject prefabToInstantiate;
    [HideInInspector]
    public FirebaseMatchManager firebaseManager;
    [Header("Opponent Data Storage")]
    public GlobalData opponentData;
    private void Awake()
    {
        resources = this;
        firebaseManager = GetComponent<FirebaseMatchManager>();
    }

    public void PromoteToQueen()
    {
        GameMaster.master.SetPromotion("Queen");
    }
    public void PromoteToKnight()
    {
        GameMaster.master.SetPromotion("Knight");
    }
    public void PromoteToBishop()
    {
        GameMaster.master.SetPromotion("Bishop");
    }
    public void PromoteToRook()
    {
        GameMaster.master.SetPromotion("Rook");
    }

    public void TapToPlaceClick()
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        GameObject spawnObject = Instantiate(prefabToInstantiate, Vector3.zero, rotation);
        GameMaster.master.SetBoard(spawnObject);
    }

    public void ReadyButtonOnClick()
    {
        //Tells the game master the player is ready
        GameMaster.master.SetReady();
    }
}
