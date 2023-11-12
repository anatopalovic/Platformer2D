using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public enum DamagableType
    {
        FiveDamage,
        TenDamage,
        TwentyDamage,
    }

    public class DamagableData
    {
        public int Id { get; }
        public DamagableType Type { get; }
        public int HealthDamage { get; }

        public DamagableData(int id, DamagableType type, int healthDamage)
        {
            Id = id;
            Type = type;
            HealthDamage = healthDamage;
        }
    }

    [SerializeField]
    private List<SpriteRenderer> damagableTypeSprites;


    public void SetData(DamagableData damagableData)
    {
        Data = damagableData;

        Debug.Log($"Dabagable giving {Data.HealthDamage} damage and type {Data.Type.ToString()} is instantiated.");

        RefreshView();
    }

    private void RefreshView()
    {
        UpdateShowingSprite();
    }

    private void UpdateShowingSprite()
    {
        var componentToShowOne = GetComponentNameForTypeOne(Data.Type);
        var componentToShowTwo = GetComponentNameForTypeTwo(Data.Type);
        foreach (var sprite in damagableTypeSprites)
        {
            if (sprite.name != componentToShowOne && sprite.name != componentToShowTwo)
            {
                Destroy(sprite.gameObject);
            }
        }
    }

    private string GetComponentNameForTypeOne(DamagableType type)
    {
        return type switch
        {
            DamagableType.FiveDamage => "FiveDamageSprite1",
            DamagableType.TenDamage => "TenDamageSprite1",
            DamagableType.TwentyDamage => "TwentyDamageSprite1",
            _ => "TwentyDamageSprite1",
        };
    }

    private string GetComponentNameForTypeTwo(DamagableType type)
    {
        return type switch
        {
            DamagableType.FiveDamage => "FiveDamageSprite2",
            DamagableType.TenDamage => "TenDamageSpriteOne2",
            DamagableType.TwentyDamage => "TwentyDamageSprite2",
            _ => "TwentyDamageSprite2",
        };
    }

    public DamagableData Data;

    public static int DamageForCollectibleType(DamagableType damagableType)
    {
        return damagableType switch
        {
            DamagableType.FiveDamage => 5,
            DamagableType.TenDamage => 10,
            DamagableType.TwentyDamage => 20,
            _ => 0,
        };
    }

}
