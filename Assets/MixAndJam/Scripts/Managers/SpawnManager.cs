using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Ranch { Meat, Potato, Tomato, Lettuce, Cheddar, Egg }

public class SpawnManager : BehaviourBase
{
    public GameObject ingredientPrefab;

    public int maxIngredientsPerScene;

    public Transform ranch;

    public Transform[] spawnPoints;

    //public Ranch ranch;

    public float spawnTime;

    private float spawnTimer;
    private int ingredientsPerScene;
    
    protected override void CustomFixedUpdate()
    {
        
    }

    protected override void CustomUpdate()
    {
        if (spawnTimer <= 0)
        {
            if (ingredientsPerScene < maxIngredientsPerScene)
            {
                Spawn();
                spawnTimer = spawnTime;
            } else
            {
                spawnTimer = spawnTime;
            }
        } else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    private void Spawn()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject ingredient = Instantiate(ingredientPrefab, spawnPoint.position - new Vector3(0, 3, 0), spawnPoint.rotation, ranch);
        ingredient.GetComponent<SuckableObject>().currentSpawner = this;
        OnGameObjectSpawned?.Invoke(ingredient);
        ingredientsPerScene++;
    }

    public void RemoveIngredient()
    {
        ingredientsPerScene--;
    }

    public Action<GameObject> OnGameObjectSpawned;
}
