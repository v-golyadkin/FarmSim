using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))] 
public class ResourceNode : ToolHit
{
    [SerializeField] private GameObject _pickUpDrop;
    [SerializeField] private float _spread = 0.7f;

    [SerializeField] private Item _item;
    [SerializeField] private int _dropCount = 4;
    [SerializeField] private int _itemCountInOneDrop = 1;
    [SerializeField] private ResourceNodeType nodeType;



    public override void Hit()
    {
        while (_dropCount > 0)
        {
            _dropCount--;

            Vector3 _position = transform.position;
            _position.x += _spread * UnityEngine.Random.value - _spread / 2;
            _position.y += _spread * UnityEngine.Random.value - _spread / 2;
            
            ItemSpawnManager.instance.SpawnItem(_position, _item, _itemCountInOneDrop);
        }
        
        Destroy(gameObject);
    }

    public override bool CanBeHit(List<ResourceNodeType> canBeHit)
    {
        return canBeHit.Contains(nodeType);
    }
}
