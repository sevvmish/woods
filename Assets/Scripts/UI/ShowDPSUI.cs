using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;

public class ShowDPSUI : MonoBehaviour
{
    [Inject] private Camera _camera;

    [SerializeField] private TextMeshProUGUI dpsSign;
    [SerializeField] private Transform location;

    private ObjectPool dpsSignPool;
    private Dictionary<GameObject, DPSData> signs = new Dictionary<GameObject, DPSData>();
    private List<GameObject> toKill = new List<GameObject>();

    private void Start()
    {
        dpsSignPool = new ObjectPool(10, dpsSign.gameObject, location);
    }

    private void Update()
    {
        if (signs.Count > 0)
        {
            foreach (GameObject key in signs.Keys)
            {
                signs[key].TTL -= Time.deltaTime;
                if (signs[key].TTL < 0 && !toKill.Contains(key))
                {
                    toKill.Add(key);
                }

                signs[key].deltaMovement *= 0.975f;
                signs[key].shiftPosition += signs[key].deltaMovement;
                signs[key].Rect.anchoredPosition = _camera.WorldToScreenPoint(signs[key].place.position + signs[key].UPVector);
                signs[key].Rect.anchoredPosition += signs[key].shiftPosition;
            }
        }

        if (toKill.Count > 0)
        {
            for (int i = 0; i < toKill.Count; i++)
            {                
                dpsSignPool.ReturnObject(toKill[i]);
                signs.Remove(toKill[i]);
            }

            toKill.Clear();
        }
        
    }

    public void ShowDPS(int amount, Transform point, Vector3 UPVector)
    {
        GameObject s = dpsSignPool.GetObject();
        s.GetComponent<TextMeshProUGUI>().text = amount.ToString();
        s.GetComponent<TextMeshProUGUI>().fontSize = 50;
        s.SetActive(true);

        int rnd = UnityEngine.Random.Range(0, 2);
        int sign = rnd == 0 ? 1 : -1;

        signs.Add(s, new DPSData(point, new Vector2(sign * 4, 8), 1.5f, UPVector, s.GetComponent<RectTransform>()));
    }

    public class DPSData
    {
        public Transform place;
        public Vector2 deltaMovement;
        public Vector2 shiftPosition;
        public float TTL;
        public Vector3 UPVector;
        public RectTransform Rect;

        public DPSData(Transform place, Vector2 deltaMovement, float tTL, Vector3 UP, RectTransform rect)
        {
            this.place = place;
            this.deltaMovement = deltaMovement;
            shiftPosition = Vector2.zero;
            TTL = tTL;
            UPVector = UP;
            Rect = rect;
        }
    }
}
