using UnityEngine;

public class CameraController : MonoBehaviour
{
    private class CameraState
    {
        public float Yaw;
        public float Degree;
        private float _roll;
        private float _x;
        private float _y;
        private float _z;

        public void SetFromTransform(Transform t)
        {
            var eulerAngles = t.eulerAngles;
            Degree = eulerAngles.x;
            Yaw = eulerAngles.y;
            _roll = eulerAngles.z;
            
            var position = t.position;
            _x = position.x;
            _y = position.y;
            _z = position.z;
        }

        public void Translate(Vector3 translation)
        {
            Vector3 rotatedTranslation = Quaternion.Euler(Degree, Yaw, _roll) * translation;

            _x += rotatedTranslation.x;
            _y += rotatedTranslation.y;
            _z += rotatedTranslation.z;
        }

        public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
        {
            Yaw = Mathf.Lerp(Yaw, target.Yaw, rotationLerpPct);
            Degree = Mathf.Lerp(Degree, target.Degree, rotationLerpPct);
            _roll = Mathf.Lerp(_roll, target._roll, rotationLerpPct);
            
            _x = Mathf.Lerp(_x, target._x, positionLerpPct);
            _y = Mathf.Lerp(_y, target._y, positionLerpPct);
            _z = Mathf.Lerp(_z, target._z, positionLerpPct);
        }

        public void UpdateTransform(Transform t)
        {
            t.eulerAngles = new Vector3(Degree, Yaw, _roll);
            t.position = new Vector3(_x, _y, _z);
        }
    }

    private readonly CameraState _targetCameraState = new CameraState();
    private readonly CameraState _interpolatingCameraState = new CameraState();

    [Header("Movement Settings")]
    [Tooltip("Exponential boost factor on translation, can be controlled by mouse wheel.")]
    public float boost = 3.5f;

    [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
    public float positionLerpTime = 0.2f;

    [Header("Rotation Settings")]
    [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
    public float rotationLerpTime = 0.01f;

    [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
    public bool invertY = false;

    private void OnEnable()
    {
        _targetCameraState.SetFromTransform(transform);
        _interpolatingCameraState.SetFromTransform(transform);
    }

    private Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction += Vector3.up;
        }
        return direction;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false; 
			#endif
        }
        // Hide and lock cursor when right mouse button pressed
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Unlock and show cursor when right mouse button released
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // Rotation
        if (Input.GetMouseButton(2))
        {
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));
            
            var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            _targetCameraState.Yaw += mouseMovement.x * mouseSensitivityFactor;
            _targetCameraState.Degree += mouseMovement.y * mouseSensitivityFactor;
        }
        
        // Translation
        var translation = GetInputTranslationDirection() * Time.deltaTime;

        // Speed up movement when shift key held
        if (Input.GetKey(KeyCode.LeftShift))
        {
            translation *= 10.0f;
        }
        
        // Modify movement by a boost factor (defined in Inspector and modified in play mode through the mouse scroll wheel)
        boost += Input.mouseScrollDelta.y * 0.2f;
        translation *= Mathf.Pow(2.0f, boost);

        _targetCameraState.Translate(translation);

        // Framerate-independent interpolation
        // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
        var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
        var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
        _interpolatingCameraState.LerpTowards(_targetCameraState, positionLerpPct, rotationLerpPct);
        _interpolatingCameraState.UpdateTransform(transform);
    }
}