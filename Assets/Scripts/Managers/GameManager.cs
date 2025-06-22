using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }



    int CollectedCoins = 0;
    int CollectedBerries = 0;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddCollectedCoins(int Num)
    {
        CollectedCoins += Num;
    }

    public void AddCollectedBerries(int Num)
    {
        CollectedBerries += Num;
    }

    Transform CurrentCheckPoint;
    public void SetCurrentCheckPoint(Transform newPoint)
    {
        CurrentCheckPoint = newPoint;
    }
    public Transform GetCurrentCheckPoint()
    {
        return CurrentCheckPoint;
    }
}
