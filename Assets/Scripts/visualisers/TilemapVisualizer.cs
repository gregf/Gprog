using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTilemap;

    [SerializeField] private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull, 
        wallInnerCornerDownLeft, wallInnerCornerDownRight, 
        wallDiagonalCornerDownRight,  wallDiagonalCornerDownLeft,  wallDiagonalCornerUpRight,  wallDiagonalCornerUpLeft;


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

    internal void paintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypeBinaryList.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallTypeBinaryList.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypeBinaryList.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallTypeBinaryList.wallBottom.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypeBinaryList.WallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        if(tile != null)
        {
            paintSingleTiles(wallTilemap, tile, position);
        }
    }

    internal void paintSingleCornerWall(Vector2Int position, string neighboursBinaryType)
    {
        
    }   

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}
