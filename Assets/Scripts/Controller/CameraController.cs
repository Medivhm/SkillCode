using System.Collections.Generic;
using UnityEngine;

public enum CameraView
{
    ThirdPerson,            // third-person view
    OverShoulder,           // over-the-shoulder view
}

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;

    float distance = 10f;
    Vector3 tpOffset = new Vector3(0, 5, 0);
    Vector3 osOffset = new Vector3(0, 3, 0);

    float yaw = 0f;
    float yawSpeed = 0.2f;

    float pitchMin = -80f;
    float pitch = 35f;
    float pitchMax = 80f;
    float pitchSpeed = 0.2f;
    CameraView cameraView;

    public CameraView CameraView => cameraView;

    public Vector3 Forward
    {
        get
        {
            return GetForward();
        }
    }

    public Vector3 Right
    {
        get
        {
            return new Vector3(Forward.z, 0, -Forward.x);
        }
    }

    void Start()
    {
        Main.MainCameraCtrl = this;
        Main.MainCamera = mainCamera;
        style = new GUIStyle();
        style.normal.background = Texture2D.whiteTexture;
        Ctrl.UnUseMouse();
        cameraView = CameraView.ThirdPerson;


    }

    private void OnDestroy()
    {
        Main.MainCameraCtrl = null;
        Main.MainCamera = null;
    }

    void Update()
    {
        //MouseRotateTick();
        //MoveTick();
    }


    private void LateUpdate()
    {
        float mouseLookAxisUp = Input.GetAxisRaw("Mouse Y"); 
        float mouseLookAxisRight = Input.GetAxisRaw("Mouse X");
        float scrollInput = -Input.GetAxis("Mouse ScrollWheel");
        Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);
        UpdateWithInput(Time.deltaTime, 0f, lookInputVector);
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    [Header("MyYOffset")]
    public float myYOffset = 0f;
    public float YOffsetSharpness = 10f;

    [Header("Framing")]
    public Camera Camera;
    public Vector3 FollowPointFraming = new Vector3(0f, 0f, 0f);
    public float FollowingSharpness = 10000f;

    [Header("Distance")]
    public float DefaultDistance = 6f;
    public float MinDistance = 0f;
    public float MaxDistance = 10f;
    public float DistanceMovementSpeed = 5f;
    public float DistanceMovementSharpness = 10f;

    [Header("Rotation")]
    public bool InvertX = false;
    public bool InvertY = false;
    [Range(-90f, 90f)]
    public float DefaultVerticalAngle = 20f;
    [Range(-90f, 90f)]
    public float MinVerticalAngle = -90f;
    [Range(-90f, 90f)]
    public float MaxVerticalAngle = 90f;
    public float RotationSpeed = 1f;
    public float RotationSharpness = 10000f;
    public bool RotateWithPhysicsMover = false;

    [Header("Obstruction")]
    public float ObstructionCheckRadius = 0.2f;
    public LayerMask ObstructionLayers = -1;
    public float ObstructionSharpness = 10000f;
    public List<Collider> IgnoredColliders = new List<Collider>();

    public Transform Transform { get; private set; }
    public Transform FollowTransform { get; private set; }

    public Vector3 PlanarDirection { get; set; }
    public float TargetDistance { get; set; }

    private bool _distanceIsObstructed;
    private float _currentDistance;
    private float _targetVerticalAngle;
    private RaycastHit _obstructionHit;
    private int _obstructionCount;
    private RaycastHit[] _obstructions = new RaycastHit[MaxObstructions];
    private float _obstructionTime;
    private Vector3 _currentFollowPosition;

    private const int MaxObstructions = 32;

    void OnValidate()
    {
        DefaultDistance = Mathf.Clamp(DefaultDistance, MinDistance, MaxDistance);
        DefaultVerticalAngle = Mathf.Clamp(DefaultVerticalAngle, MinVerticalAngle, MaxVerticalAngle);
    }

    void Awake()
    {
        Transform = this.transform;

        _currentDistance = DefaultDistance;
        TargetDistance = _currentDistance;

        _targetVerticalAngle = 0f;

        PlanarDirection = Vector3.forward;
    }

    // Set the transform that the camera will orbit around
    public void SetFollowTransform(Transform t, Transform closeHit)
    {
        FollowTransform = t;
        PlanarDirection = FollowTransform.forward;
        _currentFollowPosition = FollowTransform.position;
        closeHitTrans = closeHit;
    }

    Transform closeHitTrans;
    public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
    {
        if (FollowTransform)
        {
            if (InvertX)
            {
                rotationInput.x *= -1f;
            }
            if (InvertY)
            {
                rotationInput.y *= -1f;
            }

            // Process rotation input
            Quaternion rotationFromInput = Quaternion.Euler(FollowTransform.up * (rotationInput.x * RotationSpeed));
            PlanarDirection = rotationFromInput * PlanarDirection;
            PlanarDirection = Vector3.Cross(FollowTransform.up, Vector3.Cross(PlanarDirection, FollowTransform.up));
            Quaternion planarRot = Quaternion.LookRotation(PlanarDirection, FollowTransform.up);

            _targetVerticalAngle -= (rotationInput.y * RotationSpeed);
            _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, MinVerticalAngle, MaxVerticalAngle);
            Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
            Quaternion targetRotation = Quaternion.Slerp(Transform.rotation, planarRot * verticalRot, 1f - Mathf.Exp(-RotationSharpness * deltaTime));

            // Apply rotation
            Transform.rotation = targetRotation;

            // Process distance input
            if (_distanceIsObstructed && Mathf.Abs(zoomInput) > 0f)
            {
                TargetDistance = _currentDistance;
            }
            TargetDistance += zoomInput * DistanceMovementSpeed;
            TargetDistance = Mathf.Clamp(TargetDistance, MinDistance, MaxDistance);

            // Find the smoothed follow position
            _currentFollowPosition = Vector3.Lerp(_currentFollowPosition, FollowTransform.position.CopyAndChangeY(_currentFollowPosition.y), 1f - Mathf.Exp(-FollowingSharpness * deltaTime));
            _currentFollowPosition = _currentFollowPosition.CopyAndChangeY(Mathf.Lerp(_currentFollowPosition.y, FollowTransform.position.y, 1f - Mathf.Exp(-YOffsetSharpness * deltaTime)));
            //_currentFollowPosition = FollowTransform.position;

            // Handle obstructions
            {
                RaycastHit closestHit = new RaycastHit();
                closestHit.distance = Mathf.Infinity;
                _obstructionCount = Physics.SphereCastNonAlloc(
                                                            _currentFollowPosition + (_currentDistance - MinDistance) / (MaxDistance - MinDistance) * Transform.up * myYOffset,            // 球形射线的起始位置（跟随的目标位置）
                                                            ObstructionCheckRadius,            // 检测半径
                                                            -Transform.forward,                // 射线的方向（从相机指向目标）
                                                            _obstructions,                     // 存储遮挡物的数组
                                                            TargetDistance,                    // 射线的最大检测距离（目标距离）
                                                            ObstructionLayers,                 // 需要检测的图层
                                                            QueryTriggerInteraction.Ignore);   // 忽略触发器
                for (int i = 0; i < _obstructionCount; i++)
                {
                    bool isIgnored = false;
                    for (int j = 0; j < IgnoredColliders.Count; j++)
                    {
                        if (IgnoredColliders[j] == _obstructions[i].collider)
                        {
                            isIgnored = true;
                            break;
                        }
                    }
                    for (int j = 0; j < IgnoredColliders.Count; j++)
                    {
                        if (IgnoredColliders[j] == _obstructions[i].collider)
                        {
                            isIgnored = true;
                            break;
                        }
                    }

                    if (!isIgnored && _obstructions[i].distance < closestHit.distance && _obstructions[i].distance > 0)
                    {
                        closestHit = _obstructions[i];
                    }
                }


                // If obstructions detecter
                if (closestHit.distance < Mathf.Infinity)
                {
                    _distanceIsObstructed = true;
                    _currentDistance = Mathf.Lerp(_currentDistance, closestHit.distance, 1 - Mathf.Exp(-ObstructionSharpness * deltaTime));
                }
                // If no obstruction
                else
                {
                    _distanceIsObstructed = false;
                    _currentDistance = Mathf.Lerp(_currentDistance, TargetDistance, 1 - Mathf.Exp(-DistanceMovementSharpness * deltaTime));
                }
            }

            // Find the smoothed camera orbit position
            Vector3 targetPosition = _currentFollowPosition - ((targetRotation * Vector3.forward) * _currentDistance);

            // Handle framing
            targetPosition += Transform.right * FollowPointFraming.x;
            targetPosition += Transform.up * FollowPointFraming.y;
            targetPosition += Transform.forward * FollowPointFraming.z;
            targetPosition += (_currentDistance - MinDistance) / (MaxDistance - MinDistance) * Transform.up * myYOffset;

            // Apply position
            Transform.position = targetPosition;
        }
    }

    void OnDrawGizmos()
    {
        if (FollowTransform)
        {
            // 设置 Gizmos 的颜色
            Gizmos.color = Color.red;
            Vector3 aa = _currentFollowPosition + (_currentDistance - MinDistance) / (MaxDistance - MinDistance) * Transform.up * myYOffset;

            // 绘制球形检测区域 (使用相机朝向角色的方向)
            Vector3 spherePosition = aa - (Transform.forward * _currentDistance);

            // 使用 Gizmos 绘制球形区域
            Gizmos.DrawWireSphere(spherePosition, ObstructionCheckRadius);

            // 如果有遮挡物，可以绘制最近遮挡物的位置
            if (_distanceIsObstructed)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(aa - (Transform.forward * _currentDistance), ObstructionCheckRadius);
            }

            // 可选：在相机和跟随目标之间绘制一条线，便于调试
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Transform.position, aa);
        }
    }


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>


    bool mouseRotate = true;
    float changeEulerYaw;
    float changeEulerPitch;
    float mouseX;
    float mouseY;
    void MouseRotateTick()
    {
        if (mouseRotate)
        {
            mouseX = Input.GetAxis("Mouse X") * Main.mouseSensitivity;
            mouseY = Input.GetAxis("Mouse Y") * Main.mouseSensitivity;

            if(CameraView.ThirdPerson == CameraView)
            {
                changeEulerYaw = yawSpeed * mouseX;
                changeEulerPitch = pitchSpeed * mouseY;
            }
            else
            {
                changeEulerYaw = yawSpeed * mouseX * 0.1f;
                changeEulerPitch = pitchSpeed * mouseY * 0.1f;
            }

            yaw -= changeEulerYaw; // * CanvasScaler
            pitch -= changeEulerPitch;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

            // 角度这里有问题
            //this.transform.eulerAngles = this.transform.eulerAngles.CopyAndChangeY(yaw);
        }
    }

    //float ySpeed = 4f;
    Vector3 TargetPosition;
    void MoveTick()
    {
        if (Main.MainPlayerCtrl.IsNull()) return;

        TargetPosition = Main.MainPlayerCtrl.transform.position
            + new Vector3(distance * Mathf.Cos(Mathf.Deg2Rad * pitch) * Mathf.Sin(Mathf.Deg2Rad * yaw),
                          distance * Mathf.Sin(Mathf.Deg2Rad * pitch),
                          -distance * Mathf.Cos(Mathf.Deg2Rad * pitch) * Mathf.Cos(Mathf.Deg2Rad * yaw)); ;

        this.transform.position = TargetPosition;

        //float yDistance = TargetPosition.y - this.transform.position.y;
        //if (Mathf.Abs(yDistance) > 1f)
        //{
        //    float moveYDistance = Mathf.Sign(yDistance) * ySpeed * Time.deltaTime;
        //    DebugTool.LogFormat(Mathf.Sign(yDistance).ToString());
        //    this.transform.position = this.transform.position.CopyAndChangeY(this.transform.position.y + moveYDistance);
        //}

        // todo: 优化
        this.transform.LookAt(Main.MainPlayerCtrl.transform);
        this.transform.position += tpOffset;

        if(CameraView.OverShoulder == cameraView)
        {
            this.mainCamera.transform.position = Main.MainPlayerCtrl.magicShootPoint.position;
            this.mainCamera.transform.position += osOffset;
            this.mainCamera.transform.rotation = this.transform.rotation;
        }
    }

    Vector3 cameraToPlayer;
    Vector3 GetForward()
    {
        //cameraToPlayer = Main.MainPlayer.transform.position - this.transform.position;
        //if (Main.MainPlayer)
        //{
        //    return new Vector3(cameraToPlayer.x, 0, cameraToPlayer.z).normalized;
        //}
        //return Vector3.zero;

        cameraToPlayer = this.transform.forward;
        cameraToPlayer.y = 0;
        return cameraToPlayer.normalized;
    }

    public void SetMouseRotate(bool state)
    {
        mouseRotate = state;
    }

    // 在编辑器里修改这个
    public float lineHeight = 10f;
    public float lineWidth = 3f;
    public float lineOffset = 5f;
    private GUIStyle style;
    private Texture tex;
    private Texture2D texture2D;
    private void OnGUI()
    {
        if(!MainMouseController.Instance.mouseShow)
        {
            UnityEngine.GUI.color = Color.white;
            //  左准星
            UnityEngine.GUI.Box(new Rect(Screen.width / 2 - lineOffset - lineHeight, Screen.height / 2 - lineWidth / 2, lineHeight, lineWidth), tex, style);
            //  右准星
            UnityEngine.GUI.Box(new Rect(Screen.width / 2 + lineOffset, Screen.height / 2 - lineWidth / 2, lineHeight, lineWidth), tex, style);
            //  上准星
            UnityEngine.GUI.Box(new Rect(Screen.width / 2 - lineWidth / 2, Screen.height / 2 + lineOffset, lineWidth, lineHeight), tex, style);
            //  下准星
            UnityEngine.GUI.Box(new Rect(Screen.width / 2 - lineWidth / 2, Screen.height / 2 - lineOffset - lineHeight, lineWidth, lineHeight), tex, style);
        }
    }

    public void SetCameraView(CameraView view)
    {
        cameraView = view;
        if(CameraView.ThirdPerson == view)
        {
            mainCamera.transform.localPosition = Vector3.zero;
            mainCamera.transform.rotation = this.transform.rotation;
        }
    }

}
