using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasicCollectible : MonoBehaviour
{
    public enum CollectibleType
    {
        OnePointer,
        TwoPointer,
        FivePointer,
        TenPointer
    }

    public class CollectibleData
    {
        public int Id { get; }
        public CollectibleType Type { get; }
        public int PointsToGain { get; }

        public CollectibleData(int id, CollectibleType type, int pointsToGain)
        {
            Id = id;
            Type = type;
            PointsToGain = pointsToGain;
        }
    }

    [SerializeField]
    private List<SpriteRenderer> collectiblesTypeSprites;


    public void SetData(CollectibleData collectibleData)
    {
        Data = collectibleData;

        Debug.Log($"Collectible giving {Data.PointsToGain} and type {Data.Type.ToString()} is instantiated.");

        RefreshView();
    }

    private void RefreshView()
    {
        UpdateShowingSprite();
    }

    private void UpdateShowingSprite()
    {
        var componentToShow = GetComponentNameForType(Data.Type);
        foreach (var sprite in collectiblesTypeSprites)
        {
            if(sprite.name != componentToShow)
            {
                Destroy(sprite.gameObject);
            }
        }
    }

    private string GetComponentNameForType(CollectibleType type)
    {
        return type switch
        {
            CollectibleType.OnePointer => "OnePointerSprite",
            CollectibleType.TwoPointer => "TwoPointerSprite",
            CollectibleType.FivePointer => "FivePointerSprite",
            CollectibleType.TenPointer => "TenPointerSprite",
            _ => "TwentyDamageSprite",
        };
    }

    public CollectibleData Data;

    public static int PointsForCollectibleType(CollectibleType collectibleType)
    {
        return collectibleType switch
        {
            CollectibleType.OnePointer => 1,
            CollectibleType.TwoPointer => 2,
            CollectibleType.FivePointer => 5,
            CollectibleType.TenPointer => 10,
            _ => 0,
        };
    }

}
