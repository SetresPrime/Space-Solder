using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField]
    private Transform _cameraCenter;
    [SerializeField]
    private Joystick _joy;

    [Header("Settings")]
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _mouseSens;
    [SerializeField]
    private float _mobileSens;

    [SerializeField]
    private float minimumAngle;
    [SerializeField]
    private float maximumAngle;

    private bool _isPressed = false;
    private int _fingerId = -1;

    private Transform _mainCamera;
    private Rigidbody _rb;
    private Animator _anim;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _mainCamera = Camera.main.transform;

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }

    void FixedUpdate()
    {
        Move();
        if (_isPressed)
            Rotation();
#if UNITY_EDITOR
        Rotation();
#endif
    }
    private void Move()
    {

        float moveHorizontal = _joy.Horizontal;
        float moveVertical = _joy.Vertical;

#if UNITY_EDITOR        
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
#endif

        Vector3 camF = _mainCamera.forward;
        Vector3 camR = _mainCamera.right;
        camF.y = 0f;
        camR.y = 0f;

        Vector3 movingVector = moveHorizontal * camR.normalized + moveVertical * camF.normalized;

        _anim.SetFloat("For", moveVertical);
        _anim.SetFloat("straf", moveHorizontal);

        _rb.AddForce(movingVector * _moveSpeed, ForceMode.Impulse);
    }
    private void Rotation()
    {
        float aimX = Input.GetAxis("Mouse X");
        float aimY = Input.GetAxis("Mouse Y");
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == _fingerId && touch.phase == TouchPhase.Moved)
            {
                aimX = touch.deltaPosition.x * _mobileSens;
                aimY = touch.deltaPosition.y * _mobileSens;
            }
        }
        transform.rotation *= Quaternion.AngleAxis(aimX * _mouseSens, Vector3.up);
        _cameraCenter.rotation *= Quaternion.AngleAxis(-aimY * _mouseSens, Vector3.right);

        var angleX = _cameraCenter.localEulerAngles.x;

        if (angleX > 180 && maximumAngle > angleX)
            angleX = maximumAngle;
        else if (angleX < 180 && minimumAngle < angleX)
            angleX = minimumAngle;

        if (angleX != _cameraCenter.localEulerAngles.x)
            _cameraCenter.localEulerAngles = new Vector3(angleX, _cameraCenter.localEulerAngles.y, 0);

#if UNITY_EDITOR     

        if (_cameraCenter.localEulerAngles.y != 0 || _cameraCenter.localEulerAngles.z != 0)
            _cameraCenter.localEulerAngles = new Vector3(angleX, 0, 0);
#endif
    }
    public void Touch()
    {
        _isPressed = true;
        _fingerId = Input.touches[Input.touchCount - 1].fingerId;
    }
    public void Untouch()
    {
        _isPressed = false;
        _fingerId = -1;
    }
}
