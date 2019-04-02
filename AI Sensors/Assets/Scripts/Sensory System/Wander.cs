using UnityEngine;

public class Wander : MonoBehaviour
{
    private Vector3 _targetPos;
    [SerializeField] private float _movementSpeed = 0.5f;
    [SerializeField] private float _rotSpeed = 2.0f;
    private float _minX, _maxX, _minZ, _maxZ;

    // Start is called before the first frame update
    void Start()
    {
        _minX = -16.0f;
        _maxX = 16.0f;
        _minZ = -16.0f;
        _maxZ = 16.0f;

        GetNextPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_targetPos, transform.position) <= 5.0f)
        {
            GetNextPosition();
        }

        Quaternion tarRot = Quaternion.LookRotation(_targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, _rotSpeed * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, _movementSpeed * Time.deltaTime));
    }


    private void GetNextPosition()
    {
        _targetPos = new Vector3(Random.Range(_minX,_maxX),0.5f, Random.Range(_minZ,_maxZ));
    }

}
