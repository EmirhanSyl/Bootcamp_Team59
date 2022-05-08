using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;
using UnityEditor;

[CustomEditor(typeof(EnemyBehaviours)), CanEditMultipleObjects]
public class EnemyBehaviourEditor : Editor
{
    public SerializedProperty
        enemyStateTypeDropdown_Prop,
        regionDrowpdown_Prop,

        enemyHealth_Prop,
        movementSpeed_Prop,
        SprintSpeed_Prop,
        stunnedTimeAfterFamageTaken_Prop,
        maxDistanceToTarget_Prop,
        attackRange_Prop,
        attackDuration_Prop,
        minHitDamage_Prop,
        maxHitDamage_Prop,
        rotatioSpeed_Prop,
        controllingByGroupManager_Prop,
        attackAnimatonsList_Prop,
        counterParticles_Prop,
        bloodParticles_Prop,
        groundMask_Prop,

        walkRange_Prop,
        waitTime_Prop,

        protectedResource_Prop,
        resourceAreaBorderRange_Prop,
        protectedAreaBorderRange_Prop,

        targetObject_Prop;

    private void OnEnable()
    {
        enemyStateTypeDropdown_Prop = serializedObject.FindProperty("enemyStateTypeDropdown");
        regionDrowpdown_Prop = serializedObject.FindProperty("regionDrowpdown");


        enemyHealth_Prop = serializedObject.FindProperty("enemyHealth");
        movementSpeed_Prop = serializedObject.FindProperty("movementSpeed");
        SprintSpeed_Prop = serializedObject.FindProperty("SprintSpeed");
        rotatioSpeed_Prop = serializedObject.FindProperty("rotatioSpeed");

        maxDistanceToTarget_Prop = serializedObject.FindProperty("maxDistanceToTarget");
        stunnedTimeAfterFamageTaken_Prop = serializedObject.FindProperty("stunnedTimeAfterFamageTaken");

        attackRange_Prop = serializedObject.FindProperty("attackRange");
        attackDuration_Prop = serializedObject.FindProperty("attackDuration");
        minHitDamage_Prop = serializedObject.FindProperty("minHitDamage");
        maxHitDamage_Prop = serializedObject.FindProperty("maxHitDamage");

        controllingByGroupManager_Prop = serializedObject.FindProperty("controllingByGroupManager");
        attackAnimatonsList_Prop = serializedObject.FindProperty("attackAnimatonsList");
        counterParticles_Prop = serializedObject.FindProperty("counterParticles");
        bloodParticles_Prop = serializedObject.FindProperty("bloodParticles");
        groundMask_Prop = serializedObject.FindProperty("groundMask");

        walkRange_Prop = serializedObject.FindProperty("walkRange");
        waitTime_Prop = serializedObject.FindProperty("waitTime");

        protectedResource_Prop = serializedObject.FindProperty("protectedResource");
        resourceAreaBorderRange_Prop = serializedObject.FindProperty("resourceAreaBorderRange");
        protectedAreaBorderRange_Prop = serializedObject.FindProperty("protectedAreaBorderRange");

        targetObject_Prop = serializedObject.FindProperty("targetObject");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(regionDrowpdown_Prop);
        EnemyBehaviours.Region region = (EnemyBehaviours.Region)regionDrowpdown_Prop.enumValueIndex;
        EditorGUILayout.PropertyField(controllingByGroupManager_Prop, new GUIContent("Controlling By GroupManager"));
        EditorGUILayout.Space(20);

        EditorGUILayout.PropertyField(enemyHealth_Prop, new GUIContent("Enemy Health"));
        EditorGUILayout.PropertyField(movementSpeed_Prop, new GUIContent("Movement Speed"));
        EditorGUILayout.PropertyField(SprintSpeed_Prop, new GUIContent("Sprint Speed"));
        EditorGUILayout.PropertyField(rotatioSpeed_Prop, new GUIContent("Rotation Speed"));
        EditorGUILayout.Space(20);

        EditorGUILayout.PropertyField(maxDistanceToTarget_Prop, new GUIContent("Max Distance To Target"));
        EditorGUILayout.PropertyField(stunnedTimeAfterFamageTaken_Prop, new GUIContent("Stunned Time After Damage Taken"));
        EditorGUILayout.Space(20);

        EditorGUILayout.PropertyField(attackRange_Prop, new GUIContent("Attack Range"));
        EditorGUILayout.PropertyField(attackDuration_Prop, new GUIContent("Attack Duration"));
        EditorGUILayout.PropertyField(minHitDamage_Prop, new GUIContent("Min Hit Damage"));
        EditorGUILayout.PropertyField(maxHitDamage_Prop, new GUIContent("Max Hit Damage"));
        EditorGUILayout.Space(20);

        EditorGUILayout.PropertyField(attackAnimatonsList_Prop, new GUIContent("Attack Animatons List"));
        EditorGUILayout.PropertyField(counterParticles_Prop, new GUIContent("Counter Particles"));
        EditorGUILayout.PropertyField(bloodParticles_Prop, new GUIContent("Blood Particles"));
        EditorGUILayout.PropertyField(groundMask_Prop, new GUIContent("Ground Mask"));
        EditorGUILayout.Space(20);

        EditorGUILayout.PropertyField(enemyStateTypeDropdown_Prop);
        EnemyBehaviours.EnemyStateType enemyStateType = (EnemyBehaviours.EnemyStateType)enemyStateTypeDropdown_Prop.enumValueIndex;

        switch (enemyStateType)
        {
            case EnemyBehaviours.EnemyStateType.Purposeless:
                EditorGUILayout.PropertyField(walkRange_Prop, new GUIContent("Walk Range"));
                EditorGUILayout.PropertyField(waitTime_Prop, new GUIContent("Wait Time"));
                break;
            case EnemyBehaviours.EnemyStateType.Guardian:
                EditorGUILayout.PropertyField(protectedResource_Prop, new GUIContent("Protected Resource"));
                EditorGUILayout.PropertyField(resourceAreaBorderRange_Prop, new GUIContent("Resource Area Border Range"));
                EditorGUILayout.PropertyField(protectedAreaBorderRange_Prop, new GUIContent("Protected Area Border Range"));
                break;
            case EnemyBehaviours.EnemyStateType.TowerWizard:
                EditorGUILayout.PropertyField(attackRange_Prop, new GUIContent("Attack Range"));
                EditorGUILayout.PropertyField(attackDuration_Prop, new GUIContent("Attack Duration"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
