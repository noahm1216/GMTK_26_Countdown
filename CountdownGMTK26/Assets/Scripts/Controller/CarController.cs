using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    private Rigidbody _RigidBody;

    public bool allWheelDrive;
    public Transform[] wheelRaycastPoints;
    public LayerMask drivableLayers;

    public float driveSpeed = 7500;
    public float driveTorque = 1000;
    public float wheelTraction = 4000;
    public float throttleInput;
    public float steeringInput;
    [Space]
    public float restLength = 0.5f;
    public float springTravel = 0.2f;
    public float springStiffness = 10000;
    public float damperStiffness = 4000;


    private Vector3 startPos;
    private Quaternion startRot;

    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        TryGetComponent<Rigidbody>(out _RigidBody);
        _RigidBody.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    private void FixedUpdate()
    {
        Turn(steeringInput);

        for (int i = 0; i < wheelRaycastPoints.Length; i++)
            CalculateCarSuspension(wheelRaycastPoints[i]);
    }

    private void Update()
    {
        throttleInput = 0;
        steeringInput = 0;

        if (Input.GetKey(KeyCode.W)) throttleInput = 1;
        if (Input.GetKey(KeyCode.S)) throttleInput = -1;
        if (Input.GetKey(KeyCode.D)) steeringInput = 1;
        if (Input.GetKey(KeyCode.A)) steeringInput = -1;
        if (Input.GetKey(KeyCode.R)) ResetCar();
    }

    private void ResetCar()
    {
        transform.position = startPos;
        transform.rotation = startRot;
        _RigidBody.angularVelocity = Vector3.zero;
        _RigidBody.linearVelocity = Vector3.zero;
    }


    private void CalculateCarSuspension(Transform _wheel)
    {
        // test wheel direction
        Debug.DrawRay(_wheel.position, _wheel.forward, Color.blue);
        Debug.DrawRay(_wheel.position, _wheel.up, Color.green);
        Debug.DrawRay(_wheel.position, _wheel.right, Color.red);

        Vector3 rayDir = -_wheel.up;
        float maxDist = restLength + springTravel;
        Debug.DrawRay(_wheel.position, rayDir * maxDist, Color.red);
        if (!Physics.Raycast(_wheel.position, rayDir, out RaycastHit hit, maxDist, drivableLayers)) return;
        Debug.DrawRay(_wheel.position, rayDir * hit.distance, Color.green);
        Vector3 _wheelVel = _RigidBody.GetPointVelocity(_wheel.position);


        // Suspension for the car to bounce / wiggle 
        float compression = Mathf.Clamp(maxDist - hit.distance, 0, springTravel);
        float springForce = compression * springStiffness;
        float damperForce = -Vector3.Dot(_wheelVel, _wheel.up) * damperStiffness;
        _RigidBody.AddForceAtPosition(_wheel.up * (springForce + damperForce), hit.point);


        // Side traction so we dont slide horribly    
        float sideSpeed = Vector3.Dot(_wheelVel, _wheel.right);
        Vector3 sideForce = -_wheel.right * sideSpeed * wheelTraction;
        _RigidBody.AddForceAtPosition(sideForce, hit.point);


        // Engine calculations to get the similar car feeling 
        if (!allWheelDrive && _wheel == wheelRaycastPoints[0] || !allWheelDrive && _wheel == wheelRaycastPoints[1]) return;
        Vector3 driveForce = _wheel.forward * throttleInput * driveSpeed;
        _RigidBody.AddForceAtPosition(driveForce, hit.point);
    }

    private void Turn(float _amount)
    {
        _RigidBody.AddRelativeTorque(Vector3.up * _amount * driveTorque);
    }

}
