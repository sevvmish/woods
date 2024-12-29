using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCellUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject outline;
    [SerializeField] private GameObject durabilityIndicator;
    [SerializeField] private Image indicator;

    public Item ItemCell {  get; private set; }
    public int Amount { get; private set; }
    public int PositionIndex { get; private set; }
    public GameObject ParentObject { get; private set; }
    public RectTransform Rect {  get; private set; }
    public InventoryPosition ItemData { get; private set; }

    public void SetData(Item i, int amount, int index, GameObject g, InventoryPosition inv)
    {
        ItemData = inv;
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
        if (inv.Durability > 0)
        {
            durabilityIndicator.SetActive(true);
        }
        else
        {
            durabilityIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (ItemData.Durability > 0)
        {
            //print(ItemCell.NameRus + " = " + ItemData.Durability);
            indicator.fillAmount = ItemData.Durability / 100f;
        }
    }

    public void Setoutline(bool isActive) => outline.SetActive(isActive);

    public void ReturnBack()
    {
        transform.parent = ParentObject.transform;
        Rect.anchoredPosition = Vector2.zero;
    }

}
