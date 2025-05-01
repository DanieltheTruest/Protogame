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
        card.transform.position = tilemap.GetCellCenterWorld(cellPos);
        card.LockPlacement();
        card.ShowAvailableMoves(tilemap); // ← new line
    }
    else
    {
        Destroy(card.gameObject);
    }
}
}