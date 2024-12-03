using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCellUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject outline;

    public Item ItemCell {  get; private set; }
    public int Amount { get; private set; }
    public int PositionIndex { get; private set; }
    public GameObject ParentObject { get; private set; }
    public RectTransform Rect {  get; private set; }

    public void SetData(Item i, int amount, int index, GameObject g)
    {
        ItemCell = i;
        Amount = amount;
        PositionIndex = index;
        ParentObject = g;
        Rect = GetComponent<RectTransform>();
        image.sprite = ItemCell.UISprite;

        if (ItemCell.MaxStack > 1)
        {
            amountText.text = Amount.ToString() + "/" + ItemCell.MaxStack;
        }
        else
        {
            amountText.text = "";
        }
        
        transform.parent = ParentObject.transform;
        Rect.anchoredPosition = Vector2.zero;
        transform.localScale = Vector2.one;
        outline.SetActive(false);
    }

    public void Setoutline(bool isActive) => outline.SetActive(isActive);

    public void ReturnBack()
    {
        transform.parent = ParentObject.transform;
        Rect.anchoredPosition = Vector2.zero;
    }

}
