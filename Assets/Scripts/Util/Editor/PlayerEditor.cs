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

        player.groundMoveSpeed = EditorGUILayout.FloatField("Ŀǰ�����ٶ�", player.groundMoveSpeed);
        player.airMoveSpeed = EditorGUILayout.FloatField("Ŀǰ�����ٶ�", player.airMoveSpeed);
        player.commonGroundSpeed = EditorGUILayout.FloatField("���������ƶ��ٶ�", player.commonGroundSpeed);
        player.shiftSpeed = EditorGUILayout.FloatField("Shift�����ƶ��ٶ�", player.shiftSpeed);
        player.squatSpeed = EditorGUILayout.FloatField("�¶��ƶ��ٶ�", player.squatSpeed);
        player.commonAirSpeed = EditorGUILayout.FloatField("���������ƶ��ٶ�", player.commonAirSpeed);
        GUILayout.Space(15f);
        player.jumpSpeed = EditorGUILayout.FloatField("�����ٶ�", player.jumpSpeed);
        player.movementSharpness = EditorGUILayout.FloatField("�ƶ�ƽ����", player.movementSharpness);
        player.drag = EditorGUILayout.FloatField("ȫ����", player.drag);
        player.airAccelerationSpeed = EditorGUILayout.FloatField("����ˮƽ����", player.airAccelerationSpeed); 
        player.gravity = EditorGUILayout.Vector3Field("����", player.gravity); 
        player.maxStableSlopeAngle = EditorGUILayout.FloatField("����б�½Ƕ�", player.maxStableSlopeAngle);
        player.MaxStepHeight = EditorGUILayout.FloatField("����̨�׸߶�", player.MaxStepHeight);
        player.moveDistanceTick = EditorGUILayout.FloatField("�ƶ�n����tick", player.moveDistanceTick);
        GUILayout.Space(15f);
        player.CapsuleRadius = EditorGUILayout.FloatField("������뾶", player.CapsuleRadius);
        player.CapsuleHeight = EditorGUILayout.FloatField("������߶�", player.CapsuleHeight); 
        player.CapsuleYOffset = EditorGUILayout.FloatField("������Y��ƫ��", player.CapsuleYOffset);
        GUILayout.Space(15f);

        DrawDefaultInspector();


        if (UnityEngine.GUI.changed)
        {
            EditorUtility.SetDirty(player);
            serializedObject.ApplyModifiedProperties();
        }
    }
}