using System.Collections.Generic;
using UnityEngine;

public class Junction : MonoBehaviour
{

    public bool Right;
    public bool Left;
    public bool Forward;
    public bool Back;

    private int junctionAngle;

    public Direction currentTrafficLightGreenDireciton;
    public float trafficLightDuration;
    private float trafficLightTimer;

    public List<bool> paths = new List<bool>
    {
        false,
        false,
        false,
        false
    };

    public List<TrafficLight> trafficLights = new List<TrafficLight>();

    public void Start()
    {
        CallibrateJunctionOrientation();
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

        if(trafficLights.Count != 0)
        {
            if (trafficLightTimer > trafficLightDuration)
            {
                int rndDirection = 0;
                while (true)
                {
                    rndDirection = Random.Range(0, trafficLights.Count);
                    if(trafficLights[rndDirection] != null)
                    {
                        break;
                    }
                }
                ChangeTrafficLightDirection((Direction) rndDirection);
            }
            else
            {
                trafficLightTimer += Time.deltaTime;
            }
        }
    }

    public void ChangeTrafficLightDirection(Direction direction)
    {
        if(trafficLights[(int)direction] != null)
        {

            if (trafficLights[(int)currentTrafficLightGreenDireciton] != null)
            {
                trafficLights[(int)currentTrafficLightGreenDireciton].ChangeLight(false);
            }
            trafficLights[(int)direction].ChangeLight(true);
            currentTrafficLightGreenDireciton = direction;
        } 
        else
        {
            Debug.Log("Light missing");
        }
        trafficLightTimer = 0;
    }

    public void CallibrateJunctionOrientation()
    {
        junctionAngle = (int)(transform.eulerAngles.y + .1f);
        junctionAngle %= 360;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, junctionAngle, transform.eulerAngles.z);

        int offsetIndex = junctionAngle / 90;

        paths[(int)Direction.Right] = Right;
        paths[(int)Direction.Left] = Left;
        paths[(int)Direction.Forward] = Forward;
        paths[(int)Direction.Back] = Back;

        paths = RotateList<bool>(paths, offsetIndex, -1);
        if(trafficLights.Count != 0)
        {
            trafficLights = RotateList<TrafficLight>(trafficLights, offsetIndex, -1);
        }
    }

    public List<bool> CheckJunctionPaths(Vector3 relativeForward)
    {
        return (paths);
    }

    public static List<T> RotateList<T>(List<T> list, int shift, int direction = 1)
    {
        int j = 0;
        List<T> temp_buffer = new List<T>();
        List<T> rotated_list = new List<T>();

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
                rotated_list.Add(list[i + shift]);
            }
            else
            {
                rotated_list.Add(temp_buffer[j]);
                j++;
            }
        }
        return rotated_list;
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
