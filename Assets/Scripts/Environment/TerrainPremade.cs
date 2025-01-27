using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPremade
{
    private WorldGenerator wg;

    public TerrainPremade(WorldGenerator w)
    {
        wg = w;
    }

    public int GetTerrainID(Vector3 pos)
    {
        int result = 0;

        int index = wg.GetTerrainIndex(pos, 2);
        List<int> list = new List<int>() { 1, 2 };

        return list[index];
    }
}
