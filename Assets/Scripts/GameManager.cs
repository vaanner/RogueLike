using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    
    public int level = 1;
    public int food = 100;
    public List<Enemy> enemyList = new List<Enemy>();
    private static GameManager _instance;
    private bool sleepStep = true;
	// Use this for initialization
    void Awake()
    {
        _instance = this;
    }
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void AddFood(int count)
    {
        food += count;
    }
    public void ReduceFood(int count)
    {
        food -= count;
    }
    public void OnPlayerMove()
    {
        if(sleepStep == true)
        {
            sleepStep = false;
        }
        else
        {
            foreach(var enemy in enemyList)
            {
                enemy.Move();
            }
            sleepStep = true;
        }
    }
}
