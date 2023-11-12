using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private List<BasicCollectible> collectiblesList;

    [SerializeField]
    private List<Damagable> damagablesList;

    

    void Start()
    {
        Screen.SetResolution(3840, 2160, false);
        SetDataToCollectibles();
        SetDataToDamagables();
    }

    private void SetDataToDamagables()
    {
        var id = 0;
        foreach (var damagablePrefab in damagablesList)
        {
            var damagableType = GetRandomDamagableType();
            damagablePrefab.SetData(new Damagable.DamagableData(id++, damagableType,
            Damagable.DamageForCollectibleType(damagableType)
            ));
        }
    }

    private void SetDataToCollectibles()
    {
        var id = 0;
        foreach (var collectiblePrefab in collectiblesList)
        {
            var collectibleType = GetRandomCollectibleType();
            collectiblePrefab.SetData(new BasicCollectible.CollectibleData(id++, collectibleType,
            BasicCollectible.PointsForCollectibleType(collectibleType)
            ));
        }
    }

    private static BasicCollectible.CollectibleType GetRandomCollectibleType()
    {
        var random = new System.Random();
        var collectiblesValues = Enum.GetValues(typeof(BasicCollectible.CollectibleType));
        int randomIndexOfTypeEnum = random.Next(0, collectiblesValues.Length);
        return (BasicCollectible.CollectibleType)collectiblesValues.GetValue(randomIndexOfTypeEnum);
    }

    private static Damagable.DamagableType GetRandomDamagableType()
    {
        var random = new System.Random();
        var damagableValues = Enum.GetValues(typeof(Damagable.DamagableType));
        int randomIndexOfTypeEnum = random.Next(0, damagableValues.Length);
        return (Damagable.DamagableType)damagableValues.GetValue(randomIndexOfTypeEnum);
    }

    public void DestroyCollectibleWithId(int id)
    {
        var collectibleToDestroy = collectiblesList.Find(collectible => collectible.Data.Id == id);
        Destroy(collectibleToDestroy.gameObject);
    }
}