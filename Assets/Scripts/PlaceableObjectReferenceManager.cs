using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

public class PlaceableObjectReferenceManager : MonoBehaviour
{
    public PlaceableObjectManager placeableObjectManager;

    public void Place(Item item, Vector3Int pos)
    {
        if(placeableObjectManager == null)
        {
            Debug.Log("No placeableObjectsManager reference are detected");
            return;
        }

        placeableObjectManager.Place(item, pos);
    }

    public bool Check(Vector3Int pos)
    {
        if (placeableObjectManager == null)
        {
            Debug.Log("No placeableObjectsManager reference are detected");
            return false;
        }

        return placeableObjectManager.Check(pos);
    }

    internal void PickUp(Vector3Int gridPosition)
    {
        if (placeableObjectManager == null)
        {
            Debug.Log("No placeableObjectsManager reference are detected");
            return;
        }

        placeableObjectManager.PickUp(gridPosition);
    }
}
