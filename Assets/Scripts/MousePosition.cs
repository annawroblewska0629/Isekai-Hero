using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MousePosition : MonoBehaviour
{

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }

    public static Vector3Int GetMouseGridPosition()
    {
        Vector3Int mouseGridPosition = LevelGrid.Instance.WorldPositionToGridPosition(GetMouseWorldPosition());
        return mouseGridPosition;
    }
}
