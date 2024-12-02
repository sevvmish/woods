using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

       

    private void OnEnable()
    {
        
        InventoryPosition[] items = inventory.GetAllInventory;

        for (int i = 0; i < items.Length; i++)
        {
            //print(i + ": " + items[i].ItemID + " - " + items[i].Amount);

            if (items[i].ItemID > 0)
            {
                GameObject g = cellItemPool.GetObject();
                g.SetActive(true);

                Item item = itemManager.GetItemByID(items[i].ItemID);

                g.transform.GetChild(0).GetComponent<Image>().sprite = item.UISprite;
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].Amount.ToString();

                
                g.transform.parent = baseCells[i].transform;
                g.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                g.transform.localScale = Vector3.one;
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
        ItemIcon.gameObject.SetActive(false);
        ItemIcon.sprite = item.UISprite;

        ItemNameText.text = lang.ItemsTranslation[item.ID].Name;
        ItemQualityText.text = item.Quality.ToString();
        ItemDescriptionText.text = lang.ItemsTranslation[item.ID].Description;
        ItemAdditionalInfoText.text = "";
    }

}
