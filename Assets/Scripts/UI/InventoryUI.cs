using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class InventoryUI : MonoBehaviour
{
    [Inject] private Sounds sounds;
    [Inject] private Inventory inventory;
    [Inject] private ItemManager itemManager;
    [Inject] private ItemActivation itemActivation;


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

    [Header("Highlight for quickbar")]
    [SerializeField] private GameObject numbers;
    [SerializeField] private GameObject backs;


    private ObjectPool cellItemPool;
    private Translation lang;
    private List<GameObject> shownItems = new List<GameObject>();
    private ItemCellUI lastOutlined;

    private PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
    private List<RaycastResult> raycastResultList = new List<RaycastResult>();

    private bool isItemGrabed;
    private ItemCellUI itemGrabed;
    private RectTransform itemGrabedRect;
    private Vector3 itemGrabedPosition;
    private Vector3 mouseLastPosition;



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

        if (Globals.IsMobile)
        {
            numbers.SetActive(false);
            backs.SetActive(false);
        }
        else
        {
            numbers.SetActive(true);
            backs.SetActive(true);
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isItemGrabed)
        {
            pointerEventData.position = Input.mousePosition;
            raycastResultList.Clear();
            EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

            if (raycastResultList.Count > 0)
            {
                for (global::System.Int32 i = 0; i < raycastResultList.Count; i++)
                {
                    if (raycastResultList[i].gameObject.layer == 11)
                    {
                        ItemCellUI item = raycastResultList[i].gameObject.GetComponent<ItemCellUI>();
                        itemGrabed = item;
                        itemGrabedRect = itemGrabed.GetComponent<RectTransform>();
                        isItemGrabed = true;
                        mouseLastPosition = Input.mousePosition;
                        itemGrabedPosition = Input.mousePosition;
                        itemGrabed.transform.parent = transform.parent;
                        sounds.InventoryTakeSound();
                        if (lastOutlined != null)
                        {
                            lastOutlined.Setoutline(false);
                        }
                        //item.Setoutline(true);
                        showItemInfo(item.ItemCell);
                        lastOutlined = item;
                    }                    
                }                
            }
        }
        else if (Input.GetMouseButtonUp(0) && isItemGrabed)
        {
            pointerEventData.position = Input.mousePosition;
            raycastResultList.Clear();
            EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

            if (raycastResultList.Count > 0)
            {
                for (global::System.Int32 i = 0; i < raycastResultList.Count; i++)
                {
                    if ((itemGrabedPosition - Input.mousePosition).magnitude < 3)
                    {
                        itemActivation.ActivateItem(itemGrabed.PositionIndex);                        
                        break;
                    }
                    else if (raycastResultList[i].gameObject.layer == 10)
                    {
                        for (global::System.Int32 j = 0; j < baseCells.Length; j++)
                        {
                            if (baseCells[j].Equals(raycastResultList[i].gameObject.transform))
                            {
                                inventory.ReplaceIndex(itemGrabed.PositionIndex, j);
                                sounds.InventoryPutSound();                                
                            }
                        }
                    }      
                    
                }
            }

            isItemGrabed = false;
            recalculateInventory();
        }

        if (isItemGrabed)
        {
            Vector3 delta = Input.mousePosition - mouseLastPosition;
            mouseLastPosition = Input.mousePosition;                        
            itemGrabedRect.anchoredPosition += new Vector2(delta.x, delta.y);
        }
    }

    private void OnEnable()
    {
        recalculateInventory();
    }

    private void recalculateInventory()
    {
        InventoryPosition[] items = inventory.MainInventory.Values.ToArray();
        if (shownItems.Count > 0) shownItems.ForEach(i => cellItemPool.ReturnObject(i));

        for (int i = 0; i < items.Length; i++)
        {
            //print(i + ": " + items[i].ItemID + " - " + items[i].Amount);

            if (items[i].ItemID > 0)
            {
                GameObject g = cellItemPool.GetObject();
                g.transform.localScale = Vector3.one;
                g.SetActive(true);
                Item item = itemManager.GetItemByID(items[i].ItemID);
                g.GetComponent<ItemCellUI>().SetData(item, items[i].Amount, i, baseCells[i].gameObject, inventory.MainInventory[i]);

                baseCells[i].GetChild(1).gameObject.SetActive(items[i].IsEquiped);

                shownItems.Add(g);
            }
            else
            {
                baseCells[i].GetChild(1).gameObject.SetActive(false);
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
        ItemQualityText.text = Item.QualityText(item.Quality);
        ItemDescriptionText.text = lang.ItemsTranslation[item.ID].Description;
        ItemAdditionalInfoText.text = "";
    }

}
