using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.Instance.maxGameTime / spawnData.Length;
    }

    private void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min( Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnData.Length -1);

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            Spawn();
            
        }

        
    }
    void Spawn()
    {
       GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
