using System.Collections.Generic;
using UnityEngine;

public class Junction : MonoBehaviour
{

    public bool Right;
    public bool Left;
    public bool Forward;
    public bool Back;

    private int junctionAngle;

    public List<bool> paths = new List<bool>
    {
        false,
        false,
        false,
        false
    };

    public void Start()
    {
        CallibrateJunctionOrientation();
        paths = CheckJunctionPaths();
    }

    public void Update()
    {
        foreach (int direction in new List<int>() { 0, 1, 2, 3 })
        {
            if (paths[direction])
            {
                switch ((Direction)direction)
                {
                    case Direction.Right:
                        Debug.DrawRay(transform.position + new Vector3(0, 0.2f, 0), Vector3.right * 2f, Color.yellow);
                        break;
                    case Direction.Left:
                        Debug.DrawRay(transform.position + new Vector3(0, 0.2f, 0), -Vector3.right * 2f, Color.yellow);
                        break;
                    case Direction.Forward:
                        Debug.DrawRay(transform.position + new Vector3(0, 0.2f, 0), Vector3.forward * 2f, Color.yellow);
                        break;
                    case Direction.Back:
                        Debug.DrawRay(transform.position + new Vector3(0, 0.2f, 0), -Vector3.forward * 2f, Color.yellow);
                        break;
                }
            }
        }
    }

    public void CallibrateJunctionOrientation()
    {
        junctionAngle = (int)(transform.eulerAngles.y + .1f);
        junctionAngle %= 360;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, junctionAngle, transform.eulerAngles.z);
    }

    public List<bool> CheckJunctionPaths()
    {
        int offsetIndex = junctionAngle/90;

        paths[(int)Direction.Right] = Right;
        paths[(int)Direction.Left] = Left;
        paths[(int)Direction.Forward] = Forward;
        paths[(int)Direction.Back] = Back;

        return (RotateList<bool>(paths, offsetIndex, -1));
    }

    public static List<T> RotateList<T>(List<T> list, int shift, int direction = 1)
    {
        int j = 0;
        List<T> temp_buffer = new List<T>();

        if (direction == -1)
        {
            shift = list.Count - shift;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (i < shift)
            {
                temp_buffer.Add(list[i]);
            }

            if (i < list.Count - shift)
            {
                list[i] = list[i + shift];
            }
            else
            {
                list[i] = temp_buffer[j];
                j++;
            }
        }
        return list;
    }

    public void ResetPaths()
    {
        paths = new List<bool>
         {
             false,
             false,
             false,
             false
         };
    }
}
