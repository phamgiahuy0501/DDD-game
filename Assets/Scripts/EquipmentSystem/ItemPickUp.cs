using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Player")
        {
            if (!Inventory.instance.isContainItem(item))
            {
                pickUp();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private void pickUp()
    {
        // if (EquipmentSystem.instance != null)
        // {
        //     EquipmentSystem.instance.equipItem(item);
        // }
        if (Inventory.instance != null)
        {
            Inventory.instance.addItem(item);
        }
        Destroy(gameObject);
    }
}
