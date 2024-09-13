using Constant;
using Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
        MouseRotateTick();
        MoveTick();
    }

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

    void MoveTick()
    {
        if (Main.MainPlayerCtrl.IsNull()) return;

        this.transform.position = Main.MainPlayerCtrl.transform.position
            + new Vector3(distance * Mathf.Cos(Mathf.Deg2Rad * pitch) * Mathf.Sin(Mathf.Deg2Rad * yaw),
                          distance * Mathf.Sin(Mathf.Deg2Rad * pitch),
                          -distance * Mathf.Cos(Mathf.Deg2Rad * pitch) * Mathf.Cos(Mathf.Deg2Rad * yaw));

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
        return cameraToPlayer;
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
