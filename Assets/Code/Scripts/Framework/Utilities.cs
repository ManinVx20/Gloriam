using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector3 GetRandomNormalizedDirection(float min, float max)
    {
        float randomX = Random.Range(min, max);
        float randomZ = Random.Range(min, max);

        return new Vector3(randomX, 0.0f, randomZ).normalized;
    }

    public static Vector2 GetRandomNormalizedVector2(float min, float max)
    {
        float randomX = Random.Range(min, max);
        float randomY = Random.Range(min, max);

        return new Vector2(randomX, randomY).normalized;
    }

    public static Camera GetCameraWithName(string name)
    {
        for (int i = 0; i < Camera.allCamerasCount; i++)
        {
            if (Camera.allCameras[i].gameObject.name == name)
            {
                return Camera.allCameras[i];
            }
        }

        return null;
    }
}
