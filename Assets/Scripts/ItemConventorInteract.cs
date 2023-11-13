using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemConventorData
{
    public ItemSlot itemSlot;
    public int timer;

    public ItemConventorData()
    {
        itemSlot = new ItemSlot();
    }
}


[RequireComponent(typeof(TimeAgent))]
public class ItemConventorInteract : Interactable, IPersistance
{
    [SerializeField] Item convertableItem;
    [SerializeField] Item producedItem;
    [SerializeField] int producetItemCount;

    ItemConventorData data;

    [SerializeField] int timeToProcess = 5;

    Animator animator;

    public void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += ItemConvertProcess;

        if(data == null)
        {
            data = new ItemConventorData();
        }
        animator = GetComponent<Animator>();
        Animate();
    }

    private void ItemConvertProcess(DayTimeController dayTimeController)
    {
        if (data.itemSlot == null) { return; }
        if (data.timer > 0)
        {
            data.timer -= 1;
            if (data.timer <= 0)
            {
                CompleteItemConversion();
            }
        }
    }

    public override void Interact(Character character)
    {
        if(data.itemSlot.item == null)
        {
            if (GameManager.instance.dragAndDropController.Check(convertableItem))
            {
                StartItemProcessing(GameManager.instance.dragAndDropController.itemSlot);
                return;
            }

            ToolBarController toolBarController = character.GetComponent<ToolBarController>();
            if (toolBarController == null) { return; }

            ItemSlot itemSlot = toolBarController.GetItemSlot;

            if(itemSlot.item == convertableItem)
            {
                StartItemProcessing(itemSlot);
                return;
            }
        }
        
        if (data.itemSlot.item != null && data.timer <= 0f)
        {
            GameManager.instance.inventoryContainer.Add(data.itemSlot.item, data.itemSlot.count);
            data.itemSlot.Clear();
        }

    }

    private void StartItemProcessing(ItemSlot toProcess)
    {
        data.itemSlot.Copy(GameManager.instance.dragAndDropController.itemSlot);
        data.itemSlot.count = 1;
        if (toProcess.item.stackable)
        {
            toProcess.count -= 1;
            if(toProcess.count < 0)
            {
                toProcess.Clear();
            }
        }
        else
        {
            toProcess.Clear();
        }

        data.timer = timeToProcess;
        Animate();
    }

    private void Animate()
    {
        animator.SetBool("Working", data.timer > 0f);
    }

    private void CompleteItemConversion()
    {
        Animate();
        data.itemSlot.Clear();
        data.itemSlot.Set(producedItem, producetItemCount);
    }

    public string Read()
    {
        return JsonUtility.ToJson(data);
    }

    public void Load(string jsonString)
    {
        data = JsonUtility.FromJson<ItemConventorData>(jsonString);
    }
}
