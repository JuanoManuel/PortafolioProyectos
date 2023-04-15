using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chessman
{
    public override void ShowMovements()
    {
        //Destroy all previous plates
        gameMaster.DestroyAllPlates();
        int maxNum = 1;//max number of forward spaces the pawn can move; default 1

        if (isFirstMove)
        { //if it's its firsmove, means the piece can move forward 2 spaces
            maxNum = 2;
        }

        //***Moving plates***//
        //Creating the move plates
        //First determine the game mode
        if (gameMaster.IsOnlineMatch)//If it's an online match we just take care of the bottom use case
        {
            SpawnPawnWhitePlates(maxNum);
        }
        else//if not, then we also take care of the top board pieces
        {
            if(player == "white")
            {
                SpawnPawnWhitePlates(maxNum);
            }
            else
            {
                SpawnPawnBlackPlates(maxNum);
            }
        }
    }
    private void SpawnPawnWhitePlates(int maxNum)
    {
        for (int i = 1; i <= maxNum; i++)
        {
            //if in the next position there is already a piece we can't move the pawn there
            if (!board.BoardHasPiece(row + i, col))
            {
                if (row + i == 7)//if the pawn is about to be promoting
                    SpawnPlate(row + i, col, "Promote");//Spawn a promote plate
                else
                    SpawnPlate(row + i, col, "Moves");//Spawn a move plate
            }
            else //if its a piece it means the pawn can't move there and the next space eather
            {
                i = 3;//we force the forloop to stop iterating
            }
        }

        //***Attacking plates***//
        //The pawn can attack the forward diagonal spaces
        /*Spawning left diagonal attack*/
        SpawnPawnAttackPlate(row + 1, col - 1);
        /*Spawning right diagonal attack*/
        SpawnPawnAttackPlate(row + 1, col + 1);
        /*Spawning En Passant attack*/
        SpawnPawnEnPassant(row + 1);
    }
    private void SpawnPawnBlackPlates(int maxNum)
    {
        for (int i = -1; i >= maxNum * -1; i--)
        {
            //if in the next position there is already a piece we can't move the pawn there
            if (!board.BoardHasPiece(row + i, col))
            {
                if (row + i == 7 || row + i == 0)//if the pawn is about to be promoting
                    SpawnPlate(row + i, col, "Promote");//Spawn a promote plate
                else
                    SpawnPlate(row + i, col, "Moves");//Spawn a move plate
            }
            else //if its a piece it means the pawn can't move there and the next space eather
            {
                i = -3;//we force the forloop to stop iterating
            }
        }

        //***Attacking plates***//
        //The pawn can attack the forward diagonal spaces
        /*Spawning left diagonal attack*/
        SpawnPawnAttackPlate(row - 1, col - 1);
        /*Spawning right diagonal attack*/
        SpawnPawnAttackPlate(row - 1, col + 1);
        /*Spawning En Passant attack*/
        SpawnPawnEnPassant(row - 1);
    }
    private void SpawnPawnAttackPlate(int row, int col)
    {
        //first we check if there is an enemy piece in those positions
        if (board.BoardHasPiece(row, col))
        {
            if (board.GetPieceAtPosition(row, col).GetComponent<Chessman>().player != player)//check if the piece is an enemy
            {
                if (row == 7 || row == 0)//both cases of promoting
                    SpawnPlate(row, col, "Promote");//Spawning the plate
                else
                    SpawnPlate(row, col, "Attack");//Spawning the plate
            }
        }
    }
    //Determines an En Passant movement
    private void SpawnPawnEnPassant(int moveRow)
    {
        //if the last piece moveded was a pawn
        if(gameMaster.GetLastMoveData("type").ToString() == "Pawn" && (bool)gameMaster.GetLastMoveData("firstMove"))
        {
            //determine if the position where the pawn was moved is on the sides of this pawn
            if((int)gameMaster.GetLastMoveData("row") == row)//fist checking if its on the same row
            {
                //Then on which side the pawn is
                if((int)gameMaster.GetLastMoveData("col") == col - 1)
                {
                    SpawnPlate(moveRow, col - 1, "EnPassant",board.GetPieceAtPosition(row,col-1));
                }else if ((int)gameMaster.GetLastMoveData("col") == col + 1)
                {
                    SpawnPlate(moveRow, col + 1, "EnPassant", board.GetPieceAtPosition(row, col + 1));
                }
            }
        }
        
    }
}
