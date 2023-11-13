using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Text text;
    [SerializeField] private Image highlight;

    private int myIndex;

    ItemPanel itemPanel;

    public void SetItemPanel(ItemPanel source)
    {
        itemPanel = source;
    }

    public void SetIndex(int index)
    {
        myIndex = index;
    }
    public void Set(ItemSlot slot)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;

        if(slot.item.stackable == true)
        {
            text.gameObject.SetActive(true);
            text.text = slot.count.ToString();
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

    public void Clean()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);

        text.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        itemPanel.OnClick(myIndex);
    }

    public void Highlight(bool b)
    {
        highlight.gameObject.SetActive(b);
    }
}
