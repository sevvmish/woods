using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CharacterPanelUI : MonoBehaviour
{
    [Inject] private Inventory inventory;
    [Inject] private ItemManager itemManager;

    public bool IsMainPanelOpened => mainPanel.activeSelf;

    [Header("Base")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button chooseInventoryButton;
    [SerializeField] private TextMeshProUGUI inventoryText;
    [SerializeField] private Button chooseCraftingButton;
    [SerializeField] private TextMeshProUGUI craftingText;
    [SerializeField] private Button chooseMapButton;
    [SerializeField] private TextMeshProUGUI mapText;
    private Color upperButtonOutlined = new Color(100f/255f, 161f/255f, 219f/255f, 1);
    private Color upperButtonNonOutlined = new Color(1, 1, 0, 1);

    [Header("Inventory")]
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GameObject inventoryPanel;

    [Header("Sounds")]
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private void Start()
    {
        inventoryUI.SetInventory(inventory);
        inventoryUI.Init();

        inventoryUI.SetItemManager(itemManager);

        mainPanel.SetActive(false);

        inventoryText.text = Globals.Language.Inventory;
        craftingText.text = Globals.Language.Crafting;
        mapText.text = Globals.Language.Map;

        closeButton.onClick.AddListener(() => 
        {
            CloseCharacterPanel();
        });

        chooseInventoryButton.onClick.AddListener(() =>
        {
            OpenInventory();
        });
    }

    public void OpenInventory()
    {
        mainPanelActivation(true);
        inventoryPanel.SetActive(true);        
        turnOnButton(chooseInventoryButton.gameObject);
        turnOffButton(chooseCraftingButton.gameObject);
        turnOffButton(chooseMapButton.gameObject);
        Globals.IsOptions = true;

        if (!Globals.IsMobile)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;            
        }                
    }

    private void turnOffButton(GameObject g)
    {
        RectTransform rect = g.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(280, 70);
        g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        g.GetComponent<Image>().color = upperButtonNonOutlined;
    }

    private void turnOnButton(GameObject g)
    {
        RectTransform rect = g.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(300, 85);
        g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        g.GetComponent<Image>().color = upperButtonOutlined;
    }

    public void CloseCharacterPanel()
    {
        if (mainPanel.activeSelf)
        {
            if (!Globals.IsMobile)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            mainPanelActivation(false);
            Globals.IsOptions = false;
        }
    }

    private void mainPanelActivation(bool isActive)
    {
        if (isActive && !mainPanel.activeSelf)
        {
            mainPanel.transform.GetChild (0).gameObject.SetActive(true);

            mainPanel.transform.DOKill();
            mainPanel.SetActive(true);
            mainPanel.transform.localScale = Vector3.zero;
            mainPanel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce);

            _audio.Stop();
            _audio.clip = openSound;
            _audio.Play();
        }
        else if (!isActive && mainPanel.activeSelf)
        {
            inventoryUI.Clean();

            mainPanel.transform.DOKill();            
            mainPanel.transform.localScale = Vector3.one;
            mainPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutBounce).OnComplete(() => { mainPanel.SetActive(false); });

            mainPanel.transform.GetChild(0).gameObject.SetActive(false);

            _audio.Stop();
            _audio.clip = closeSound;
            _audio.Play();
        }
    }
}
