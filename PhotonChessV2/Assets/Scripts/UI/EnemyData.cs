using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [SerializeField] TextMeshPro opponentName;
    [SerializeField] TextMeshPro numMatches;
    [SerializeField] TextMeshPro winRate;

    public void FillLabels(string displayName, string numMatches, string winRate)
    {
        opponentName.text = displayName;
        this.numMatches.text = numMatches;
        this.winRate.text = winRate;
    }
}
