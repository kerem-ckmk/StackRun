using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmoUtilities
{
    public static void DrawArrow(float length, float width, Vector3 position, Vector3 forward, Vector3 up)
    {
        Vector3 right = Vector3.Cross(forward, up);

        Vector3 endPos = position - forward * length * 0.5f;
        Vector3 tipPos = position + forward * length * 0.5f;
        Vector3 tipRightPos = tipPos - forward * width * 0.5f + right * width * 0.5f;
        Vector3 tipLeftPos = tipPos - forward * width * 0.5f - right * width * 0.5f;

        Gizmos.DrawLine(tipPos, endPos);
        Gizmos.DrawLine(tipPos, tipRightPos);
        Gizmos.DrawLine(tipPos, tipLeftPos);
    }

    public static void DrawCircle(Vector3 center, Vector3 normal, float radius, int segmentCount = 16)
    {
        Vector3 upVector = Mathf.Abs(Vector3.Dot(normal, Vector3.up)) >= 0.95f ? Vector3.right : Vector3.up;

        Vector3 axis1 = Vector3.Cross(normal, upVector).normalized;
        Vector3 axis2 = Vector3.Cross(normal, axis1).normalized;

        float currentAngle = 0f;
        Vector3 prePoint = center + radius * (axis1 * Mathf.Sin(currentAngle) + axis2 * Mathf.Cos(currentAngle));

        for (int i = 1; i <= segmentCount; i++)
        {
            currentAngle = (i / (float)segmentCount) * Mathf.PI * 2;
            Vector3 curPoint = center + radius * (axis1 * Mathf.Sin(currentAngle) + axis2 * Mathf.Cos(currentAngle));

            Gizmos.DrawLine(prePoint, curPoint);

            prePoint = curPoint;
        }
    }

    public static void DrawGizmoNumber(int number, Vector3 center, float digitWidth, float digitHeight)
    {
        float halfDigitWidth = digitWidth * 0.5f;
        float halfDigitHeight = digitHeight * 0.5f;
        float digitSpace = digitWidth * 0.5f;

        Vector3 topLeftPoint = new Vector3(-halfDigitWidth, 0f, halfDigitHeight);
        Vector3 topRightPoint = new Vector3(halfDigitWidth, 0f, halfDigitHeight);
        Vector3 centerLeftPoint = new Vector3(-halfDigitWidth, 0f, 0f);
        Vector3 centerRightPoint = new Vector3(halfDigitWidth, 0f, 0f);
        Vector3 bottomLeftPoint = new Vector3(-halfDigitWidth, 0f, -halfDigitHeight);
        Vector3 bottomRightPoint = new Vector3(halfDigitWidth, 0f, -halfDigitHeight);

        string numberAsStr = number.ToString();

        for (int i = 0; i < numberAsStr.Length; i++)
        {
            char digit = numberAsStr[i];

            float xFactor = i - 0.5f * (numberAsStr.Length - 1);

            Vector3 pivotPoint = center;
            pivotPoint.x = center.x + xFactor * (digitWidth + digitSpace);

            if (digit == '-')
            {
                Gizmos.DrawLine(pivotPoint + centerLeftPoint, pivotPoint + centerRightPoint);
            }
            else if (digit == '0')
            {
                Gizmos.DrawLine(pivotPoint + topLeftPoint, pivotPoint + topRightPoint);
                Gizmos.DrawLine(pivotPoint + topRightPoint, pivotPoint + bottomRightPoint);
                Gizmos.DrawLine(pivotPoint + bottomRightPoint, pivotPoint + bottomLeftPoint);
                Gizmos.DrawLine(pivotPoint + bottomLeftPoint, pivotPoint + topLeftPoint);
            }
            else if (digit == '1')
            {
                Gizmos.DrawLine(pivotPoint + topRightPoint, pivotPoint + bottomRightPoint);
            }
            else if (digit == '2')
            {
                Gizmos.DrawLine(pivotPoint + topLeftPoint, pivotPoint + topRightPoint);
                Gizmos.DrawLine(pivotPoint + topRightPoint, pivotPoint + centerRightPoint);
                Gizmos.DrawLine(pivotPoint + centerRightPoint, pivotPoint + centerLeftPoint);
                Gizmos.DrawLine(pivotPoint + centerLeftPoint, pivotPoint + bottomLeftPoint);
                Gizmos.DrawLine(pivotPoint + bottomLeftPoint, pivotPoint + bottomRightPoint);
            }
            else if (digit == '3')
            {
                Gizmos.DrawLine(pivotPoint + topLeftPoint, pivotPoint + topRightPoint);
                Gizmos.DrawLine(pivotPoint + topRightPoint, pivotPoint + bottomRightPoint);
                Gizmos.DrawLine(pivotPoint + bottomRightPoint, pivotPoint + bottomLeftPoint);
                Gizmos.DrawLine(pivotPoint + centerRightPoint, pivotPoint + centerLeftPoint);
            }
            else if (digit == '4')
            {
                Gizmos.DrawLine(pivotPoint + topLeftPoint, pivotPoint + centerLeftPoint);
                Gizmos.DrawLine(pivotPoint + centerLeftPoint, pivotPoint + centerRightPoint);
                Gizmos.DrawLine(pivotPoint + topRightPoint, pivotPoint + bottomRightPoint);
            }
            else if (digit == '5')
            {
                Gizmos.DrawLine(pivotPoint + topRightPoint, pivotPoint + topLeftPoint);
                Gizmos.DrawLine(pivotPoint + topLeftPoint, pivotPoint + centerLeftPoint);
                Gizmos.DrawLine(pivotPoint + centerLeftPoint, pivotPoint + centerRightPoint);
                Gizmos.DrawLine(pivotPoint + centerRightPoint, pivotPoint + bottomRightPoint);
                Gizmos.DrawLine(pivotPoint + bottomRightPoint, pivotPoint + bottomLeftPoint);
            }
            else if (digit == '6')
            {
                Gizmos.DrawLine(pivotPoint + topRightPoint, pivotPoint + topLeftPoint);
                Gizmos.DrawLine(pivotPoint + topLeftPoint, pivotPoint + bottomLeftPoint);
                Gizmos.DrawLine(pivotPoint + bottomLeftPoint, pivotPoint + bottomRightPoint);
                Gizmos.DrawLine(pivotPoint + bottomRightPoint, pivotPoint + centerRightPoint);
                Gizmos.DrawLine(pivotPoint + centerRightPoint, pivotPoint + centerLeftPoint);
            }
            else if (digit == '7')
            {
                Gizmos.DrawLine(pivotPoint + topLeftPoint, pivotPoint + topRightPoint);
                Gizmos.DrawLine(pivotPoint + topRightPoint, pivotPoint + bottomRightPoint);
            }
            else if (digit == '8')
            {
                Gizmos.DrawLine(pivotPoint + topLeftPoint, pivotPoint + topRightPoint);
                Gizmos.DrawLine(pivotPoint + topRightPoint, pivotPoint + bottomRightPoint);
                Gizmos.DrawLine(pivotPoint + bottomRightPoint, pivotPoint + bottomLeftPoint);
                Gizmos.DrawLine(pivotPoint + bottomLeftPoint, pivotPoint + topLeftPoint);
                Gizmos.DrawLine(pivotPoint + centerLeftPoint, pivotPoint + centerRightPoint);
            }
            else if (digit == '9')
            {
                Gizmos.DrawLine(pivotPoint + centerRightPoint, pivotPoint + centerLeftPoint);
                Gizmos.DrawLine(pivotPoint + centerLeftPoint, pivotPoint + topLeftPoint);
                Gizmos.DrawLine(pivotPoint + topLeftPoint, pivotPoint + topRightPoint);
                Gizmos.DrawLine(pivotPoint + topRightPoint, pivotPoint + bottomRightPoint);
                Gizmos.DrawLine(pivotPoint + bottomRightPoint, pivotPoint + bottomLeftPoint);
            }
        }
    }
}
