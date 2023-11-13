using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    [SerializeReference] HighlightController highlightController;
    CharacterController2D characterController;
    Rigidbody2D body;
    Character character;
    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        body = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }
    private void Update()
    {
        Check();
        
        if (Input.GetMouseButtonDown(1))
        {
            Interact();
        }
    }

    public void Check()
    {
        Vector2 position = body.position + characterController.lastMotionVector * offsetDistance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D c in colliders)
        {
            Interactable interact = c.GetComponent<Interactable>();
            if (interact != null)
            {
                highlightController.HightlightGameObject(interact.gameObject);

                return;
            }
        }

        highlightController.Hide();
    }

    public void Interact()
    {
        Vector2 position = body.position + characterController.lastMotionVector * offsetDistance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D c in colliders)
        {
            Interactable interact = c.GetComponent<Interactable>();
            if (interact != null)
            {
                interact.Interact(character);
                break;
            }
        }
    }
}
