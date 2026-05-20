using UnityEngine;

public class GridGizmo : MonoBehaviour
{
    [SerializeField] private Grid _Grid;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        for (int x = 0; x <= _Grid.cellSize.x + 1; x++)
        {
            Vector3 start = transform.position + new Vector3(x * _Grid.cellSize.x, 0, 0);
            Vector3 end = transform.position + new Vector3(x * _Grid.cellSize.x, 0, _Grid.cellSize.z * 2);

            Gizmos.DrawLine(start, end);
        }

        for (int z = 0; z <= _Grid.cellSize.z + 1; z++)
        {
            Vector3 start = transform.position + new Vector3(0, 0, z * _Grid.cellSize.z);
            Vector3 end = transform.position + new Vector3(_Grid.cellSize.x * 2, 0, z * _Grid.cellSize.z);

            Gizmos.DrawLine(start, end);
        }
    }
}
