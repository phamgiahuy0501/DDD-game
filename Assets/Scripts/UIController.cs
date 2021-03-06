using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject UIInventoryObject;
    public GameObject UIEquipment;
    private bool isOpenUIInventory;
    private bool isOpenUIEquipment;
    private void Awake()
    {

    }
    private void Start()
    {
        isOpenUIInventory = false;
        isOpenUIEquipment = false;
        UIInventoryObject.SetActive(isOpenUIInventory);
        UIEquipment.SetActive(isOpenUIEquipment);
    }
    private void Update()
    {
        #region Inventory controller
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpenUIInventory = !isOpenUIInventory;
            UIInventoryObject.SetActive(isOpenUIInventory);
        }
        #endregion
        #region Equipment window controller
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            isOpenUIEquipment = !isOpenUIEquipment;
            UIEquipment.SetActive(isOpenUIEquipment);
        }
        #endregion
    }
}
