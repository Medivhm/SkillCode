using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        PlayerController player = (PlayerController)target;

        player.groundMoveSpeed = EditorGUILayout.FloatField("目前地面速度", player.groundMoveSpeed);
        player.airMoveSpeed = EditorGUILayout.FloatField("目前空中速度", player.airMoveSpeed);
        player.commonGroundSpeed = EditorGUILayout.FloatField("基础地面移动速度", player.commonGroundSpeed);
        player.shiftSpeed = EditorGUILayout.FloatField("Shift地面移动速度", player.shiftSpeed);
        player.squatSpeed = EditorGUILayout.FloatField("下蹲移动速度", player.squatSpeed);
        player.commonAirSpeed = EditorGUILayout.FloatField("基础空中移动速度", player.commonAirSpeed);
        GUILayout.Space(15f);
        player.jumpSpeed = EditorGUILayout.FloatField("起跳速度", player.jumpSpeed);
        player.movementSharpness = EditorGUILayout.FloatField("移动平滑度", player.movementSharpness);
        player.drag = EditorGUILayout.FloatField("全阻力", player.drag);
        player.airAccelerationSpeed = EditorGUILayout.FloatField("空中水平阻力", player.airAccelerationSpeed); 
        player.gravity = EditorGUILayout.Vector3Field("重力", player.gravity); 
        player.maxStableSlopeAngle = EditorGUILayout.FloatField("可走斜坡角度", player.maxStableSlopeAngle);
        player.MaxStepHeight = EditorGUILayout.FloatField("可走台阶高度", player.MaxStepHeight);
        player.moveDistanceTick = EditorGUILayout.FloatField("移动n米做tick", player.moveDistanceTick);
        GUILayout.Space(15f);
        player.CapsuleRadius = EditorGUILayout.FloatField("胶囊体半径", player.CapsuleRadius);
        player.CapsuleHeight = EditorGUILayout.FloatField("胶囊体高度", player.CapsuleHeight); 
        player.CapsuleYOffset = EditorGUILayout.FloatField("胶囊体Y轴偏移", player.CapsuleYOffset);
        GUILayout.Space(15f);

        DrawDefaultInspector();


        if (UnityEngine.GUI.changed)
        {
            EditorUtility.SetDirty(player);
            serializedObject.ApplyModifiedProperties();
        }
    }
}