using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman
{
    public override void ShowMovements()
    {
        //Destroy all the previous plates
        gameMaster.DestroyAllPlates();
        //The knight moves in L//
        SpawnPlateAtPoint(row + 2, col + 1);
        SpawnPlateAtPoint(row + 2, col - 1);
        SpawnPlateAtPoint(row + 1, col + 2);
        SpawnPlateAtPoint(row - 1, col + 2);
        SpawnPlateAtPoint(row - 2, col + 1);
        SpawnPlateAtPoint(row - 2, col - 1);
        SpawnPlateAtPoint(row + 1, col - 2);
        SpawnPlateAtPoint(row - 1, col - 2);
    }
}
