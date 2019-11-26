using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction   //cyclic order of directions
{
    Forward,
    Right,
    Back,
    Left
}

public class Vechicle : MonoBehaviour
{
    public float Speed;
    public float RotationRadius;

    private float movementSnap = 1;

    private List<bool> junctionPaths = new List<bool>
    {
        false,  //Direciton.Forward
        false,  //Direction.Right
        false,  //Direction.Back
        false   //Direction.Left
    };

    public static List<Vector3> DirectionVectorGlobal = new List<Vector3>
    {
        Vector3.forward,
        Vector3.right,
        Vector3.back,
        Vector3.left
    };

    private Vector3 destinationVector;

    private Direction currentMovingDirection;
    private Direction nextDirection;
    private int atJunctionSnapCounter;

    private bool isMoving;
    private bool atJunction;
    private bool movementSnapped;

    private bool directionChanged;
    private bool nextDirectionLocked;

    void Start()
    {
        Setup();
    }

    void Update()
    {

        foreach (int direction in new List<int>() { 0, 1, 2, 3 })
        {
            if (junctionPaths[direction])
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

        if (isMoving)
        {
            if (movementSnapped)
            {
                Debug.Log("snapped : " + atJunction + " : " + directionChanged);
                if (atJunction && !directionChanged)
                {
                    atJunctionSnapCounter++;
                    //Debug.Log(atJunctionSnapCounter);
                    RotateRandom();
                }
                movementSnapped = false;
            }
            else
            {
                Move(currentMovingDirection);
            }
        }
        else
        {

        }
    }

    private void Setup()
    {
        currentMovingDirection = Direction.Left;
        isMoving = true;
        atJunction = false;
        destinationVector = transform.position + transform.forward * movementSnap;
        directionChanged = false;
        atJunctionSnapCounter = -1;
    }

    private void Move(Direction direction)
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destinationVector) <= 0.1f)
        {
            movementSnapped = true;
            transform.position = destinationVector;
            destinationVector = transform.position + transform.forward * movementSnap;
        }
    }

    private void RotateRandom()
    {
        if (!nextDirectionLocked)
        {
            nextDirection = Direction.Forward;
            while (true)
            {
                nextDirection = (Direction)UnityEngine.Random.Range(0, sizeof(Direction));
                if (junctionPaths[(int)nextDirection] && ((int)currentMovingDirection - (int)nextDirection) % 2 != 0)
                {
                    nextDirectionLocked = true;
                    Debug.Log("direction: " + nextDirection);
                    break;
                }
            }
        }

        Debug.Log((int)nextDirection - (int)currentMovingDirection + " : " + atJunctionSnapCounter);

        if (((new List<int> { -1, 3 }.Contains((int)nextDirection - (int)currentMovingDirection))) && atJunctionSnapCounter == 0 
            || atJunctionSnapCounter == 1)
        {
            Debug.Log("Rotating");

            transform.LookAt(transform.position + DirectionVectorGlobal[(int)nextDirection]);
            destinationVector = transform.position + DirectionVectorGlobal[(int)nextDirection] * movementSnap;
            currentMovingDirection = nextDirection;

            directionChanged = true;
            atJunctionSnapCounter = -1;
            nextDirectionLocked = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Junction")
        {
            directionChanged = false;
            atJunction = true;
            junctionPaths = other.gameObject.GetComponent<Junction>().CheckJunctionPaths(transform.forward);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Junction")
        {
            atJunction = false;
        }
    }
}