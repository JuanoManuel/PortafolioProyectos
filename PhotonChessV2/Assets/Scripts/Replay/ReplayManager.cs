using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    private Board board;
    [SerializeField] GlobalData globalData;
    private MatchData matchData;
    private int moveIndex;
    private Stack<DeletedObject> deletedObjects;
    private void Awake()
    {
        moveIndex = -1;
        matchData = new MatchData();
        matchData.Date = "06 - 05 - 2021";
        matchData.Color = "white";
        matchData.Opponent = "admin";
        matchData.Type = "Online";
        matchData.Winner = "white";
        matchData.Moves = new List<string>(){ "m,13,33", "m,64,44", "m,33,44", "m,63,43", "e,44,53,43", "m,74,64", "m,53,64" };
        matchData.Duration = "28.3003";
        globalData.matchData = matchData;
    }

    private void Start()
    {
        //subcribe to the events we are interested in
        ReplayEventSystem.current.OnBoardInstantiated += OnBoardInstantiated;
        ReplayEventSystem.current.OnPlayerReady += OnPlayerReady;
        ReplayEventSystem.current.OnNextMove += ReplayNextMove;
        ReplayEventSystem.current.OnPreviousMove += ReplayPreviousMove;

        deletedObjects = new Stack<DeletedObject>();
    }

    private void OnDestroy()
    {
        //Unsubscribing to all events
        ReplayEventSystem.current.OnBoardInstantiated -= OnBoardInstantiated;
        ReplayEventSystem.current.OnPlayerReady -= OnPlayerReady;
        ReplayEventSystem.current.OnNextMove -= ReplayNextMove;
        ReplayEventSystem.current.OnPreviousMove -= ReplayPreviousMove;
    }

    private void OnBoardInstantiated(BoardInstantiatedEvent e)
    {
        board = e.Board.GetComponent<Board>();
        Debug.Log("Board referenciado");
    }

    private void OnPlayerReady()
    {
        if(globalData.matchData.Type == "Online")
        {
            board.CreateBoard(globalData.matchData.Color);
            board.CreateEnemyBoard(globalData.matchData.Color == "white" ? "black" : "white");
            board.SetPlayerColliders("white", false);
            board.SetPlayerColliders("black", false);
        }
    }

    public void ReplayNextMove()
    {
        if(MoveExists(moveIndex + 1))
            RecreateMove(++moveIndex);
    }

    public void ReplayPreviousMove()
    {
        if (MoveExists(moveIndex))
        {
            RecreatePreviousMove(moveIndex);
            moveIndex--;
        }
    }

    private bool MoveExists(int index)
    {
        if (index < globalData.matchData.Moves.Count && index >= 0)
            return true;
        return false;
    }

    private void RecreateMove(int index)
    {
        Debug.Log(index);
        string[] move = globalData.matchData.Moves[index].Split(',');
        Debug.Log("move: "+globalData.matchData.Moves[index]);
        switch (move[0])
        {
            case "m"://simple move m,r1c1,r2c2
                int row1 = int.Parse(move[1].Substring(0, 1));
                int col1 = int.Parse(move[1].Substring(1, 1));
                int row2 = int.Parse(move[2].Substring(0, 1));
                int col2 = int.Parse(move[2].Substring(1, 1));
                DeletePiece(index, row2, col2);
                MovePiece(row1, col1, row2, col2);
                break;
            case "p": //promotion move p,chessmanType,player,r1c1,r2c2
                row1 = int.Parse(move[3].Substring(0, 1));
                col1 = int.Parse(move[3].Substring(1, 1));
                row2 = int.Parse(move[4].Substring(0, 1));
                col2 = int.Parse(move[4].Substring(1, 1));
                Destroy(board.GetPieceAtPosition(row1,col1));//destroy pawn
                DeletePiece(index,row2, col2);//destroy piece where promoting if there is any
                board.Create(row2,col2,move[1],move[2]);//creates promoted piece
                break;
            case "c": //castling move c,kr1,kr2,r1,r2
                row1 = int.Parse(move[3].Substring(0, 1));
                col1 = int.Parse(move[3].Substring(1, 1));
                row2 = int.Parse(move[4].Substring(0, 1));
                col2 = int.Parse(move[4].Substring(1, 1));
                int rookrow1 = int.Parse(move[3].Substring(0, 1));
                int rookcol1 = int.Parse(move[3].Substring(1, 1));
                int rookrow2 = int.Parse(move[4].Substring(0, 1));
                int rookcol2 = int.Parse(move[4].Substring(1, 1));
                MovePiece(row1, col1, row2, col2);
                MovePiece(rookrow1, rookcol1, rookrow2, rookcol2);
                break;
            case "e": //em passant move e,r1c1,r2c2,pr,pc
                row1 = int.Parse(move[1].Substring(0, 1));
                col1 = int.Parse(move[1].Substring(1, 1));
                row2 = int.Parse(move[2].Substring(0, 1));
                col2 = int.Parse(move[2].Substring(1, 1));
                int pawnrow = int.Parse(move[3].Substring(0, 1));
                int pawncol = int.Parse(move[3].Substring(1, 1));
                MovePiece(row1, col1, row2, col2);
                DeletePiece(index,pawnrow, pawncol);
                break;
        }
    }

    private void RecreatePreviousMove(int index)
    {
        Debug.Log(index);
        string[] move = globalData.matchData.Moves[index].Split(',');
        Debug.Log("move: " + globalData.matchData.Moves[index]);
        switch (move[0])
        {
            case "m"://simple move m,r1c1,r2c2
                int row1 = int.Parse(move[1].Substring(0, 1));
                int col1 = int.Parse(move[1].Substring(1, 1));
                int row2 = int.Parse(move[2].Substring(0, 1));
                int col2 = int.Parse(move[2].Substring(1, 1));

                MovePiece(row2, col2, row1, col1);
                RestorePiece(index, row2, col2);
                break;
            case "p": //promotion move p,chessmanType,player,r1c1,r2c2
                row1 = int.Parse(move[3].Substring(0, 1));
                col1 = int.Parse(move[3].Substring(1, 1));
                row2 = int.Parse(move[4].Substring(0, 1));
                col2 = int.Parse(move[4].Substring(1, 1));
                Destroy(board.GetPieceAtPosition(row2, col2));
                RestorePiece(index, row2, col2);
                board.Create(row1, col1, "Pawn", move[2]);
                break;
            case "c": //castling move c,kr1,kr2,r1,r2
                row1 = int.Parse(move[3].Substring(0, 1));
                col1 = int.Parse(move[3].Substring(1, 1));
                row2 = int.Parse(move[4].Substring(0, 1));
                col2 = int.Parse(move[4].Substring(1, 1));
                int rookrow1 = int.Parse(move[3].Substring(0, 1));
                int rookcol1 = int.Parse(move[3].Substring(1, 1));
                int rookrow2 = int.Parse(move[4].Substring(0, 1));
                int rookcol2 = int.Parse(move[4].Substring(1, 1));
                MovePiece(row2, col2, row1, col1);
                MovePiece(rookrow2, rookcol2, rookrow1, rookcol1);
                break;
            case "e": //em passant move e,r1c1,r2c2,pr,pc
                row1 = int.Parse(move[1].Substring(0, 1));
                col1 = int.Parse(move[1].Substring(1, 1));
                row2 = int.Parse(move[2].Substring(0, 1));
                col2 = int.Parse(move[2].Substring(1, 1));
                int pawnrow = int.Parse(move[3].Substring(0, 1));
                int pawncol = int.Parse(move[3].Substring(1, 1));
                RestorePiece(index, pawnrow, pawncol);
                MovePiece(row2, col2, row1, col1);
                break;
        }
    }

    private void MovePiece(int row1,int col1,int row2,int col2)
    {
        GameObject piece = board.GetPieceAtPosition(row1, col1);
        piece.GetComponent<Chessman>().SetNewPosition(row2, col2);
        board.SetPieceAtPosition(row2, col2, piece);
        board.SetPositionEmpty(row1, col1);
    }

    private void CheckWin(GameObject piece)
    {
        if (piece == null)
            return;
        if(piece.TryGetComponent(out Chessman chessman))
        {
            if (chessman.GetType().ToString().Equals("King"))
                ReplayEventSystem.current.EndGame(chessman.player.Equals(globalData.matchData.Color)?"You win":"You lose");
        }
    }

    private void DeletePiece(int index, int row, int col)
    {
        GameObject piece = board.GetPieceAtPosition(row, col);
        if(piece != null)
        {
            deletedObjects.Push(new DeletedObject { Index = index, Object = piece });
            CheckWin(piece);
            piece.SetActive(false);
        }
    }

    private void RestorePiece(int index,int row,int col)
    {
        if (deletedObjects.Count > 0 && deletedObjects.Peek().Index == index)
        {
            var piece = deletedObjects.Pop().Object;
            piece.SetActive(true);
            board.SetPieceAtPosition(row, col, piece);
        }
    }

    /// <summary>
    /// delted pieces class, used to keep deleted gameobjects for future respawnings
    /// </summary>
    public class DeletedObject
    {
        public int Index { get; set; }
        public GameObject Object { get; set; }
    }
}
