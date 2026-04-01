using UnityEngine;
using Unity.AI.Navigation;


[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshBaker : MonoBehaviour
{
    public static NavMeshBaker Instance { get; private set; }

    public bool IsReady { get; private set; } = false;

    private NavMeshSurface surface;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        surface = GetComponent<NavMeshSurface>();
    }

    public void RebakeNavMesh()
    {
        if (surface == null) return;

        IsReady = false;
        Debug.Log("[NavMeshBaker] Trwa bakowanie NavMesh...");

        surface.RemoveData();
        surface.BuildNavMesh();

        IsReady = true;
        Debug.Log("[NavMeshBaker] NavMesh gotowy!");
    }
}
