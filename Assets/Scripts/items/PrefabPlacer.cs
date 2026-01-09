using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PrefabPlacer : MonoBehaviour
{
    ItemPlacerData dungeonData;

    [SerializeField] private List<GameObject> itemsToPlace;
    [SerializeField] private int iterationsMax;

    public void PlaceItems()
    {
        foreach (var item in itemsToPlace)
        {
            Vector2Int size = new Vector2Int(1, 1);
            var itemComponent = item.GetComponent<Size>();

            Vector2? position = dungeonData.GetItemPlacementPosition(PlacementType.NearWall, iterationsMax, size, itemComponent != null);

            if (position != null)
            {
                Instantiate(item, new Vector3(position.Value.x, position.Value.y, 0), Quaternion.identity, this.transform);
            }
        }
    }



}
