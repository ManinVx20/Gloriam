using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public struct SmoothTransformData
    {
        public Vector3 StartLocalPosition;
        public Vector3 EndLocalPosition;
        public Quaternion StartLocalRotation;
        public Quaternion EndLocalRotation;
        public Vector3 StartLocalScale;
        public Vector3 EndLocalScale;
    }

    public static IEnumerator SmoothenTransformCoroutine(Transform transform, SmoothTransformData smoothTransformData, float duration,
        System.Action<float> onUpdate = null, System.Action onFinish = null)
    {
        float timer = 0.0f;
        onUpdate?.Invoke(timer);

        while (timer < 1.0f)
        {
            transform.localPosition = Vector3.Lerp(smoothTransformData.StartLocalPosition, smoothTransformData.EndLocalPosition, timer);
            transform.localRotation = Quaternion.Slerp(smoothTransformData.StartLocalRotation, smoothTransformData.EndLocalRotation, timer);
            transform.localScale = Vector3.Lerp(smoothTransformData.StartLocalScale, smoothTransformData.EndLocalScale, timer);

            timer += Time.deltaTime / duration;
            onUpdate?.Invoke(timer);

            yield return null;
        }

        timer = 1.0f;
        onUpdate?.Invoke(timer);

        transform.localPosition = smoothTransformData.EndLocalPosition;
        transform.localRotation = smoothTransformData.EndLocalRotation;
        transform.localScale = smoothTransformData.EndLocalScale;

        onFinish?.Invoke();
    }

    public static IEnumerator DelayActionCoroutine(float delayTime, System.Action action)
    {
        yield return new WaitForSeconds(delayTime);

        action?.Invoke();
    }

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

    public static Vector2 RandomPointOnCircle(float radius)
    {
        return Random.insideUnitCircle.normalized * radius;
    }
}
