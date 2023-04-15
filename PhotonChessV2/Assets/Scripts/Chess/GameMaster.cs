using Lean.Touch;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
[RequireComponent(typeof(ReplayGenerator))]
public class GameMaster : MonoBehaviour
{
    public static GameMaster master;
    public enum PlayerStatus { None, Both, Me, Opponent }

    public GameObject platesContainer;//reference to the empty game object where the plates will be instantiating
    //**UI Elements**//
    public Text turnText;
    public GameObject winnerPanel;
    public Text winnerText;
    [Space(30)]
    public GameObject[] plates;//List of all types of plates
    [Header("Debuging")]
    public string currentTurn;//The current turn of the game
    public string player;//my type of player when is an online match;
    public bool IsOnlineMatch;
    public bool IsBoardPlaced;
    public PlayerStatus readyToPlay;

    private Dictionary<string, GameObject> dPlates = new Dictionary<string, GameObject>();//dictionary for easy finding plates
    private ArrayList promoteData = new ArrayList();//Only used when happends a promotion
    private ARPlaneManager planeManager;
    private Text textInstructions;//UI Text to show instructions
    private GameObject[] opponentLastMovePlates = new GameObject[2];//Stores the two plates that represent the last move of the opponent
    private Dictionary<string, object> lastMoveData = new Dictionary<string, object>();
    private Board board;//reference to the board class
    private MultiplayerService multiplayer;
    private ReplayGenerator replayGenerator;

    private void Awake()
    {
        readyToPlay = PlayerStatus.None;
        foreach (GameObject plate in plates)
            dPlates.Add(plate.name, plate);
    }

    private void Start()
    {
        if (IsOnlineMatch)//If is an online match reference the multiplayerService and it will define the singleton
            multiplayer = GetComponent<MultiplayerService>();
        else//If is not an online match set this as the singleton
            master = this;
        planeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ARPlaneManager>();
        replayGenerator = GetComponent<ReplayGenerator>();
        turnText = GameResources.resources.turnText;
        winnerPanel = GameResources.resources.winnerPanel;
        winnerText = GameResources.resources.winnerText;
        textInstructions = GameResources.resources.instructions;
        SetInstruction("Place the board");
    }
    #region Plates
    //Destroys all the plates instantiated in the plates container
    public void DestroyAllPlates()
    {
        int i = 0;

        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[platesContainer.transform.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in platesContainer.transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //Now destroy them
        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }
    }
    //returns the plate gameobject according the given name
    public GameObject GetPlate(string name)
    {
        return dPlates[name];
    }
    #endregion
    #region Turn Management
    //Gets the current turn
    public string GetTurn()
    {
        return currentTurn;
    }
    //Updates the next turn
    public void NextTurn()
    {
        //Deactivate the previous turn pieces before setting a new turn
        if (!IsOnlineMatch)
        {//If it isn't an online match, we deactivate the opposite turn pieces each turn
            board.SetPlayerColliders(currentTurn, false);//the previous turn's piece will be deactivated
        }
        currentTurn = currentTurn == "white" ? "black" : "white";//setting next turn
        if (IsOnlineMatch)
        {//If is an online match set if its your or opponent's turn
            turnText.text = player == currentTurn ? "Your Turn" : "Opponent's Turn";//updating turn text
        }
        else
        {//If isn't an online match set if its whites or blacks turn
            turnText.text = player == currentTurn ? "Whites Turn" : "Blacks Turn";//updating turn text
            board.SetPlayerColliders(currentTurn, true);//Activate the current turn player pieces
        }
    }

    private void EndGame(string winner)
    {
        float time = board.GetComponent<TimeManager>().StopTimer();
        board.SetPlayerColliders(winner == "white"?"black":"white", false);
        if (IsOnlineMatch)
        {
            if (multiplayer.IsMaster())
            {
                //creating match
                GameResources.resources.firebaseManager.StoreMatch(winner, time, DateTime.Now, replayGenerator.GetReplay(), true, player);
            }
            string s = winner == player ? "win" : "lose";
            winnerText.text = "You " + s;
            GameResources.resources.firebaseManager.UpdatePlayerStats(winner == player);
        }
        else
        {
            winnerText.text = winner + " wins";
            GameResources.resources.firebaseManager.StoreMatch(winner, time,DateTime.Now, replayGenerator.GetReplay(), false);
            replayGenerator.Clear();
        }
        winnerPanel.SetActive(true);
        SetInstruction(string.Empty);
    }
    private void CheckWin(GameObject attackedPiece)
    {
        //check if its the king
        if (attackedPiece.GetComponent<Chessman>().GetType().ToString() == "King")
            EndGame(attackedPiece.GetComponent<Chessman>().player == "white" ? "black" : "white");//If so, set the opponent as the winner
    }
    #endregion
    #region UIBehaviour
    //Shows the promote panel
    public void ShowPromotePanel(int pawnRow, int pawnCol,int row,int col,string player)
    {
        promoteData.Clear();
        promoteData.Add(pawnRow);
        promoteData.Add(pawnCol);
        promoteData.Add(row);
        promoteData.Add(col);
        promoteData.Add(player);
        GameResources.resources.promotingPanel.SetActive(true);
    }
    //Set the ui instructions text
    public void SetInstruction(string text)
    {
        textInstructions.text = text;
    }
    //Activates or Deactivates all the ARPlanes
    public void SetAllPlanesActive(bool value)
    {
        planeManager.enabled = value;
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }

    public void StartTimer()
    {
        board.GetComponent<TimeManager>().StartTimer();
    }

    public void SetReady()
    {
        GameResources.resources.btnReady.SetActive(false);//hides the ready button
        //Desible the rotating behavior in the board
        GameResources.resources.gameObject.GetComponent<LeanTouch>().enabled = false;
        if (IsOnlineMatch)
        {//If it's an online match, we only activate the player pieces for the entire game
            board.SetPlayerColliders(player, true);
            multiplayer.SendReadyToPlay(GameResources.resources.firebaseManager.GetDisplayName(), GameResources.resources.firebaseManager.GetUserData());
        }
        else
        {
            SetInstruction(string.Empty);
            StartTimer();
        }
    }
    #endregion
    #region Board Interaction
    //Gets the board, used by Chessmans and plates
    public Board GetBoard()
    {
        return board;
    }
    //Sets the spawned board
    public void SetBoard(GameObject objBoard)
    {
        //Set the board has been placed
        IsBoardPlaced = true;
        //getting references to the containers
        board = objBoard.GetComponent<Board>();
        platesContainer = board.platesContainer;
        board.CreateBoard(player);//creating board
        if (IsOnlineMatch)//if is an online match
        {
            //if its an online match and this player has the blacks rotate the board
            if (player == "black")
                board.boardModel.transform.rotation = Quaternion.identity;
            //if the opponent was ready before I spawn my board, spawn the enemy board
            if (readyToPlay == PlayerStatus.Opponent)//if my opponent is the only one ready
            {
                board.CreateEnemyBoard(player == "white" ? "black" : "white");
                FillEnemyData(GameResources.resources.opponentData.DisplayName, "" + GameResources.resources.opponentData.numMatches, "" + GameResources.resources.opponentData.winRate);
            }
        }
        else
        {
            board.CreateEnemyBoard(player == "white" ? "black" : "white");
            NextTurn();
        }
        SetAllPlanesActive(false);
        GameResources.resources.btnReady.SetActive(true);//showing the ready button
        SetInstruction("Adjust the board"); //changing instruction
        //populating last move dictionary
        lastMoveData.Add("row", 0);//saving row
        lastMoveData.Add("col", 0);//saving col
        lastMoveData.Add("type", "none");//saving the type of piece that was moved
        lastMoveData.Add("firstMove", true);
    }
    //Fills enemy data in the board prefab
    public void FillEnemyData(string displayName, string numMatches, string winRate)
    {
        board.gameObject.GetComponent<EnemyData>().FillLabels(displayName, numMatches, winRate);
    }
    //Creates the enemy board when is a online match
    public void CreateEnemyBoard()
    {
        board.CreateEnemyBoard(player == "white" ? "black" : "white");
    }
    /// <summary>
    /// Makes a move on the board
    /// </summary>
    /// <param name="initRow">the original position of the piece</param>
    /// <param name="initCol">the original position of the piece</param>
    /// <param name="newRow">position where will be the piece</param>
    /// <param name="newCol">position where will be the piece</param>
    /// <param name="isSent">indicates if this movement was sent by opponent</param>
    /// <param name="isNextTurn">indicates if the method needs to set a new turn</param>
    public void MovePiece(int initRow, int initCol, int newRow, int newCol, bool isSent, bool isNextTurn = true)
    {
        GameObject piece = board.GetPieceAtPosition(initRow, initCol);//get the piece to move
        Chessman cm = piece.GetComponent<Chessman>();
        //save the last move data
        lastMoveData["row"] = newRow;//saving row
        lastMoveData["col"] = newCol;//saving col
        lastMoveData["type"] = cm.GetType().ToString();//saving the type of piece that was moved
        lastMoveData["firstMove"] = cm.isFirstMove;
        cm.isFirstMove = false;
        //If it's an online match spawn the last move plates
        if (IsOnlineMatch)
        {
            if (isSent)//if it is sent from the opponent, spawn the last move plates
            {
                if (multiplayer.IsMaster())//if Im the master client add my opponent's move into the replay
                {
                    replayGenerator.AddMove(initRow, initCol, newRow, newCol);
                }
                //Destroying last move opponent plates
                if (opponentLastMovePlates[0])
                    Destroy(opponentLastMovePlates[0]);
                if (opponentLastMovePlates[1])
                    Destroy(opponentLastMovePlates[1]);
                //Instantianting plates that represents this move
                opponentLastMovePlates[0] = Instantiate(dPlates["Previous"], Vector3.zero, Quaternion.identity);//initial position
                opponentLastMovePlates[1] = Instantiate(dPlates["Previous"], Vector3.zero, Quaternion.identity);//final position
                                                                                                          //Setting parent to the pieceContainer, also adjusting local scale and rotation
                foreach (GameObject plate in opponentLastMovePlates)
                {
                    plate.transform.parent = board.platesContainer.transform;
                    plate.transform.localScale = new Vector3(0.0125f, 0.0125f, 0.0125f);
                    plate.transform.localRotation = Quaternion.identity;
                }
                //Setting correct position of the plates
                opponentLastMovePlates[0].transform.localPosition = new Vector3(initCol * Chessman.offset, 0.0001f, initRow * Chessman.offset);
                opponentLastMovePlates[1].transform.localPosition = new Vector3(newCol * Chessman.offset, 0.0001f, newRow * Chessman.offset);
            }
            else //if wasn't called from the opponent, then request the opponent to replicate the move
            {
                multiplayer.SendMove(initRow, initCol, newRow, newCol, isNextTurn);
            }
        }
        GameObject attackedPiece = board.GetPieceAtPosition(newRow, newCol);//get the piece at the final position
        if (attackedPiece)//if there is a piece, destroy it
        {
            CheckWin(attackedPiece);
            Destroy(attackedPiece);//Destroy the attacked piece
        }
        //Moving indicated piece
        board.SetPieceAtPosition(newRow, newCol, piece);//set the new Position
        board.SetPositionEmpty(initRow, initCol);//empty the previous position
        //updating logic and world coordinates of the chessman
        cm.SetNewPosition(newRow, newCol);//visualy moving the piece
        if (isNextTurn) NextTurn();
    }
    //Creates a local promotion on board. Called From the promotion UI panel
    public void SetPromotion(string chessmanType)
    {
        GameResources.resources.promotingPanel.SetActive(false);
        //LocalPromotion
        replayGenerator.AddPromotion(chessmanType, IsOnlineMatch ? master.player : promoteData[4].ToString(), (int)promoteData[0], (int)promoteData[1], (int)promoteData[2], (int)promoteData[3]);
        Destroy(board.GetPieceAtPosition((int)promoteData[0], (int)promoteData[1]));//destroy pawn
        if(board.BoardHasPiece((int)promoteData[2], (int)promoteData[3]))
            CheckWin(board.GetPieceAtPosition((int)promoteData[2], (int)promoteData[3]));
        Destroy(board.GetPieceAtPosition((int)promoteData[2], (int)promoteData[3]));//destroy piece where promoting if there is any
        board.Create((int)promoteData[3], (int)promoteData[2], chessmanType, IsOnlineMatch?master.player:promoteData[4].ToString());//creates promoted piece
        if (IsOnlineMatch)
        {
            //replicates the promotion on the opponent's board
            multiplayer.SendPromotion(chessmanType, promoteData);
        }
        NextTurn();
    }
    //Called from the RPC to replicate a promotion
    public void SetPromotion(string chessmanType, string player,int pawnRow,int pawnCol,int row,int col)
    {
        if ((IsOnlineMatch && multiplayer.IsMaster()) || !IsOnlineMatch)
            replayGenerator.AddPromotion(chessmanType, player, pawnRow, pawnCol, row, col);
        //Replicating promotion
        Destroy(board.GetPieceAtPosition(pawnRow, pawnCol));//destroy pawn
        if(board.BoardHasPiece(row,col))
            CheckWin(board.GetPieceAtPosition(row, col));
        Destroy(board.GetPieceAtPosition(row, col));//destroy piece where promoting if there is any
        board.Create(col, row, chessmanType, player);//creates promoted piece
        NextTurn();//Setting next turn
    }
    /*reates a local castling on board
     * kingRow, kingCol, rookRow, rookCol: actual coordinates
     * kingNewRow, kingNewCol, rookNewRow, rookNewCol: new coordinates
     */
    public void SetClastling(int kingRow,int kingCol,int kingNewRow,int kingNewCol, int rookRow, int rookCol, int rookNewRow, int rookNewCol)
    {
        if ((IsOnlineMatch && multiplayer.IsMaster()) || !IsOnlineMatch)
            replayGenerator.AddCastling(kingRow, kingCol, kingNewRow, kingNewCol, rookRow, rookCol, rookNewRow, rookNewCol);
        //Moving local king and rook
        MovePiece(rookRow, rookCol, rookNewRow, rookNewCol, false,false);
        MovePiece(kingRow, kingCol, kingNewRow, kingNewCol, false);
    }
    /// <summary>
    /// Creates an EnPassant move in the local board
    /// If is an online match also send the move to the opponent if is need it
    /// </summary>
    /// <param name="initRow">init row location of the pawn</param>
    /// <param name="initCol">init col location of the pawn</param>
    /// <param name="newRow">new row of the pawn</param>
    /// <param name="newCol">new col of the pawn</param>
    /// <param name="pawnRow">row of the pawn to eliminate</param>
    /// <param name="pawnCol">col of the pawn to eliminate</param>
    /// <param name="sendMove">if it's required to send this move at the opponent</param>
    public void SetEnPassant(int initRow, int initCol, int newRow,int newCol, int pawnRow, int pawnCol,bool sendMove)
    {
        if ((IsOnlineMatch && multiplayer.IsMaster()) || !IsOnlineMatch)
            replayGenerator.AddEnPassant(initRow, initCol, newRow, newCol, pawnRow, pawnCol);
        //Visualy destroying the pawn
        Destroy(board.GetPieceAtPosition(pawnRow, pawnCol));
        //Set the enemy pawn position to empty
        board.SetPositionEmpty(pawnRow, pawnCol);
        //pawn to move reference
        GameObject pawn = board.GetPieceAtPosition(initRow, initCol);
        //updating position of the pawn on the board
        board.SetPositionEmpty(initRow, initCol);
        board.SetPieceAtPosition(newRow, newCol, pawn);
        //updating chessman properties
        pawn.GetComponent<Chessman>().SetNewPosition(newRow, newCol);
        if (IsOnlineMatch && sendMove)
            multiplayer.SendEnPassant(initRow, initCol, newRow, newCol, pawnRow, pawnCol);
        NextTurn();
    }
    //Getting the last opponent move, used mainly for the pawn movement calculation
    public object GetLastMoveData(string key)
    {
        return lastMoveData[key];
    }

    public void AddMoveToReplay(int row,int col,int newRow,int newCol)
    {
        if ((IsOnlineMatch && multiplayer.IsMaster()) || !IsOnlineMatch)
        {
            replayGenerator.AddMove(row, col, newRow, newCol);
        }
    }
    #endregion
}
