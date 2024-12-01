using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMediator : ISubject
{
    List<IObserver> observers;
    GameData Data;

    public DataMediator()
    {
        observers = new List<IObserver>();
        Data = new GameData();
    }
    public DataMediator(GameData data)
    {
        observers = new List<IObserver>();
        Data = data;
        Notify();
    }
    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }
    public void Dettach(IObserver observer)
    {
        observers.Remove(observer);
    }
    public void Notify()
    {
        foreach (IObserver observer in observers)
        {
            observer.UpdateData(Data);
        }
    }

    public void SetLevelName (string levelName)
    {
        Data.LevelName = levelName;
        Notify();
    }
    public void SetObsticlesCount(int count)
    {
        Data.ObsticlesCount = count;
        Notify();
    }
    public void SetMaxBombCount(int count)
    {
        Data.MaxBombCount = count;
        Notify();
    }
    public void SetPlacedBombCount(int count)
    {
        Data.PlacedBombCount = count;
        Notify();
    }
    public void SetExplotionRange(int range)
    {
        Data.ExplotionRange = range;
        Notify();
    }
    public void SetBonusPlayerSpeed(int speed)
    {
        Data.BonusPlayerSpeed = speed;
        Notify();
    }
}
