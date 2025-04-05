using UnityEngine;

public class Pista : MonoBehaviour
{
    public Transform player;

    [Header("Escala Horizontal baseada na distância Y")]
    public float maxDistance = 30f;
    public float minScaleX = 0.5f;
    public float maxScaleX = 2f;

    [Header("Reciclagem da Pista")]
    public Transform referencePoint; // <- Novo: ponto base de reposição
    public float recycleThreshold = -15f;

    private Vector3 initialScale;
    private static Pista[] allRoads;
    private SpriteRenderer sr;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player não encontrado! Verifique se ele tem a tag 'Player'.");
            return;
        }

        if (referencePoint == null)
        {
            Debug.LogError("ReferencePoint não foi atribuído!");
            return;
        }

        initialScale = transform.localScale;
        sr = GetComponent<SpriteRenderer>();

        if (allRoads == null || allRoads.Length == 0)
            allRoads = transform.parent.GetComponentsInChildren<Pista>();
    }

    void Update()
    {
        if (player == null || referencePoint == null) return;

        float distanceY = Mathf.Abs(transform.position.y - player.position.y);
        float t = Mathf.Clamp01(distanceY / maxDistance);
        float scaleX = Mathf.Lerp(maxScaleX, minScaleX, t);

        transform.localScale = new Vector3(scaleX, initialScale.y, initialScale.z);

        if (transform.position.y < player.position.y + recycleThreshold)
            Recycle();
    }

    void Recycle()
    {
        Pista topRoad = null;
        float highestY = float.MinValue;

        foreach (var road in allRoads)
        {
            if (road != this && road.transform.position.y > highestY)
            {
                highestY = road.transform.position.y;
                topRoad = road;
            }
        }

        if (topRoad == null) return;

        float topHeight = topRoad.GetScaledHeight();
        float myHeight = GetScaledHeight();

        // Use referencePoint como base para calcular nova posição
        transform.position = new Vector3(
            referencePoint.position.x,
            topRoad.transform.position.y + topHeight / 2 + myHeight / 2,
            referencePoint.position.z
        );
    }

    float GetScaledHeight()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (sr == null) return 1f;
        return sr.bounds.size.y;
    }
}
