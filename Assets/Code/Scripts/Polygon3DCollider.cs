using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Polygon3DCollider : MonoBehaviour
{
    private void Awake()
    {
        PolygonCollider2D collider = gameObject.GetComponent<PolygonCollider2D>();
        Mesh plane = collider.CreateMesh(false, false, true);

        Destroy(collider);

        if (plane != null)
        {
            MeshCollider newCollider = gameObject.AddComponent<MeshCollider>();
            newCollider.sharedMesh = plane;

            newCollider.convex = true;
        }
        else
        {
            Debug.Log("something is terribly wrong");
        }
    }
}
