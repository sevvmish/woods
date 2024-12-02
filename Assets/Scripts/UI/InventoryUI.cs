using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public void SetInventory(Inventory i) => inventory = i;
    private Inventory inventory;
    public void SetItemManager(ItemManager i) => itemManager = i;
    private ItemManager itemManager;
        
    [Header("Base")]
    [SerializeField] private TextMeshProUGUI inventoryText;
    [SerializeField] private TextMeshProUGUI quickBarText;
    [SerializeField] private Transform[] baseCells;
    [SerializeField] private GameObject cellItemExample;

    [Header("Item description menu")]
    [SerializeField] private Image ItemIcon;
    [SerializeField] private TextMeshProUGUI ItemNameText;
    [SerializeField] private TextMeshProUGUI ItemQualityText;
    [SerializeField] private TextMeshProUGUI ItemDescriptionText;
    [SerializeField] private TextMeshProUGUI ItemAdditionalInfoText;


    private ObjectPool cellItemPool;
    private Translation lang;
    private List<GameObject> shownItems = new List<GameObject>();
    private ItemCellUI lastOutlined;
    private PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

    public void Init()
    {
        lang = Globals.Language;
        
        inventoryText.text = lang.Inventory;
        quickBarText.text = lang.QuickBar;

        cellItemPool = new ObjectPool(32, cellItemExample, transform);

        ItemIcon.gameObject.SetActive(false);
        ItemNameText.text = "";
        ItemQualityText.text = "";
        ItemDescriptionText.text = "";
        ItemAdditionalInfoText.text = "";
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> raycastResultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

            if (raycastResultList.Count > 0)
            {
                for (global::System.Int32 i = 0; i < raycastResultList.Count; i++)
                {
                    if (raycastResultList[i].gameObject.layer == 11)
                    {
                        ItemCellUI item = raycastResultList[i].gameObject.GetComponent<ItemCellUI>();
                        showItemInfo(item.ItemCell);
                        if (lastOutlined != null)
                        {
                            lastOutlined.Setoutline(false);
                        }
                        item.Setoutline(true);
                        lastOutlined = item;
                    }                    
                }                
            }
        }
        
        
    }

    private void OnEnable()
    {
        recalculateInventory();
    }

    private void recalculateInventory()
    {
        InventoryPosition[] items = inventory.GetAllInventory;
        if (shownItems.Count > 0) shownItems.ForEach(i => cellItemPool.ReturnObject(i));

        for (int i = 0; i < items.Length; i++)
        {
            //print(i + ": " + items[i].ItemID + " - " + items[i].Amount);

            if (items[i].ItemID > 0)
            {
                GameObject g = cellItemPool.GetObject();
                g.SetActive(true);
                Item item = itemManager.GetItemByID(items[i].ItemID);
                g.GetComponent<ItemCellUI>().SetData(item, items[i].Amount, i, baseCells[i].gameObject);
                shownItems.Add(g);
            }
        }
    }

        
    public void Clean()
    {
        shownItems.ForEach(i => cellItemPool.ReturnObject(i));
    }

    private void showItemInfo(Item item)
    {
        ItemIcon.gameObject.SetActive(true);
        ItemIcon.sprite = item.UISprite;

        ItemNameText.text = lang.ItemsTranslation[item.ID].Name;
        ItemQualityText.text = item.Quality.ToString();
        ItemDescriptionText.text = lang.ItemsTranslation[item.ID].Description;
        ItemAdditionalInfoText.text = "";
    }

}
