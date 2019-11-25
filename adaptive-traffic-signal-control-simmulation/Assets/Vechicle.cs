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
        if (isMoving)
        {
            if(movementSnapped)
            {
                if(atJunction && !directionChanged)
                {
                    Debug.Log(atJunctionSnapCounter);
                    atJunctionSnapCounter++;
                    RotateRandom();
                }
                movementSnapped = false;
                Debug.Log("snapped");
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
                nextDirection = (Direction)UnityEngine.Random.Range(0, sizeof(Direction) - 1);
                if(junctionPaths[(int)nextDirection] & nextDirection != Direction.Back)
                {
                    nextDirectionLocked = true;
                    Debug.Log("direction: " + nextDirection);
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
            Debug.Log("at junction");
            directionChanged = false;
            atJunction = true;
            junctionPaths = other.gameObject.GetComponent<Junction>().CheckJunctionPaths();
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