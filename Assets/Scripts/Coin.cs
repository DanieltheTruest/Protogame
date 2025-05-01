using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public Tilemap tilemap;                  // Assign in Inspector
    public GameObject coinPrefab;            // Assign coin prefab
    public int numberOfCoins = 10;

    private HashSet<Vector3Int> usedPositions = new();

    void Start()
    {
        PlaceRandomCoins();
    }

    void PlaceRandomCoins()
    {
        BoundsInt bounds = tilemap.cellBounds;
        int attempts = 0;

        while (usedPositions.Count < numberOfCoins && attempts < numberOfCoins * 10)
        {
            attempts++;
            int x = Random.Range(bounds.xMin, bounds.xMax);
            int y = Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int cell = new Vector3Int(x, y, 0);

            if (tilemap.HasTile(cell) && !usedPositions.Contains(cell))
            {
                Vector3 worldPos = tilemap.GetCellCenterWorld(cell);
                Instantiate(coinPrefab, worldPos, Quaternion.identity);
                usedPositions.Add(cell);
            }
        }
    }
}