using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chessman
{
    public override void ShowMovements()
    {
        //Destroying all previous plates
        gameMaster.DestroyAllPlates();
        //The king can move around him//

        //Spawning plates around the king
        SpawnPlateAtPoint(row - 1, col - 1);
        SpawnPlateAtPoint(row-1,col);
        SpawnPlateAtPoint(row-1,col+1);
        SpawnPlateAtPoint(row,col-1);
        SpawnPlateAtPoint(row,col+1);
        SpawnPlateAtPoint(row+1,col-1);
        SpawnPlateAtPoint(row+1,col);
        SpawnPlateAtPoint(row + 1, col + 1);
        //The king can also make a castling move if there isn't piece between him and any rook
        //And also both haven't move previously
        SpawnCastlingPlates();
    }

    //Castling Move
    private void SpawnCastlingPlates()
    {
        //if is not the first move of the king finish function
        if (!isFirstMove)
            return;
        //validating left castling
        if (board.BoardHasPiece(row, 0)) //if there is a piece then check if the piece is a rook
        {
            GameObject rookObject = board.GetPieceAtPosition(row, 0);
            Chessman cm = rookObject.GetComponent<Chessman>();//reference to the chessman
            if(cm.GetType().ToString() == "Rook")//if it's a rook
            {
                Rook rook = (Rook)cm; //casting as Rook
                if (!rook.isFirstMove)//if isn't the rook's first move the function ends
                    return;
                //checking if there is a piece between them
                bool hasPiece = false;
                for(int i = 1; i < col && !hasPiece; i++)
                {
                    //If it is then we can't spawn a castling move
                    if (board.BoardHasPiece(row, i))
                        hasPiece = true;
                }
                //if the function reaches this point then we can spawn the castling move
                if(!hasPiece)
                    SpawnPlate(row, 2, "Castling",rookObject);
            }
        }
        if (board.BoardHasPiece(row, 7))//checking right castling
        {
            GameObject rookObject = board.GetPieceAtPosition(row, 7);
            Chessman cm = rookObject.GetComponent<Chessman>();//reference to the chessman
            if (cm.GetType().ToString() == "Rook")//if it's a rook
            {
                Rook rook = (Rook)cm; //casting as Rook
                if (!rook.isFirstMove)//if isn't the rook's first move the function ends
                    return;
                //checking if there is a piece between them
                bool hasPiece = false;
                for (int i = col+1; i < rook.col && !hasPiece; i++)
                {
                    //If it is then we can't spawn a castling move, so the function ends
                    if (board.BoardHasPiece(row, i))
                        hasPiece = true;
                }
                //if hasPiece is still false then we can spawn the castling plate
                if(!hasPiece)
                    SpawnPlate(row, 6, "Castling", rookObject);
            }
        }
    }
}
