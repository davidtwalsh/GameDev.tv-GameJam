using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Placeable 
{
    void Place(Vector3 worldPosition);

    bool IsPlaceable(Vector3 worldPosition);

    void MoveWithMouse(Vector3 worldPosition);
}
