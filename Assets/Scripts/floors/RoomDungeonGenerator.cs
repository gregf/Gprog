using System.Collections.Generic;
using UnityEngine;

public class RoomDungeonGenerator : RandomWalkDungeonGenerator
{
    [SerializeField]private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField][Range(0,10)]private int offset = 1;
    [SerializeField]private bool randomWalkRooms = false;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning
            (new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)),minRoomWidth, minRoomHeight);


        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        floor = CreateRooms(roomsList);

        List<Vector2Int> roomCenter = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenter.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenter);
        floor.UnionWith(corridors);

        tilemapVisualizer.paintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenter)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenter[Random.Range(0, roomCenter.Count)];
        return corridors;
    }

    private HashSet<Vector2Int> CreateRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int collum = offset; collum < room.size.x - offset; collum++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(collum, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
