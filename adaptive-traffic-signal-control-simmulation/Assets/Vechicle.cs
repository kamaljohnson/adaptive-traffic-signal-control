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

    public float minSpacingDistance;

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
        //Debug.DrawRay(transform.position + new Vector3(0, .1f, 0), transform.forward * 2f, Color.red);

        RaycastHit hit;
        bool vehicleInfront = false;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward, out hit, minSpacingDistance))
        {
            if(hit.collider.tag == "Car")
            {
                vehicleInfront = true;
            }
        }

        if(vehicleInfront)
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), transform.forward * hit.distance, Color.yellow);
            isMoving = false;
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), transform.forward * minSpacingDistance, Color.white);
            isMoving = true;
        }

        if (isMoving)
        {
            if (movementSnapped)
            {
                if (atJunction && !directionChanged)
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
                    break;
                }
            }
        }

        if (((new List<int> { -1, 3 }.Contains((int)nextDirection - (int)currentMovingDirection))) && atJunctionSnapCounter == 0 
            || atJunctionSnapCounter == 1)
        {
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