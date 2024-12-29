using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class QuickBarUI : MonoBehaviour
{
    [Inject] private Inventory inventory;
    [Inject] private ItemManager itemManager;
    [Inject] private ItemActivation itemActivation;

    [SerializeField] private GameObject numbers;
    [SerializeField] private Transform[] places;
    [SerializeField] private GameObject cellItemExample;

    private ObjectPool cellItemPool;
    private List<GameObject> shownItems = new List<GameObject>();

    private PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
    private List<RaycastResult> raycastResultList = new List<RaycastResult>();


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

    private void Update()
    {
        if (Globals.IsMobile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pointerEventData.position = Input.mousePosition;
                raycastResultList.Clear();
                EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

                if (raycastResultList.Count > 0)
                {
                    for (global::System.Int32 i = 0; i < raycastResultList.Count; i++)
                    {
                        if (raycastResultList[i].gameObject.layer == 12)
                        {
                            activateItem(raycastResultList[i].gameObject.transform);
                        }
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                activateItem(places[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                activateItem(places[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                activateItem(places[2]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                activateItem(places[3]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                activateItem(places[4]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                activateItem(places[5]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                activateItem(places[6]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                activateItem(places[7]);
            }
        }
        
    }

    private void activateItem(Transform t)
    {
        for (global::System.Int32 i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).TryGetComponent(out ItemCellUI item))
            {
                itemActivation.ActivateItem(item.PositionIndex);
                break;
            }
        }        
    }

    public void RecalculateBar()
    {
        InventoryPosition[] items = inventory.MainInventory.Values.ToArray();
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
                g.GetComponent<ItemCellUI>().SetData(item, items[i].Amount, i, places[i].gameObject, inventory.MainInventory[i]);

                places[i].GetChild(1).gameObject.SetActive(items[i].IsEquiped);
                

                shownItems.Add(g);
            }
            else
            {
                places[i].GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
