using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerService : MonoBehaviour, IPunInstantiateMagicCallback
{
    [Header("Debugging")]
    public GameMaster master;
    private PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    //Callback when this object is instantiated from the ClientMaster
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!PV.IsMine)
        {//Indicating the not mine GameMaster that is an online match to avoid a local match on that game object
            GetComponent<GameMaster>().IsOnlineMatch = true;
            return;
        }
        GameMaster.master = GetComponent<GameMaster>();
        master = GameMaster.master;//setting singleton
        //Get the instantiation data
        object[] data = info.photonView.InstantiationData;
        //setting the type of player of this client
        master.player = (int)data[0] == PhotonRoom.WHITES ? "white" : "black";
        master.IsOnlineMatch = true;
        master.currentTurn = "black";
    }

    public bool IsMaster()
    {
        return PV.Owner.IsMasterClient;
    }
    #region RPC Callers
    //Called from the ready button, tells the opponent player the local player is ready to play
    public void SendReadyToPlay(string displayName,Dictionary<string,object> userData)
    {
        if (master.readyToPlay == GameMaster.PlayerStatus.Opponent)//if only my opponent is ready, set both are ready
        {
            master.readyToPlay = GameMaster.PlayerStatus.Both;//Both are ready
            master.SetInstruction(string.Empty);
            master.NextTurn();
            master.StartTimer();
            master.FillEnemyData(GameResources.resources.opponentData.DisplayName, ""+GameResources.resources.opponentData.numMatches,""+GameResources.resources.opponentData.winRate);
        }
        else if (master.readyToPlay == GameMaster.PlayerStatus.None)//if anyone is ready, set im the only one ready state
        {
            master.readyToPlay = GameMaster.PlayerStatus.Me;//Im the only one ready
            master.SetInstruction("Waiting opponent...");
        }
        PV.RPC("RpcSendReadyToPlay", RpcTarget.Others,displayName,int.Parse(userData["numMatches"].ToString()),float.Parse(userData["winRate"].ToString()),userData["idHistorial"].ToString(),userData["userKey"].ToString());
    }
    /*function called by the promote panel buttons
     * Promotes the pawn
     */
    public void SendPromotion(string chessmanType, ArrayList promoCoords)
    {
        //Calls the RPC method to replicate the promotion on the opponent board
        PV.RPC("RpcSendPromotion", RpcTarget.Others, 7 - (int)promoCoords[0], (int)promoCoords[1], 7 - (int)promoCoords[2], (int)promoCoords[3], chessmanType);
    }
    public void SendEnPassant(int initRow,int initCol,int newRow,int newCol, int pawnRow,int pawnCol)
    {
        PV.RPC("RpcSendEnPassant", RpcTarget.Others, 7 - initRow, initCol, 7 - newRow, newCol, 7 - pawnRow, pawnCol);
    }
    //Plates call this method to send a move to the opponent client
    public void SendMove(int initRow, int initCol, int newRow, int newCol, bool isNextTurn)
    {
        //all the coordinates must be converted to its opposite positions on the board
        //to move opponents pieces
        PV.RPC("RpcSendMove", RpcTarget.Others, 7 - initRow, initCol, 7 - newRow, newCol, isNextTurn);
    }
    #endregion

    #region Photon RPC
    [PunRPC]
    //RPC to let the opponent know when I am ready
    private void RpcSendReadyToPlay(string displayName, int numMatches, float winRate, string idHistorial,string userID)
    {
        Debug.Log($"Oponente {displayName} listo");
        GameResources.resources.opponentData.DisplayName = displayName;
        GameResources.resources.opponentData.numMatches = numMatches;
        GameResources.resources.opponentData.winRate = winRate;
        GameResources.resources.opponentData.historialKey = idHistorial;
        GameResources.resources.opponentData.userKey = userID;
        GameMaster master = GameMaster.master;//reference to the local game master
        if (master.IsBoardPlaced)
        {//if the board has already been instantiated
            master.CreateEnemyBoard();//create enemy board
            master.FillEnemyData(displayName, numMatches+"", winRate+"");
        }
        if (master.readyToPlay == GameMaster.PlayerStatus.None)//If anyone is ready, set only my opponent is ready
        {
            master.readyToPlay = GameMaster.PlayerStatus.Opponent;//setting my opponent is ready
            master.SetInstruction("Opponent ready");
        }
        else if (master.readyToPlay == GameMaster.PlayerStatus.Me)//If I'm ready, set both are ready
        {
            master.readyToPlay = GameMaster.PlayerStatus.Both;
            master.SetInstruction(string.Empty);
            master.NextTurn();
            master.StartTimer();
        }
    }
    [PunRPC]
    //RPC to replicate a promotion on the opponents board
    private void RpcSendPromotion(int pawnRow, int pawnCol,int row, int col, string chessmanType)
    {
        GameMaster master = GameMaster.master;//reference to the local game master
        master.SetPromotion(chessmanType, master.player == "white" ? "black" : "white",pawnRow,pawnCol,row,col);//creates the new piece
    }
    [PunRPC]
    //RPC to replicate a enpassant move on the opponents board
    private void RpcSendEnPassant(int initRow,int initCol,int newRow,int newCol,int pawnRow,int pawnCol)
    {
        GameMaster.master.SetEnPassant(initRow, initCol, newRow, newCol, pawnRow, pawnCol,false);
    }
    [PunRPC]
    //RPC to replicate a player movement on the opponents board
    private void RpcSendMove(int initRow, int initCol, int newRow, int newCol, bool isNextTurn)
    {
        GameMaster master = GameMaster.master;//Get a reference of the GameMaster of the Client
        //Moving the piece//
        master.MovePiece(initRow, initCol, newRow, newCol,true,isNextTurn);
    }
    #endregion
}
