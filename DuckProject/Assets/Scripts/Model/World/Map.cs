using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell {
    public HashSet<Pickup> Pickups { get; } = new();
}

public class Map : MonoBehaviour {

    #region Properties

    public Vector2 Size => (Vector3)grid.transform.GetChild(1).GetComponent<CompositeCollider2D>().bounds.size;
    public float Margin => 1.75f;

    #endregion

    #region Fields

    // Collections.
    private Dictionary<Vector2Int, Cell> cells = new();

    // Components.
    private Grid grid;

    #endregion

    #region Initialize

    public void Initialize() {
        grid = this.gameObject.GetOrAddComponent<Grid>();

        Main.Game.CurrentMap = this;
    }

    #endregion

    #region Cell

    public void Add(Pickup pickup) {
        Vector2Int cellPosition = (Vector2Int)grid.WorldToCell(pickup.transform.position);

        Cell cell = GetCell(cellPosition);
        if (cell == null) return;

        cell.Pickups.Add(pickup);
    }

    public void Remove(Pickup pickup) {
        Vector2Int cellPosition = (Vector2Int)grid.WorldToCell(pickup.transform.position);

        Cell cell = GetCell(cellPosition);
        if (cell == null) return;

        cell.Pickups.Remove(pickup);
    }
    private Cell GetCell(Vector2Int cellPosition) {
        if (cells.TryGetValue(cellPosition, out Cell cell)) return cell;
        cell = new();
        cells.Add(cellPosition, cell);
        return cell;
    }

    #endregion

    public bool IsInMap(Vector2 position) {
        if (position.x < 0 + Margin || Size.x - Margin < position.x) return false;
        if (position.y < 0 + Margin || Size.y - Margin < position.y) return false;
        return true;
    }
    public Vector2 GetRandomAroundPosition(Vector2 origin, float radius) {
        Vector2 position = Vector2.zero;
        int count = 0;
        while (count < 1000) {
            count++;
            float angle = Random.Range(0, Mathf.PI * 2);
            Vector2 direction = new(Mathf.Cos(angle), Mathf.Sin(angle));
            position = origin + direction * radius;
            if (IsInMap(position)) return position;
        }
        return position;
    }
    public Vector2 GetRandomEdgePosition(float margin) {
        if (Random.Range(0, 2) == 0) {
            float x = Random.Range(0, 2) == 0 ? margin : Size.x - margin;
            float y = Random.Range(margin, Size.y - margin);
            return new(x, y);
        }
        else {
            float x = Random.Range(margin, Size.x - margin);
            float y = Random.Range(0, 2) == 0 ? margin : Size.y - margin;
            return new(x, y);
        }
    }

    public List<Pickup> GetPickups(Vector2 position, float range) {
        List<Pickup> pickups = new();

        int minX = grid.WorldToCell(position + new Vector2(-range, 0)).x;
        int maxX = grid.WorldToCell(position + new Vector2(+range, 0)).x;
        int minY = grid.WorldToCell(position + new Vector2(0, -range)).y;
        int maxY = grid.WorldToCell(position + new Vector2(0, +range)).y;

        for (int x = minX; x <= maxX; x++) {
            for (int y = minY; y <= maxY; y++) {
                if (!cells.ContainsKey(new(x, y))) continue;
                pickups.AddRange(cells[new(x, y)].Pickups);
            }
        }

        return pickups;
    }


}