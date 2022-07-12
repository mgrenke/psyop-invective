using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    public int slots = 20;
    public List<Item> items;

    public bool Add(Item item)
    {
        if (items.Count >= slots)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        items.Add(item);
        
        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        
        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public bool HasItem(string requiredItemName, bool takeItem)
    {
        for (var i = 0; i < items.Count; i++)
        {
            if (String.CompareOrdinal(items[i].name, requiredItemName) == 0)
            {
                if (takeItem)
                    instance.Remove(items[i]);
                return true;
            }
        }

        return false;
    }
}
