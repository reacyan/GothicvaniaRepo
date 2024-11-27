using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private float xlimit=960;
    private float ylimit=540;
    [SerializeField] private float xOffest;
    [SerializeField] private float yOffest;

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXoffest = 0;
        float newYoffest = 0;

        if (mousePosition.x > xlimit)
        {
            newXoffest = -xOffest;
        }
        else
        {
            newXoffest = xOffest;
        }

        if (mousePosition.y > ylimit)
        {
            newYoffest = -yOffest;
        }
        else
        {
            newYoffest = yOffest;
        }

        transform.position = new Vector2(mousePosition.x + newXoffest, mousePosition.y + newYoffest);
    }
}
