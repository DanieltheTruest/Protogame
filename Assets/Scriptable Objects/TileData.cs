using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu]
public class TileData :ScriptableObject
{
    public TileBase[] tiles;

    public float filled, blocked;
}
