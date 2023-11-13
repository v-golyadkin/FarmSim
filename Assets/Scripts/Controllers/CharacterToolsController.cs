using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterToolsController : MonoBehaviour
{
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] float maxDistance = 1.5f;
    [SerializeField] ToolAction onTilePickUp;
    [SerializeField] IconHighlight iconHighlight;
    AttackController attackController;

    ToolBarController toolbarController;
    CharacterController2D characterController;
    Character character;
    Rigidbody2D body;
    Animator animator;
    Vector3Int selectedTilePosition;
    bool selectable;


    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        character = GetComponent<Character>();
        body = GetComponent<Rigidbody2D>();
        toolbarController = GetComponent<ToolBarController>();
        animator = GetComponent<Animator>();
        attackController = GetComponent<AttackController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            WeaponAction();
        }


        SelectTile();
        CanSelectCheck();
        Marker();
        if (Input.GetMouseButtonDown(0))
        {
            if(UseToolWorld() == true)
            {
                return;
            }
            UseToolGrid();
        }
    }

    private void WeaponAction()
    {
        Item item = toolbarController.GetItem;
        if(item == null) { return; }
        if (item.isWeapon == false) { return; }

        Vector2 position = body.position + characterController.lastMotionVector * offsetDistance;

        attackController.Attack(item.damage, characterController.lastMotionVector);
    }

    private void EnergyCost(int energyCost)
    {
        Item item = toolbarController.GetItem;

        character.GetTired(energyCost);
    }

    private void SelectTile()
    {
        selectedTilePosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
    }

    void CanSelectCheck()
    {
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
        iconHighlight.CanSelect = selectable;
    }

    private void Marker()
    {
        markerManager.markedCellPosition = selectedTilePosition;
        iconHighlight.cellPosition = selectedTilePosition;
    }

    private bool UseToolWorld()
    {
        Vector2 position = body.position + characterController.lastMotionVector * offsetDistance;

        Item item = toolbarController.GetItem;
        if (item == null) { return false; }
        if(item.onAction == null) { return false; }

        EnergyCost(item.onAction.energyCost);

        animator.SetTrigger("act");
        bool complete = item.onAction.OnApply(position);

        if (complete == true)
        {
            if (item.onItemUsed != null)
            {
                item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);
            }
        }

        return complete;
    }

    private void UseToolGrid()
    {
        if(selectable == true)
        {
            Item item = toolbarController.GetItem;
            if (item == null) 
            {
                PickUpTile();
                
                return; 
            }
            if (item.onTileMapAction == null) { return; }

            EnergyCost(item.onAction.energyCost);

            animator.SetTrigger("act");
            bool complete = item.onTileMapAction.OnApplyToTileMap(selectedTilePosition, tileMapReadController, item);

            if(complete == true)
            {
                if (item.onItemUsed != null)
                {
                    item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);
                }
            }
        }
    }

    private void PickUpTile()
    {
        if(onTilePickUp == null) { return; }

        onTilePickUp.OnApplyToTileMap(selectedTilePosition, tileMapReadController, null);
    }
}
