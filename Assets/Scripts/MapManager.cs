using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MapManager : MonoBehaviour {

    //外墙
    public GameObject[] outWallArray;
    //背景
    public GameObject[] floorWallArray;
    //障碍物
    public GameObject[] wallArray;
    //食物
    public GameObject[] foodArray;
    //怪物
    public GameObject[] enemyArray;
    //出口 prefab
    public GameObject exitPrefab;
    //游戏控制脚本
    public GameManager gameManager;
    //障碍物 最小数量
    public int minWallNum = 2;
    //障碍物 最大数量
    public int maxWallNum = 5;
    //记录可能产生障碍物的区域位置信息
    private List<Vector2> itemsPosition = new List<Vector2>();
    //将生成的子对象放在 map 物体下
    private Transform mapHolder;
    //地图 行数
    public int rows = 10;
    //地图 列数
    public int cols = 10;
	// Use this for initialization
	void Start () 
    {
        gameManager = this.GetComponent<GameManager>();
	    InitMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //生成地图
    private void InitMap()
    {
        mapHolder = new GameObject("Map").transform;
        InitBackGround(mapHolder);
        InitItems(mapHolder);
    }
    ///初始化地图背景
    private void InitBackGround(Transform mapHolder)
    {
         for(int row = 0;row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if(row == 0 || col == 0 || row == rows-1 || col == cols -1)
                {
                    GameObject wall = RandomPrefabs(outWallArray);
                    wall = GameObject.Instantiate(wall,new Vector3(row,col,0),Quaternion.identity) as GameObject;
                    wall.transform.SetParent(mapHolder);
                }
                else
                {
                    GameObject wall = RandomPrefabs(floorWallArray);
                    wall =  GameObject.Instantiate(wall,new Vector3(row,col,0),Quaternion.identity) as GameObject;
                    wall.transform.SetParent(mapHolder);
                }
            }
        }
    }
    ///初始化障碍物
    private void InitItems(Transform mapHolder)
    {
        InitPositions();
        ///随机生成障碍物数量
        int wallNum = Random.Range(minWallNum,maxWallNum);
        InstantiateItems(wallNum,wallArray,mapHolder);
        //随机生成植物
        int foodCount = Random.Range(2,gameManager.level * 2+1);
        InstantiateItems(foodCount,foodArray,mapHolder);
        //创建敌人
        int enemyCount = gameManager.level / 2 + 1;
        InstantiateItems(enemyCount,enemyArray,mapHolder);
        //创建出口
        GameObject exit = Instantiate(exitPrefab,new Vector2(rows-2,cols-2),Quaternion.identity) as GameObject;
        exit.transform.SetParent(mapHolder);
    }
    private void InstantiateItems(int count,GameObject[] prefabs,Transform mapHolder)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = RandomPosition();
            GameObject obj = RandomPrefabs(prefabs);
            obj = GameObject.Instantiate(obj,pos,Quaternion.identity) as GameObject;
            obj.transform.SetParent(mapHolder);
        }
    }
    ///获取可放置的随机坐标数组
    private void InitPositions()
    {
        itemsPosition.Clear();
        for (int row = 2; row < rows-2; row++)
        {
            for (int col = 2; col < cols-2; col++)
            {
                itemsPosition.Add(new Vector2(row,col));
            }
        }
    }
    ///生成随机坐标
    private Vector2 RandomPosition()
    {
         int wallIndex  = Random.Range(0,itemsPosition.Count);
         Vector2 wallVec = itemsPosition[wallIndex];
         itemsPosition.RemoveAt(wallIndex);
         return wallVec;
    }
    //随机生成 prefab
    private GameObject RandomPrefabs(GameObject[] prefabs)
    {
        int index = Random.Range(0,prefabs.Length);
        return prefabs[index];
    }
}
