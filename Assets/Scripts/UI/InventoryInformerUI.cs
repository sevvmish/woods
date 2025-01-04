using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using VContainer;

public class InventoryInformerUI : MonoBehaviour
{
    [Inject] private ItemManager itemManager;

    private TextMeshProUGUI mainTexter;
    private Dictionary<Item, invData> data = new Dictionary<Item, invData>();
    

    private class invData
    {        
        public int Amount;
        public float TTL;
        private const float TIME_TO_STAY = 3f;

        public invData(int amount)
        {
            Amount = amount;
            TTL = TIME_TO_STAY;
        }

        public void ResetTimer() => TTL = TIME_TO_STAY;
    }

    private void Awake()
    {
        mainTexter = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (data.Count > 0)
        {
            List<Item> toKill = new List<Item>();

            foreach (Item item in data.Keys)
            {
                if (data[item].TTL > 0)
                {
                    data[item].TTL -= Time.deltaTime;
                }
                else
                {
                    toKill.Add(item);
                }
            }

            if (toKill.Count > 0)
            {
                for (int i = 0; i < toKill.Count; i++)
                {
                    data.Remove(toKill[i]);
                }

                updateText();
            }
        }
    }

    public void AddItemInfo(int id, int amount)
    {
        if (id <= 0 || amount <=0) return;

        Item item = itemManager.GetItemByID(id);

        if (!data.ContainsKey(item))
        {
            data.Add(item, new invData(amount));
        }
        else
        {
            data[item].Amount += amount;
            data[item].ResetTimer();
        }

        updateText();
    }

    private void updateText()
    {
        if (data.Count == 0)
        {
            mainTexter.text = "";
            return;
        }

        string result = "";

        List<string> preResult = new List<string>();
        float maxTime = -1000;
        float minTime = 1000;

        foreach (Item item in data.Keys)
        {
            if (data[item].TTL >= maxTime)
            {
                maxTime = data[item].TTL;
                preResult.Insert(0, $"{Globals.Language.ItemsTranslation[item.ID].Name} +{data[item].Amount}\n");
            }
            else if (data[item].TTL <= minTime)
            {
                minTime = data[item].TTL;
                preResult.Add($"{Globals.Language.ItemsTranslation[item.ID].Name} +{data[item].Amount}\n");
            }
            else
            {
                if (preResult.Count > 1)
                {
                    preResult.Insert(1, $"{Globals.Language.ItemsTranslation[item.ID].Name} +{data[item].Amount}\n");
                }
                else
                {
                    preResult.Add($"{Globals.Language.ItemsTranslation[item.ID].Name} +{data[item].Amount}\n");
                }
            }
        }

        if (preResult.Count > 0)
        {            
            for (int i = 0; i < preResult.Count; i++)
            {
                result += preResult[i];
            }
        }

        mainTexter.text = result;
    }
}


