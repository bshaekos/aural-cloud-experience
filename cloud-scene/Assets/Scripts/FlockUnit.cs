using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockUnit : MonoBehaviour
{
    [SerializeField] private float FOVangle;
    [SerializeField] private float smoothDamp;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Vector3[] directionsToCheckToAvoidObstacles;
    
    private List<FlockUnit> cohesionNeighbors = new List<FlockUnit>();
    private List<FlockUnit> avoidanceNeighbors = new List<FlockUnit>();
    private List<FlockUnit> alignmentNeighbors = new List<FlockUnit>();
    private Flock assignedFlock;
    private Vector3 currentVelocity;
    private Vector3 currentObstacleAvoidanceVector;
    private float speed;

    public SpriteRenderer unitSpriteRenderer;

    GameObject player;

    Color unitExpiredColor = new Color(255, 255, 255, 0);

    public Transform myTransform { get; set;}

    private void Awake()
    {
        myTransform = transform;

        unitSpriteRenderer = GetComponent<SpriteRenderer>();
        unitSpriteRenderer.color = Color.white;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        //FadeUnitColor();
        
    }

    public void AssignFlock(Flock flock)
    {
        assignedFlock = flock;
    }

    public void InitializeSpeed(float speed)
    {
        this.speed = speed;
    }

    public void MoveUnit()
    {
        FindNeighbors();
        CalculateSpeed();

        var cohesionVector = CalculateCohesionVector() * assignedFlock.cohesionWeight;
        var avoidanceVector = CalculateAvoidanceVector() * assignedFlock.avoidanceWeight;
        var alignmentVector = CalculateAlignmentVector() * assignedFlock.alignmentWeight;
        var boundsVector = CalculateBoundsVector() * assignedFlock.boundsWeight;
        var obstacleVector = CalculateObstacleVector() *assignedFlock.obstacleWeight;

        var moveVector = cohesionVector + avoidanceVector + alignmentVector + boundsVector + obstacleVector;
        moveVector = Vector3.SmoothDamp(myTransform.forward, moveVector, ref currentVelocity, smoothDamp);
        moveVector = moveVector.normalized * speed;
        if (moveVector == Vector3.zero)
            moveVector = transform.forward;

        myTransform.forward = moveVector;
        myTransform.position += moveVector * Time.deltaTime;

    }

    public SpriteRenderer SetOpacity(float opacity)
    {
        Color color = new Color (255, 255, 255, opacity);

        unitSpriteRenderer.color = color;
        return unitSpriteRenderer;
    }

    // private SpriteRenderer FadeUnitColor()
    // {
    //     Color currentUnitColor = unitSpriteRenderer.color;

    //     if (currentUnitColor.a > 0)
    //     {
    //         unitSpriteRenderer.color = Color.Lerp(currentUnitColor, unitExpiredColor, 10f * Time.deltaTime);
    //         return unitSpriteRenderer;
    //     }
    //     else
    //     {
    //         return null;
    //     }

    // }

    private void FindNeighbors()
    {
        cohesionNeighbors.Clear();
        avoidanceNeighbors.Clear();
        alignmentNeighbors.Clear();
        var allUnits = assignedFlock.allUnits;
        for (int i = 0; i < allUnits.Length; i++)
        {
            var currentUnit = allUnits[i];
            if(currentUnit != this)
            {
                float currentNeighborDistanceSqr = Vector3.SqrMagnitude(currentUnit.myTransform.position - myTransform.position);
                if(currentNeighborDistanceSqr <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
                {
                    cohesionNeighbors.Add(currentUnit);
                }
                if(currentNeighborDistanceSqr <= assignedFlock.avoidanceDistance * assignedFlock.avoidanceDistance)
                {
                    avoidanceNeighbors.Add(currentUnit);
                }
                if(currentNeighborDistanceSqr <= assignedFlock.alignmentDistance * assignedFlock.alignmentDistance)
                {
                    alignmentNeighbors.Add(currentUnit);
                }
            }
        }
    }

    private void CalculateSpeed()
    {
        if(cohesionNeighbors.Count == 0)
            return;
        speed = 0;
        for(int i = 0; i < cohesionNeighbors.Count; i++)
        {
            speed += cohesionNeighbors[i].speed;
        }
        speed /= cohesionNeighbors.Count;
        speed = Mathf.Clamp(speed, assignedFlock.minSpeed, assignedFlock.maxSpeed);
    }

    private Vector3 CalculateCohesionVector()
    {
        var cohesionVector = Vector3.zero;
        if (cohesionNeighbors.Count == 0)
            return Vector3.zero;
        int neighborsInFOV = 0;
        for (int i = 0; i < cohesionNeighbors.Count; i++)
        {
            if(IsInFOV(cohesionNeighbors[i].myTransform.position))
            {
                neighborsInFOV++;
                cohesionVector += cohesionNeighbors[i].myTransform.position;
            }
        }
        if (neighborsInFOV == 0)
            return cohesionVector;
        cohesionVector /= neighborsInFOV;
        cohesionVector -= myTransform.position;
        cohesionVector = cohesionVector.normalized;
        return cohesionVector;
    }

    private Vector3 CalculateAvoidanceVector()
    {
        var avoidanceVector = Vector3.zero;
        if(avoidanceNeighbors.Count == 0)
            return Vector3.zero;
        int neighborsInFOV = 0;
        for (int i = 0; i < avoidanceNeighbors.Count; i++)
        {
            if (IsInFOV(avoidanceNeighbors[i].myTransform.position))
            {
                neighborsInFOV++;
                avoidanceVector += (myTransform.position - avoidanceNeighbors[i].myTransform.position);
            }
        }

        avoidanceVector /= neighborsInFOV;
        avoidanceVector = avoidanceVector.normalized;
        return avoidanceVector;
    }

    private Vector3 CalculateAlignmentVector()
    {
        var alignmentVector = myTransform.forward;
        if(alignmentNeighbors.Count == 0)
            return alignmentVector;
        int neighborsInFOV = 0;
        for (int i = 0; i < alignmentNeighbors.Count; i++)
        {
            if(IsInFOV(alignmentNeighbors[i].myTransform.position))
            {
                neighborsInFOV++;
                alignmentVector += alignmentNeighbors[i].myTransform.forward;
            }
        }
        if(neighborsInFOV == 0)
            return myTransform.forward;
        alignmentVector /= neighborsInFOV;
        alignmentVector = alignmentVector.normalized;
        return alignmentVector;
    }

    private Vector3 CalculateBoundsVector()
    {
        var offsetToCenter = assignedFlock.transform.position  - myTransform.position;
        bool isNearCenter = (offsetToCenter.magnitude /assignedFlock.boundsDistance <= 0.9f);
        return isNearCenter ? offsetToCenter.normalized : Vector3.zero;
    }

    private Vector3 CalculateObstacleVector()
    {
        var obstacleVector = Vector3.zero;
        RaycastHit hit;
        if(Physics.Raycast(myTransform.position, myTransform.forward, out hit, assignedFlock.obstacleDistance, obstacleMask))
        {
            obstacleVector = FindDirectionToAvoidObstacle();
        }
        else 
        {
            currentObstacleAvoidanceVector = Vector3.zero;
        }
        return obstacleVector;
    }

    private Vector3 FindDirectionToAvoidObstacle()
    {
        if (currentObstacleAvoidanceVector != Vector3.zero)
        {
            RaycastHit hit;
            if (!Physics.Raycast(myTransform.position, myTransform.forward, out hit, assignedFlock.obstacleDistance, obstacleMask))
            {
                return currentObstacleAvoidanceVector;
            }
        }
        float maxDistance = int.MinValue;
        var selectedDirection = Vector3.zero;
        for (int i = 0; i < directionsToCheckToAvoidObstacles.Length; i++)
        {
            RaycastHit hit;
            var currentDirection = myTransform.TransformDirection(directionsToCheckToAvoidObstacles[i].normalized);
            if (Physics.Raycast(myTransform.position, currentDirection, out hit, assignedFlock.obstacleDistance, obstacleMask))
            {
                float currentDistance = (hit.point - myTransform.position).sqrMagnitude;
                if (currentDistance > maxDistance)
                {
                    maxDistance = currentDistance;
                    selectedDirection = currentDirection;
                }
            }
            else
            {
                selectedDirection = currentDirection;
                currentObstacleAvoidanceVector = currentDirection.normalized;
                return selectedDirection.normalized;
            }
        }
        return selectedDirection.normalized;
    }

    private bool IsInFOV(Vector3 position)
    {
        return Vector3.Angle(myTransform.forward, position - myTransform.position) <= FOVangle;
    }

}
