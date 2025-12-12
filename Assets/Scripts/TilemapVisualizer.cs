using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTilemap;

    [SerializeField] private TileBase floorTile, wallTop;


    public void paintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        paintTiles(floorPositions, floorTilemap, floorTile);
    }

    private void paintTiles(IEnumerable<Vector2Int> Positions, Tilemap Tilemap, TileBase Tiles)
    {
        foreach (var position in Positions)
        {
            paintSingleTiles(Tilemap, Tiles, position);
        }
    }

    private void paintSingleTiles(Tilemap tilemap, TileBase tiles, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tiles);
    }

    internal void paintSingleBasicWall(Vector2Int position)
    {
        paintSingleTiles(wallTilemap, wallTop, position);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}
