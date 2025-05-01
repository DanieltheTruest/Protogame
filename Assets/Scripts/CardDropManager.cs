using UnityEngine;
using UnityEngine.Tilemaps;

public class CardDropManager : MonoBehaviour
{
    public static CardDropManager Instance;
    
    public Tilemap tilemap;
    public LayerMask tilemapLayer;

    private void Awake()
    {
        Instance = this;
    }

public void TryPlaceCard(DraggableCard card)
{
    Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector3Int cellPos = tilemap.WorldToCell(worldPos);

    if (tilemap.HasTile(cellPos))
    {
        Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPos);
        card.transform.position = cellCenter;


        card.SetLastValidPosition(cellCenter);

        card.LockPlacement();
        card.ShowAvailableMoves(tilemap);
    }
    else
    {

        card.SnapBackToLastValidPosition();
    }
}
}