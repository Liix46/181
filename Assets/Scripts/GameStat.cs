using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameStat
{
    [SerializeField] public static List<MoveData> Moves = new();
    [SerializeField] public static List<MoveData> BestMoves = new();

    public static bool IsRecord
    {
        get
        {
            int total = 0, bestTotal = 0;
            foreach (MoveData move  in Moves) total += move.Score;
            foreach (MoveData move in BestMoves) bestTotal += move.Score;
            return total > bestTotal;
        }
    }
}
