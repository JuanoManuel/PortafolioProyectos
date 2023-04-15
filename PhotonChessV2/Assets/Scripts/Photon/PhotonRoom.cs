using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room info
    public static PhotonRoom room;//singleton
    public static int WHITES = 0, BLACKS = 1;
    private PhotonView PV;//PhotonView of this gameObject

    public bool isGameLoaded;//is game scene loaded
    public int currentScene;//current index scene
    public Text netInfoText;

    //Player info
    private Player[] photonPlayers;//players connected to the room
    public int playersInRoom;//number of players
    public int myNumberInRoom;//my number in the room
    public int playerInGame;//player is in game scene
    public int playerType;//0 = whites, 1 = blacks

    //avoing destroy the singleton on loading scene
    private void Awake()
    {
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(gameObject);
            }
        }
    }

    //subscribe to sceneLoaded event
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    //unsuscribe to sceneloaded event
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();//gets reference of the PhotonView
    }

    #region callbacks
    //When someone join the room
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined a room");
        netInfoText.text = "Waiting opponent";
        photonPlayers = PhotonNetwork.PlayerList;//update players
        playersInRoom = photonPlayers.Length;//num of players
        myNumberInRoom = playersInRoom;//get my number
        playerType = myNumberInRoom == 1 ? WHITES : BLACKS;
        PhotonNetwork.NickName = myNumberInRoom.ToString();//set a nickname
        if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)//if are the max players in the room
        {
            //return the function if is not the master client
            if (!PhotonNetwork.IsMasterClient)
                return;
            //close the room and start the game
            PhotonNetwork.CurrentRoom.IsOpen = false;
            StartGame();
        }
    }

    //When a player enters the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("New player connected");
        //update players and num of players in the room
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)//if are the max players in the room
        {
            //return the function if is not the master client
            if (!PhotonNetwork.IsMasterClient)
                return;
            //close the room and start the game
            PhotonNetwork.CurrentRoom.IsOpen = false;
            StartGame();
        }
    }
    #endregion

    //Starts The GameScene
    private void StartGame()
    {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);
    }

    //When a scene finished loading
    private void OnSceneFinishedLoading(Scene scene,LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;//update the currentScene
        if(currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene)//check if the scene is the gameplay scene
        {
            isGameLoaded = true;
            //calls the RPC to load the game in the master client
            PV.RPC("RpcLoadedGameScene", RpcTarget.MasterClient);
        }
    }

    #region RPCs
    [PunRPC]
    private void RpcLoadedGameScene()
    {
        playerInGame++;
        if(playerInGame == PhotonNetwork.PlayerList.Length)//when all players are in the game scene
        {
            //Runs the RPC to create the player
            PV.RPC("RpcCreatePlayer", RpcTarget.All);
        }
    }
    [PunRPC]
    private void RpcCreatePlayer()
    {
        //Instantiates the player
        object[] data = new object[]{ playerType };
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0,data);
    }
    #endregion
}
