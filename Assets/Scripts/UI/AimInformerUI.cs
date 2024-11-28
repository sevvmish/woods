using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;

public class AimInformerUI : MonoBehaviour
{
    [Inject] private Camera _camera;
    [Inject] private PlayerControl pc;

    [Header("aimers")]
    [SerializeField] private TextMeshProUGUI aimSign;
    [SerializeField] private GameObject informerExample;
    [SerializeField] private Transform location;

    private ActionControl actions;
    private GameObject objectForActionsForMobile;

    private ObjectPool informerPool;
    private Dictionary<GameObject, RectTransform> assetsForMobileVisibility = new Dictionary<GameObject, RectTransform>();

    private float _timer;
    private float _cooldown = 0.2f;
    private bool isCheck;
    private List<GameObject> places = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        informerPool = new ObjectPool(10, informerExample, location);

        //aim text data
        aimSign.text = "";
        if (Globals.IsMobile)
        {
            aimSign.fontSize = 32;
            actions = pc.GetComponentInChildren<ActionControl>();
        }
        else
        {
            aimSign.fontSize = 25;
        }
    }
    

    private void Update()
    {
        float minDistance = 1000;
        if (assetsForMobileVisibility.Count > 0)
        {            
            if (_timer > _cooldown)
            {
                _timer = 0;
                isCheck = true;                
                places = new List<GameObject>();
            }
            else
            {
                _timer += Time.deltaTime;
            }

            foreach (GameObject key in assetsForMobileVisibility.Keys)
            {
                float distance = (pc.transform.position - key.transform.position).magnitude;
                if (distance > 12)
                {                    
                    places.Add(key);
                }

                assetsForMobileVisibility[key].anchoredPosition = _camera.WorldToScreenPoint(key.transform.position + Vector3.up * 1.5f);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    objectForActionsForMobile = key;
                }
            }

            if ( places.Count > 0)
            {
                for (int i = 0; i < places.Count; i++)
                {
                    if (assetsForMobileVisibility.ContainsKey(places[i].gameObject))
                    {
                        informerPool.ReturnObject(assetsForMobileVisibility[places[i]].gameObject);
                    }

                    if (assetsForMobileVisibility.ContainsKey(places[i].gameObject))
                    {
                        assetsForMobileVisibility.Remove(places[i].gameObject);
                    }                        
                }                
            }

            isCheck = false;
        }

        if (Globals.IsMobile)
        {
            if (objectForActionsForMobile == null)
            {
                actions.SetAim(null);
            }
            else
            {
                actions.SetAim(objectForActionsForMobile);
            }            
        }
    }

    public void ShowAimCursorText(GameObject g, bool isCollect)
    {
        bool isCollectable = false;
        bool isChopable = false;
        bool isMinable = false;
        
        aimSign.text = Asset.GetNameByAsset(g, out isCollectable, out isChopable, out isMinable);
        string actionText = Globals.Language.Collect;

        if (isChopable)
        {
            actionText = Globals.Language.Chop;
        }
        else if (isMinable)
        {
            actionText = Globals.Language.Mine;
        }

        if (isCollect)
        {
            if (Globals.IsMobile)
            {
                aimSign.text += $"\n{actionText}";
            }
            else
            {
                aimSign.text += $"\n[{Globals.Language.E}] {actionText}";
            }
        }
    }

    public void ShowAimCursorText(string s)
    {
        aimSign.text = s;
    }

    public void ShowMobile(Asset asset)
    {
        if (!assetsForMobileVisibility.ContainsKey(asset.gameObject))
        {
            GameObject g = informerPool.GetObject();
            g.SetActive(true);
            g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Asset.GetNameByAsset(asset.gameObject);
            g.GetComponent<RectTransform>().anchoredPosition = _camera.WorldToScreenPoint(asset.gameObject.transform.position + Vector3.up * 1.5f);
            assetsForMobileVisibility.Add(asset.gameObject, g.GetComponent<RectTransform>());
        }
    }

    public void HideMobile(Asset asset)
    {
        if (assetsForMobileVisibility.ContainsKey(asset.gameObject))
        {
            informerPool.ReturnObject(assetsForMobileVisibility[asset.gameObject].gameObject);
            assetsForMobileVisibility.Remove(asset.gameObject);
        }
    }

}
