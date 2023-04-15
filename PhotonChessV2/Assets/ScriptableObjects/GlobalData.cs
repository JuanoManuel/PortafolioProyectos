using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New global data",menuName = "Dependencies/Data Storage")]
public class GlobalData : ScriptableObject
{
    public string userKey { get; set; }
    public string DisplayName { get; set; }
    public int numMatches { get; set; }
    public double winRate { get; set; }
    public string historialKey { get; set; }

    public MatchData matchData { get; set; }
}
