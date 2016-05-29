using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject gameManager;
	// Use this for initialization
    void Awake()
    {
        if(GameManager.Instance == null)
        {
            GameObject.Instantiate(gameManager);
        }
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
