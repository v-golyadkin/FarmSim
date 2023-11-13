using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableControls : MonoBehaviour
{
    CharacterController2D characterController;
    CharacterToolsController characterToolsController;
    InventoryController inventoryController;
    ToolBarController toolBarController;
    ItemContainerInteractController itemContainerInteractController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        characterToolsController = GetComponent<CharacterToolsController>();
        inventoryController = GetComponent<InventoryController>();
        toolBarController = GetComponent<ToolBarController>();  
        itemContainerInteractController = GetComponent<ItemContainerInteractController>();
    }

    public void DisableControl()
    {
        characterController.enabled = false;
        characterToolsController.enabled = false;
        inventoryController.enabled = false;
        toolBarController.enabled = false;
        itemContainerInteractController.enabled = false;
    }

    public void EnableControl()
    {
        characterController.enabled = true;
        characterToolsController.enabled = true;   
        inventoryController.enabled = true;
        toolBarController.enabled = true;
        itemContainerInteractController.enabled = true;
    }
}
