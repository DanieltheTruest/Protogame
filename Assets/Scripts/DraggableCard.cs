using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public enum Direction
{
    N, NE, E, SE, S, SW, W, NW
}
public class DraggableCard : MonoBehaviour
{   
    private static DraggableCard currentlySelected;
    private bool isSelected = false;
private List<GameObject> spawnedMarkers = new();

[SerializeField] private GameObject highlightMarkerPrefab;
[SerializeField] private Tilemap highlightTilemap; 
public TileBase highlightTile; 
    private Vector3 offset;
    private bool dragging = false;
    public List<Direction> allowedDirections = new List<Direction>();
    public bool IsPlaced { get; private set; } = false;

    void OnMouseDown()
    {
        if (IsPlaced) return;

        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    void OnMouseDrag()
    {
        if (!dragging || IsPlaced) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -1;
        transform.position = mousePos + offset;
    }
    
    void OnMouseUp()
    {
        if (!dragging || IsPlaced) return;

        dragging = false;
        CardDropManager.Instance.TryPlaceCard(this);
    }

    public void LockPlacement()
    {
        IsPlaced = true;
        dragging = false;
    }
private List<GameObject> activeHighlights = new();

public void ShowAvailableMoves(Tilemap tilemap)
{
    reachableTiles.Clear();
    ClearHighlights();

    Vector3Int currentCell = tilemap.WorldToCell(transform.position);

    foreach (var dir in allowedDirections)
    {
        Vector3Int offset = GetOffsetForDirection(dir);
        Vector3Int targetCell = currentCell + offset;

        if (tilemap.HasTile(targetCell))
        {
            reachableTiles.Add(targetCell);

            // Convert tile position to world position
            Vector3 worldPos = tilemap.GetCellCenterWorld(targetCell);
            GameObject marker = Instantiate(highlightMarkerPrefab, worldPos, Quaternion.identity);
            spawnedMarkers.Add(marker);
        }
    }
}

private void ClearHighlights()
{
    foreach (var marker in spawnedMarkers)
    {
        Destroy(marker);
    }
    spawnedMarkers.Clear();
}


    private Vector3Int GetOffsetForDirection(Direction dir)
    {
    return dir switch
    {
        Direction.N  => new Vector3Int(0, 1, 0),
        Direction.NE => new Vector3Int(1, 1, 0),
        Direction.E  => new Vector3Int(1, 0, 0),
        Direction.SE => new Vector3Int(1, -1, 0),
        Direction.S  => new Vector3Int(0, -1, 0),
        Direction.SW => new Vector3Int(-1, -1, 0),
        Direction.W  => new Vector3Int(-1, 0, 0),
        Direction.NW => new Vector3Int(-1, 1, 0),
        _ => Vector3Int.zero
    };
    }
    private List<Vector3Int> reachableTiles = new();
[SerializeField] private Tilemap tilemap; 
private bool IsClickedOnSelf(Vector3 mouseWorldPos)
{
    Vector3 cardPos = transform.position;
    Collider2D col = GetComponent<Collider2D>();

    return col != null && col.OverlapPoint(mouseWorldPos);
}
 void Update()
{
    if (!IsPlaced || tilemap == null) return;

    if (Input.GetMouseButtonDown(0))
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (IsClickedOnSelf(mouseWorldPos))
        {
            if (!isSelected)
            {
                isSelected = true;
                ShowAvailableMoves(tilemap);
            }
            else
            {
                isSelected = false;
                ClearHighlights();
            }
        }
        else if (isSelected)
        {
            Vector3Int clickedCell = tilemap.WorldToCell(mouseWorldPos);
            if (reachableTiles.Contains(clickedCell))
            {
                MoveTo(clickedCell);
                isSelected = false;
                ClearHighlights();
            }
        }
    }
}
private void MoveTo(Vector3Int cell)
{
    transform.position = tilemap.GetCellCenterWorld(cell);
    ClearHighlights();
    reachableTiles.Clear();
    isSelected = false;
}
private void Deselect()
{
    isSelected = false;
    ClearHighlights();
}

}