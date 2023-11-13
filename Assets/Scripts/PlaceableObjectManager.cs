using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class PlaceableObjectManager : MonoBehaviour
{
    [SerializeField] PlaceableObjectsContainer placeableObjects;
    [SerializeField] Tilemap targetTilemap;

    public void Start()
    {
        GameManager.instance.GetComponent<PlaceableObjectReferenceManager>().placeableObjectManager = this;
        VisualizeMap();
    }

    private void VisualizeMap()
    {
        for(int i = 0; i < placeableObjects.placeableObjects.Count; i++)
        {
            VisualizeItem(placeableObjects.placeableObjects[i]);
        }
    }

    private void OnDestroy()
    {
        for(int i = 0; i < placeableObjects.placeableObjects.Count; i++)
        {
            if (placeableObjects.placeableObjects[i].targetObject == null) { continue; }
            
            IPersistance persistance = placeableObjects.placeableObjects[i].targetObject.GetComponent<IPersistance>();
            if(persistance != null)
            {
                string jsonString = persistance.Read();
                placeableObjects.placeableObjects[i].objectState = jsonString;
            }

            placeableObjects.placeableObjects[i].targetObject = null;
        }
    }
    private void VisualizeItem(PlaceableObject placeableObject)
    {
        GameObject go = Instantiate(placeableObject.placedItem.itemPrefab);
        go.transform.parent = transform;

        Vector3 position = targetTilemap.CellToWorld(placeableObject.positionOnGrid) + targetTilemap.cellSize / 2;
        position -= Vector3.forward * 0.1f;
        go.transform.position = position;

        IPersistance persistance = go.GetComponent<IPersistance>();
        if(persistance != null)
        {
            persistance.Load(placeableObject.objectState);
        }

        placeableObject.targetObject = go.transform;
    }

    public bool Check(Vector3Int position)
    {
        return placeableObjects.Get(position) != null;
    }

    public void Place(Item item, Vector3Int positionOnGrid)
    {
        if(Check(positionOnGrid) == true) { return; }
        PlaceableObject placeableObject = new PlaceableObject(item, positionOnGrid);
        VisualizeItem(placeableObject);
        placeableObjects.placeableObjects.Add(placeableObject);
    }

    internal void PickUp(Vector3Int gridPosition)
    {
        PlaceableObject placedObject = placeableObjects.Get(gridPosition);

        if(placedObject == null)
        {
            return;
        }

        ItemSpawnManager.instance.SpawnItem(targetTilemap.CellToWorld(gridPosition), placedObject.placedItem, 1);

        Destroy(placedObject.targetObject.gameObject);

        placeableObjects.Remove(placedObject);
    }
}
