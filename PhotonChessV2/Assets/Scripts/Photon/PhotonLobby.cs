using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// This class has the task of manage the connection to master and creation of rooms
/// </summary>
public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;//singleton

    public GameObject battleButton;
    public GameObject cancelButton;
    public GameObject localButton;
    public GameObject onlineButton;
    public Text netStatusText;
    private void Awake()
    {
        lobby = this;//reference the singleton
    }
    //Starting connection to photon servers
    public void StartConnection()
    {
        localButton.SetActive(false);
        onlineButton.SetActive(false);
        netStatusText.text = "Connecting to Server";
        PhotonNetwork.ConnectUsingSettings();
    }
    public void StartLocalGame()
    {
        SceneManager.LoadScene("LocalMatch");
    }
    //Activate battle button
    public override void OnConnectedToMaster()
    {
        netStatusText.text = "Connected to server";
        PhotonNetwork.AutomaticallySyncScene = true;
        battleButton.SetActive(true);
    }
    //Creating a room
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        netStatusText.text = "Failed to join a random room, creating one";
        CreateRoom();
    }
    //Retry creating a room
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        netStatusText.text = "Error creating room, maybe there is already a room with this name";
        CreateRoom();
    }

    //Create a random room
    private void CreateRoom()
    {
        int randomRoomName = Random.Range(0, 100000);//picks a random number between 0 to 100,000
        //setting room options
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);//creating room
    }
    //Battlebutton action
    public void OnBattleButtonClick()
    {
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        netStatusText.text = "Trying to join a random room";
        PhotonNetwork.JoinRandomRoom();
    }
    //Cancel Button Action
    public void OnCancelButtonClick()
    {
        battleButton.SetActive(true);
        cancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }
}
