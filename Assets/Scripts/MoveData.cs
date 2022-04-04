using System;
using UnityEngine;

[Serializable]
public class MoveData
{
    [SerializeField] private int _num;
    [SerializeField] private int _time;
    [SerializeField] private int _kegelsFall;
    [SerializeField]  private int _score;
    
    public int Num { get => _num; set => _num = value; } // Move number
    public int Time { get => _time; set => _time = value; } // Move duraction (sec)
    public int KegelsFall { get => _kegelsFall; set => _kegelsFall = value; }
    public int Score { get => _score; set => _score = value; }
}