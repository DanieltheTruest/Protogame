using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHoverIndicator : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject indicatorPrefab;

    private GameObject currentIndicator;
    private Vector3Int previousCell;

    void Update()
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = tilemap.WorldToCell(worldMousePos);

        if (tilemap.HasTile(cellPos))
        {
            if (currentIndicator == null || cellPos != previousCell)
            {
                if (currentIndicator != null)
                    Destroy(currentIndicator);

                Vector3 spawnPos = tilemap.GetCellCenterWorld(cellPos);
                currentIndicator = Instantiate(indicatorPrefab, spawnPos, Quaternion.identity);
                previousCell = cellPos;
            }
        }
        else
        {
            if (currentIndicator != null)
            {
                Destroy(currentIndicator);
                currentIndicator = null;
            }
        }
    }
}