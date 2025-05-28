using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PlayerInventory : SingleTon<PlayerInventory>
{
    private Dictionary<ResourceType, int> _resourceDict = new();
    public event Action<Sprite, ResourceType, Category, int> OnResourceChanged;

    void Start()
    {
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            _resourceDict[type] = 0;
        }
    }

    public void AddResource(Sprite icon, ResourceType type, Category category, int amount)
    {
        if (_resourceDict.ContainsKey(type))
        {
            _resourceDict[type] += amount;
        }
        else
        {
            _resourceDict[type] = amount;
        }

        // UI에 현재 수량 전달
        OnResourceChanged?.Invoke(icon, type, category, _resourceDict[type]);
    }

    public int GetAmount(ResourceType type)
    {
        return _resourceDict.TryGetValue(type, out int val) ? val : 0;
    }
}
