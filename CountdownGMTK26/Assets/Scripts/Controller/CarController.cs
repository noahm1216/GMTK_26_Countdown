using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    private Rigidbody _RigidBody;


    public Transform[] wheelRaycastPoints;
    public LayerMask drivableLayers;

    public float driveSpeed = 7500;
    public float driveTorque = 1000;
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
        for (int i = 0; i < wheelRaycastPoints.Length; i++)
            CalculateCarSuspension(wheelRaycastPoints[i]);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W)) Drive(Vector3.forward);
        if (Input.GetKey(KeyCode.S)) Drive(Vector3.back);
        if (Input.GetKey(KeyCode.D)) Turn(1);
        if (Input.GetKey(KeyCode.A)) Turn(-1);
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
        Vector3 rayDir = -_wheel.up;
        float maxDist = restLength + springTravel;

        Debug.DrawRay(_wheel.position, rayDir * maxDist, Color.red);

        if (Physics.Raycast(_wheel.position, rayDir, out RaycastHit hit, maxDist, drivableLayers))
        {
            Debug.DrawRay(_wheel.position, rayDir * hit.distance, Color.green);

            Vector3 springDir = _wheel.up;
            Vector3 wheelVel = _RigidBody.GetPointVelocity(_wheel.position);

            float compression = maxDist - hit.distance;
            compression = Mathf.Clamp(compression, 0, springTravel);

            if (compression > 0)
            {
                float springForce = compression * springStiffness; 
                float damperForce = -damperStiffness * Vector3.Dot(wheelVel, springDir);
                float totalForce = springForce + damperForce;

                _RigidBody.AddForceAtPosition(springDir * totalForce, hit.point);
            }           
        }
    }

    private void Drive(Vector3 _dir)
    {
        _RigidBody.AddRelativeForce(_dir * driveSpeed);
    }

    private void Turn(float _amount)
    {
        _amount =  Mathf.Clamp(_amount, -1, 1);
        _RigidBody.AddRelativeTorque(Vector3.up * driveTorque * _amount);
    }

}
