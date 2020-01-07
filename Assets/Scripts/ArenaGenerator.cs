using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaGenerator : MonoBehaviour
{
    [Range(0, 100)]
    public int minObstacls;
    [Range(0, 100)]
    public int maxObstacls;

    public float height;
    public float width;

    public GameObject[] obstacls;

    // Start is called before the first frame update
    void Start()
    {
        if(minObstacls > maxObstacls)
        {
            maxObstacls = minObstacls;
        }

        int numOfObstacals = (int) Random.Range(minObstacls, maxObstacls);

        for (int i = 0; i < numOfObstacals; i++)
        {
            PlaceObstacle();
        }
    }

    void PlaceObstacle()
    {
        int index = (int) Random.Range(0, obstacls.Length);

        float posY = Random.Range(-height + 2, height - 2);
        float posX = Random.Range(-width + 2, width - 2);

        Quaternion rotation = Random.rotation;
        rotation.Set(0, 0, rotation.z, rotation.w);

        GameObject current = Instantiate(obstacls[index], new Vector3(posX, posY, 0), rotation);

        if (!ValidLocation(current))
        {
            Destroy(current);
            PlaceObstacle();
        }
    }

    bool ValidLocation(GameObject current)
    {
        return current.GetComponent<BoxCollider2D>().IsTouchingLayers();
    }
}
