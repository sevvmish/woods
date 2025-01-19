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

    private readonly float NPCHitFontSize = 70;
    private readonly float otherHitFontSize = 50;
    private readonly float playerHitFontSize = 70;

    public enum AimTypes
    {
        None,
        NPCs,
        Player
    }

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
                signs[key].Rect.anchoredPosition = _camera.WorldToScreenPoint(signs[key].place + signs[key].UPVector) - new Vector3(150, 0, 0);
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

    public void ShowDPS(float amount, Transform point, Vector3 UPVector, AimTypes aim)
    {        
        GameObject s = dpsSignPool.GetObject();
        TextMeshProUGUI texter = s.GetComponent<TextMeshProUGUI>();
        float koeffX = 1f;
        float koeffY = 1f;

        switch (aim)
        {
            case AimTypes.None:
                texter.fontSize = otherHitFontSize;
                texter.color = Color.yellow;
                break;

            case AimTypes.NPCs:
                texter.fontSize = NPCHitFontSize;
                texter.color = Color.yellow;
                koeffX = 0.5f;
                koeffY = 0.75f;
                break;

            case AimTypes.Player:
                texter.fontSize = playerHitFontSize;
                texter.color = Color.red;
                koeffX = 1.5f;
                koeffY = -0.2f;
                UPVector *= 0.5f;
                break;
        }

        texter.text = amount.ToString("f0");        
        texter.gameObject.SetActive(true);

        int rnd = UnityEngine.Random.Range(0, 2);
        int sign = rnd == 0 ? 1 : -1;

        signs.Add(s, new DPSData(point.position, new Vector2(sign * 4 * koeffX, 8 * koeffY), 1.5f, UPVector, s.GetComponent<RectTransform>()));
    }

    public class DPSData
    {
        public Vector3 place;
        public Vector2 deltaMovement;
        public Vector2 shiftPosition;
        public float TTL;
        public Vector3 UPVector;
        public RectTransform Rect;

        public DPSData(Vector3 place, Vector2 deltaMovement, float tTL, Vector3 UP, RectTransform rect)
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
