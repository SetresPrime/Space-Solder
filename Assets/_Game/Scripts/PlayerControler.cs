using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraCenter;
    [SerializeField]
    private Joystick _joy;

    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _mouseSens;

    [SerializeField]
    private float minimumAngle;
    [SerializeField]
    private float maximumAngle;

    private bool _isMobile = false;
    private bool _isPressed = false;
    private int _fingerId;

    private Transform _mainCamera;
    private Rigidbody _rb;
    private Animator _anim;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _mainCamera = Camera.main.transform;

        if (Application.platform == RuntimePlatform.Android)
            _isMobile = true;
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    void FixedUpdate()
    {
        Move();
        Rotation();
    }
    private void Move()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (_isMobile)
        {
            moveHorizontal = _joy.Horizontal;
            moveVertical = _joy.Vertical;
        }

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

        if (_isPressed && _isMobile)
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == _fingerId && touch.phase == TouchPhase.Moved)
                {
                    aimX = touch.deltaPosition.x;
                    aimY = touch.deltaPosition.y;
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

        //Це вирішує помилку, яка з'являється лише у Unity під час запуску гри
        if (_cameraCenter.localEulerAngles.y != 0 || _cameraCenter.localEulerAngles.z != 0)
            _cameraCenter.localEulerAngles = new Vector3(angleX, 0, 0);
    }
    public void Toche()
    {
        _isPressed = true;
        if (_isMobile)
            _fingerId = Input.touches[Input.touchCount - 1].fingerId;
    }
    public void AnToche()
    {
        _isPressed = false;
        if (_isMobile)
            _fingerId = 0;
    }
    
}
