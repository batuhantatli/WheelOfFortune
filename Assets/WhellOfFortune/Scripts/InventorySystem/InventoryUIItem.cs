using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WhellOfFortune.Scripts.InventorySystem;

public class InventoryUIItem : MonoBehaviour
{
    private BaseInventoryItemData _inventoryData; 
    
    private InventoryController _inventoryController;

    public Image image;
    public TMP_Text countText;
    
    public BaseInventoryItemData InventoryData => _inventoryData;
    
    public virtual void Initialize(InventoryController inventory,BaseInventoryItemData inventoryItemData , int count)
    {
        _inventoryController = inventory;
        SetItemIcon(inventoryItemData.icon);
        SetItemCountText(count);
    }

    public void SetItemIcon(Sprite icon)
    {
        image.sprite = icon;
    }

    public void SetItemCountText( int count)
    {
        countText.text = count.ToString();
        
    }

    public virtual void AddItem( int count)
    {
        _inventoryData.UserData.count += count;
        _inventoryData.SaveUserData();
    }

    public virtual void Destroy()
    {
        Destroy(this);
    }
}
