using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Chessman
{
    public override void ShowMovements()
    {
        //Destroy all the previous plates
        gameMaster.DestroyAllPlates();
        //The rooks can move left,right,up and down
        LinealSpawnPlate(-1, 0);
        LinealSpawnPlate(1, 0);
        LinealSpawnPlate(0, -1);
        LinealSpawnPlate(0, 1);
    }
}
