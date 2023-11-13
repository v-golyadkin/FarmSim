using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _pickUpDistance = 1.5f;
    [SerializeField] private float _timeToLive = 10f;

    public Item item;
    private Transform _player;

    public int count = 1;

    private void Awake()
    {
        _player = GameManager.instance.player.transform;
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = item.icon;
    }
    public void Update()
    {
        _timeToLive -= Time.deltaTime;
        if(_timeToLive < 0)
        {
            Destroy(gameObject);
        }
        
        float _distance = Vector3.Distance(transform.position, _player.position);
        if( _distance > _pickUpDistance)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _player.position, _speed * Time.deltaTime);

        if(_distance < 0.1f)
        {
            if(GameManager.instance.inventoryContainer != null)
            {
                GameManager.instance.inventoryContainer.Add(item, count);
            }
            else
            {
                Debug.LogWarning("No inventory container attached to the game manager");
            }

            Destroy(gameObject);
        }
    }
}
