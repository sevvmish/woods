using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class QuickBarUI : MonoBehaviour
{
    [Inject] private Inventory inventory;
    [Inject] private ItemManager itemManager;

    [SerializeField] private GameObject numbers;
    [SerializeField] private Transform[] places;
    [SerializeField] private GameObject cellItemExample;

    private ObjectPool cellItemPool;
    private List<GameObject> shownItems = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        cellItemPool = new ObjectPool(8, cellItemExample, transform);

        if (Globals.IsMobile)
        {
            numbers.SetActive(false);
            transform.GetChild(0).localScale = Vector3.one * 1.15f;
            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(40, 20);
        }
        else
        {
            numbers.SetActive(true);
            transform.GetChild(0).localScale = Vector3.one * 0.9f;
        }

        
        inventory.OnInventoryChanged += RecalculateBar;
        RecalculateBar();
    }

    public void RecalculateBar()
    {
        InventoryPosition[] items = inventory.GetAllInventory;
        if (shownItems.Count > 0) shownItems.ForEach(i => cellItemPool.ReturnObject(i));

        for (int i = 0; i < 8; i++)
        {
            //print(i + ": " + items[i].ItemID + " - " + items[i].Amount);

            if (items[i].ItemID > 0)
            {
                GameObject g = cellItemPool.GetObject();
                g.transform.localScale = Vector3.one;
                g.SetActive(true);
                Item item = itemManager.GetItemByID(items[i].ItemID);
                g.GetComponent<ItemCellUI>().SetData(item, items[i].Amount, i, places[i].gameObject);
                shownItems.Add(g);
            }
        }
    }
}
