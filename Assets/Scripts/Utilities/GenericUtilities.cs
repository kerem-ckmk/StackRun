using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GenericUtilities
{
    public static string KiloFormatNumber(this int number)
    {
        if (number >= 100000000)
        {
            return (number / 1000000D).ToString("0.#M", System.Globalization.CultureInfo.InvariantCulture);
        }
        else if (number >= 1000000)
        {
            return (number / 1000000D).ToString("0.##M", System.Globalization.CultureInfo.InvariantCulture);
        }
        else if (number >= 100000)
        {
            return (number / 1000D).ToString("0.#k", System.Globalization.CultureInfo.InvariantCulture);
        }
        else if (number >= 10000)
        {
            return (number / 1000D).ToString("0.##k", System.Globalization.CultureInfo.InvariantCulture);
        }
        else if (number >= 1000)
        {
            return (number / 1000D).ToString("0.##k", System.Globalization.CultureInfo.InvariantCulture);
        }

        return number.ToString("#,0", System.Globalization.CultureInfo.InvariantCulture);
    }

    public static string RemoveWhitespace(this string input)
    {
        return new string(input.ToCharArray()
            .Where(c => !Char.IsWhiteSpace(c))
            .ToArray());
    }

    public static Vector2 FlatVectorConvert(Vector3 flatVector)
    {
        return new Vector2(flatVector.x, flatVector.z);
    }

    public static Vector3 FlatHeight(this Vector3 vector)
    {
        return new Vector3(vector.x, 0f, vector.z);
    }

    public static void SetPositionX(this Transform transform, float x)
    {
        var position = transform.position;
        position.x = x;

        transform.position = position;
    }

    public static void SetPositionY(this Transform transform, float y)
    {
        var position = transform.position;
        position.y = y;

        transform.position = position;
    }

    public static void SetPositionZ(this Transform transform, float z)
    {
        var position = transform.position;
        position.z = z;

        transform.position = position;
    }

    public static void SetLocalPositionX(this Transform transform, float x)
    {
        var localPosition = transform.localPosition;
        localPosition.x = x;

        transform.localPosition = localPosition;
    }

    public static void SetLocalPositionY(this Transform transform, float y)
    {
        var localPosition = transform.localPosition;
        localPosition.y = y;

        transform.localPosition = localPosition;
    }

    public static void SetLocalPositionZ(this Transform transform, float z)
    {
        var localPosition = transform.localPosition;
        localPosition.z = z;

        transform.localPosition = localPosition;
    }

    public static void SetLocalScaleX(this Transform transform, float x)
    {
        var localScale = transform.localScale;
        localScale.x = x;

        transform.localScale = localScale;
    }

    public static void SetLocalScaleY(this Transform transform, float y)
    {
        var localScale = transform.localScale;
        localScale.y = y;

        transform.localScale = localScale;
    }

    public static void SetLocalScaleZ(this Transform transform, float z)
    {
        var localScale = transform.localScale;
        localScale.z = z;

        transform.localScale = localScale;
    }

    public static Bounds CalculateCumulativeRendererBounds(GameObject gameObject)
    {
        var renderers = gameObject.GetComponentsInChildren<Renderer>();

        if (renderers != null && renderers.Length > 0)
        {
            var bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        return new Bounds(Vector3.zero, Vector3.zero);
    }

    public static void DestroyAllChildren(this Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }

    public static bool Contains2D(this Bounds bounds, Vector3 point)
    {
        if (point.x >= bounds.min.x && point.x <= bounds.max.x && point.z >= bounds.min.z && point.z <= bounds.max.z)
            return true;

        return false;
    }
}
