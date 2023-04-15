using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayGenerator : MonoBehaviour
{
    private List<string> replay;

    private void Start()
    {
        replay = new List<string>();
    }
    /// <summary>
    /// Clear the replay to store a new one
    /// </summary>
    public void Clear()
    {
        replay.Clear();
    }
    /// <summary>
    /// Add a simple move
    /// </summary>
    /// <param name="row">initial row position</param>
    /// <param name="col">Initial col position</param>
    /// <param name="newRow">Final row position</param>
    /// <param name="newCol">/final col position</param>
    public void AddMove(int row,int col,int newRow,int newCol)
    {
        replay.Add("m," +row+col+","+newRow+newCol);
        Debug.Log("Movimiento agregado a replay");
    }
    /// <summary>
    /// Adds a promotion move
    /// </summary>
    /// <param name="chessmanType">type of chessman the pawn will promote</param>
    /// <param name="player">owner of the pawn</param>
    /// <param name="pawnRow">pawn row position</param>
    /// <param name="pawnCol">pawn col position</param>
    /// <param name="row">spawn row position</param>
    /// <param name="col">wpaen col position</param>
    public void AddPromotion(string chessmanType, string player, int pawnRow, int pawnCol, int row, int col)
    {
        replay.Add("p," + chessmanType + "," + player + "," + pawnRow + pawnCol + "," + row + col);
        Debug.Log("Promocion agregado a replay");
    }
    /// <summary>
    /// Adds a castling move
    /// </summary>
    /// <param name="kingRow">Initial king row position</param>
    /// <param name="kingCol">Initial king col position</param>
    /// <param name="kingNewRow">Final king row position</param>
    /// <param name="kingNewCol">Final king col position</param>
    /// <param name="rookRow">Initial Rook row position</param>
    /// <param name="rookCol">Initial Rook col position</param>
    /// <param name="rookNewRow">Final Rook row position</param>
    /// <param name="rookNewCol">Final Rook col position</param>
    public void AddCastling(int kingRow, int kingCol, int kingNewRow, int kingNewCol, int rookRow, int rookCol, int rookNewRow, int rookNewCol)
    {
        replay.Add("c," + kingRow + kingCol + "," + kingNewRow + kingNewCol + "," + rookRow + rookCol + "," + rookNewRow + rookNewCol);
        Debug.Log("Enroque agregado a replay");
    }
    /// <summary>
    /// Adds an EnPassant move
    /// </summary>
    /// <param name="initRow">Initial pawn row position</param>
    /// <param name="initCol">Initial pawn col position</param>
    /// <param name="newRow">Final pawn row position</param>
    /// <param name="newCol">Final pawn col position</param>
    /// <param name="pawnRow">Opponent pawn row</param>
    /// <param name="pawnCol">Opponent pawn col</param>
    public void AddEnPassant(int initRow, int initCol, int newRow, int newCol, int pawnRow, int pawnCol)
    {
        replay.Add("e," + initRow + initCol + "," + newRow + newCol + "," + pawnRow + pawnCol);
        Debug.Log("EnPassant agregado a replay");
    }
    /// <summary>
    /// returns the replay stored
    /// Don't forget to call the method Clear() to store a new replay
    /// </summary>
    /// <returns></returns>
    public List<string> GetReplay()
    {
        return replay;
    }
}
