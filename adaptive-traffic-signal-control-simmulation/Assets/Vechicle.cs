using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Right,
    Left,
    Forward,
    Back
}

public class Vechicle : MonoBehaviour
{
    public float Speed;
    public float RotationRadius;

    private float movementSnap = 1;

    private List<bool> junctionPaths = new List<bool>
    {
        false,  //Direciton.Right
        false,  //Direction.Left
        false,  //Direction.Forward
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
        if(isMoving)
        {
            if(movementSnapped)
            {
                if(atJunction && !directionChanged)
                {
                    atJunctionSnapCounter++;
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
        currentMovingDirection = Direction.Forward;
        isMoving = true;
        atJunction = false;
        destinationVector = transform.position + transform.forward * movementSnap;
        directionChanged = false;
        atJunctionSnapCounter = -1;
        Debug.Log(destinationVector);
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
        if(!nextDirectionLocked)
        {
            nextDirection = Direction.Forward;
            while (true)
            {
                nextDirection = (Direction)Random.Range(0, sizeof(Direction) - 1);
                if(junctionPaths[(int)nextDirection])
                {
                    nextDirectionLocked = true;
                    break;
                }
            }
        }

        var angle = -1;

        switch (nextDirection)
        {
            case Direction.Right:
                if(atJunctionSnapCounter == 1)
                {
                    angle = 90;
                }
                break;
            case Direction.Left:
                if (atJunctionSnapCounter == 0)
                {
                    angle = -90;
                }
                break;
            case Direction.Forward:
                if (atJunctionSnapCounter == 2)
                {
                    angle = 0;
                }
                break;
        }
        if(angle != -1)
        {
            Debug.Log("Rotating");
            transform.RotateAround(transform.position, transform.up, angle);
            destinationVector = transform.position + transform.forward * movementSnap;

            currentMovingDirection = Direction.Forward;
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
            junctionPaths[(int)Direction.Right] = true;
            junctionPaths[(int)Direction.Left] = true;
            junctionPaths[(int)Direction.Forward] = true;
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