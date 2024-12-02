using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PCHelper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI W;
    [SerializeField] private TextMeshProUGUI A;
    [SerializeField] private TextMeshProUGUI S;
    [SerializeField] private TextMeshProUGUI D;
    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI block;
    [SerializeField] private TextMeshProUGUI jump;
    [SerializeField] private TextMeshProUGUI inventory;
    [SerializeField] private TextMeshProUGUI menu;

    // Start is called before the first frame update
    void Start()
    {
        W.text = Globals.Language.W;
        A.text = Globals.Language.A;
        S.text = Globals.Language.S;
        D.text = Globals.Language.D;

        attack.text = Globals.Language.Attack;
        block.text = Globals.Language.Block;
        jump.text = Globals.Language.Jump;
        inventory.text = Globals.Language.Inventory;
        menu.text = Globals.Language.Menu;
    }

    
}
