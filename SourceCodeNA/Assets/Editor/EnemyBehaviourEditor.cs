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
        poisonedSpeed_Prop,
        stunnedTimeAfterFamageTaken_Prop,
        knocbackDevider_Prop,
        maxDistanceToTarget_Prop,
        attackRange_Prop,
        attackDuration_Prop,
        minHitDamage_Prop,
        maxHitDamage_Prop,
        rotatioSpeed_Prop,
        controllingByGroupManager_Prop,
        poisonedTakeDamageDuration_Prop,
        healDuration_prop,
        poisonedHitCount_Prop,
        attackAnimatonsList_Prop,
        poisonedDamageParticles_Prop,
        bloodParticles_Prop,
        groundMask_Prop,
        enemyLayers_Prop,

        walkRange_Prop,
        waitTime_Prop,

        projectile_Prop,
        projectileInitLocation_Prop,

        dodgeChance_Prop,

        protectedResource_Prop,
        resourceAreaBorderRange_Prop,
        protectedAreaBorderRange_Prop,

        collectAnimatonsList_Prop,
        carryingCapacity_Prop,

        hugeKnightNPC_Prop,
        enemyHealthBar_Prop,

        targetObject_Prop;

    private void OnEnable()
    {
        enemyStateTypeDropdown_Prop = serializedObject.FindProperty("enemyStateTypeDropdown");
        regionDrowpdown_Prop = serializedObject.FindProperty("regionDrowpdown");

        enemyHealth_Prop = serializedObject.FindProperty("enemyHealth");
        movementSpeed_Prop = serializedObject.FindProperty("movementSpeed");
        SprintSpeed_Prop = serializedObject.FindProperty("SprintSpeed");
        poisonedSpeed_Prop = serializedObject.FindProperty("poisonedSpeed");
        rotatioSpeed_Prop = serializedObject.FindProperty("rotatioSpeed");

        maxDistanceToTarget_Prop = serializedObject.FindProperty("maxDistanceToTarget");
        stunnedTimeAfterFamageTaken_Prop = serializedObject.FindProperty("stunnedTimeAfterFamageTaken");
        knocbackDevider_Prop = serializedObject.FindProperty("knocbackDevider");

        attackRange_Prop = serializedObject.FindProperty("attackRange");
        attackDuration_Prop = serializedObject.FindProperty("attackDuration");
        minHitDamage_Prop = serializedObject.FindProperty("minHitDamage");
        maxHitDamage_Prop = serializedObject.FindProperty("maxHitDamage");

        controllingByGroupManager_Prop = serializedObject.FindProperty("controllingByGroupManager");
        poisonedTakeDamageDuration_Prop = serializedObject.FindProperty("poisonedTakeDamageDuration");
        poisonedHitCount_Prop = serializedObject.FindProperty("poisonedHitCount");
        healDuration_prop = serializedObject.FindProperty("healDuration");
        attackAnimatonsList_Prop = serializedObject.FindProperty("attackAnimatonsList");
        poisonedDamageParticles_Prop = serializedObject.FindProperty("poisonedDamageParticles");
        bloodParticles_Prop = serializedObject.FindProperty("bloodParticles");
        groundMask_Prop = serializedObject.FindProperty("groundMask");
        enemyLayers_Prop = serializedObject.FindProperty("enemyLayers");

        walkRange_Prop = serializedObject.FindProperty("walkRange");
        waitTime_Prop = serializedObject.FindProperty("waitTime");

        projectile_Prop = serializedObject.FindProperty("projectile");
        projectileInitLocation_Prop = serializedObject.FindProperty("projectileInitLocation");

        dodgeChance_Prop = serializedObject.FindProperty("dodgeChance");

        protectedResource_Prop = serializedObject.FindProperty("protectedResource");
        resourceAreaBorderRange_Prop = serializedObject.FindProperty("resourceAreaBorderRange");
        protectedAreaBorderRange_Prop = serializedObject.FindProperty("protectedAreaBorderRange");

        collectAnimatonsList_Prop = serializedObject.FindProperty("collectAnimatonsList");
        carryingCapacity_Prop = serializedObject.FindProperty("carryingCapacity");

        hugeKnightNPC_Prop = serializedObject.FindProperty("hugeKnightNPC");
        enemyHealthBar_Prop = serializedObject.FindProperty("enemyHealthBar");

        targetObject_Prop = serializedObject.FindProperty("targetObject");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var enemyBehaviours = target as EnemyBehaviours;
        enemyBehaviours.controllingByGroupManager = GUILayout.Toggle(enemyBehaviours.controllingByGroupManager, "Controlling By GroupManager");

        if (!enemyBehaviours.controllingByGroupManager)
        {
            EditorGUILayout.PropertyField(regionDrowpdown_Prop);
            EnemyBehaviours.Region region = (EnemyBehaviours.Region)regionDrowpdown_Prop.enumValueIndex;
            EditorGUILayout.PropertyField(enemyLayers_Prop, new GUIContent("Enemy Mask"));
        }
        EditorGUILayout.Space(10);

        EditorGUILayout.PropertyField(enemyHealth_Prop, new GUIContent("Enemy Health"));
        EditorGUILayout.PropertyField(movementSpeed_Prop, new GUIContent("Movement Speed"));
        EditorGUILayout.PropertyField(SprintSpeed_Prop, new GUIContent("Sprint Speed"));
        EditorGUILayout.PropertyField(poisonedSpeed_Prop, new GUIContent("Poisoned Speed"));
        EditorGUILayout.PropertyField(rotatioSpeed_Prop, new GUIContent("Rotation Speed"));
        EditorGUILayout.Space(20);

        EditorGUILayout.PropertyField(maxDistanceToTarget_Prop, new GUIContent("Max Distance To Target"));
        EditorGUILayout.PropertyField(stunnedTimeAfterFamageTaken_Prop, new GUIContent("Stunned Time After Damage Taken"));
        EditorGUILayout.PropertyField(knocbackDevider_Prop, new GUIContent("Knocback Devider", "The amount of knokback. (camera.forward/knocbackDevider)"));
        EditorGUILayout.PropertyField(poisonedTakeDamageDuration_Prop, new GUIContent("Poisoned Take Damage Duration"));
        EditorGUILayout.PropertyField(poisonedHitCount_Prop, new GUIContent("PoisonedHit Count"));
        EditorGUILayout.PropertyField(healDuration_prop, new GUIContent("Heal Duration"));
        EditorGUILayout.Space(20);

        EditorGUILayout.PropertyField(attackRange_Prop, new GUIContent("Attack Range"));
        EditorGUILayout.PropertyField(attackDuration_Prop, new GUIContent("Attack Duration"));
        EditorGUILayout.PropertyField(minHitDamage_Prop, new GUIContent("Min Hit Damage"));
        EditorGUILayout.PropertyField(maxHitDamage_Prop, new GUIContent("Max Hit Damage"));
        EditorGUILayout.Space(20);

        EditorGUILayout.PropertyField(groundMask_Prop, new GUIContent("Ground Mask"));
        EditorGUILayout.PropertyField(enemyHealthBar_Prop, new GUIContent("Enemy Health Bar"));
        EditorGUILayout.PropertyField(poisonedDamageParticles_Prop, new GUIContent("Counter Particles"));
        EditorGUILayout.PropertyField(bloodParticles_Prop, new GUIContent("Blood Particles"));
        EditorGUILayout.PropertyField(attackAnimatonsList_Prop, new GUIContent("Attack Animatons List"));
        EditorGUILayout.Space(10);

        EditorGUILayout.PropertyField(enemyStateTypeDropdown_Prop);
        EnemyBehaviours.EnemyStateType enemyStateType = (EnemyBehaviours.EnemyStateType)enemyStateTypeDropdown_Prop.enumValueIndex;

        switch (enemyStateType)
        {
            case EnemyBehaviours.EnemyStateType.Purposeless:
                EditorGUILayout.PropertyField(walkRange_Prop, new GUIContent("Walk Range"));
                EditorGUILayout.PropertyField(waitTime_Prop, new GUIContent("Wait Time"));
                EditorGUILayout.Slider(dodgeChance_Prop, 0f, 1f, new GUIContent("Dodge Chance"));
                EditorGUILayout.PropertyField(hugeKnightNPC_Prop, new GUIContent("Huge Knight NPC"));
                break;
            case EnemyBehaviours.EnemyStateType.Guardian:
                EditorGUILayout.PropertyField(protectedResource_Prop, new GUIContent("Protected Resource"));
                EditorGUILayout.PropertyField(resourceAreaBorderRange_Prop, new GUIContent("Resource Area Border Range"));
                EditorGUILayout.PropertyField(protectedAreaBorderRange_Prop, new GUIContent("Protected Area Border Range"));
                EditorGUILayout.Slider(dodgeChance_Prop, 0f, 1f, new GUIContent("Dodge Chance"));
                EditorGUILayout.PropertyField(hugeKnightNPC_Prop, new GUIContent("Huge Knight NPC"));
                break;
            case EnemyBehaviours.EnemyStateType.TowerWizard:
                EditorGUILayout.PropertyField(attackRange_Prop, new GUIContent("Attack Range"));
                EditorGUILayout.PropertyField(attackDuration_Prop, new GUIContent("Attack Duration"));
                EditorGUILayout.PropertyField(projectile_Prop, new GUIContent("Projectile"));
                EditorGUILayout.PropertyField(projectileInitLocation_Prop, new GUIContent("Projectile Spawn Location"));
                break;
            case EnemyBehaviours.EnemyStateType.RobotWorker:
                EditorGUILayout.PropertyField(protectedResource_Prop, new GUIContent("Protected Resource"));
                EditorGUILayout.PropertyField(resourceAreaBorderRange_Prop, new GUIContent("Resource Area Border Range"));
                EditorGUILayout.PropertyField(carryingCapacity_Prop, new GUIContent("Carrying Capacity"));
                EditorGUILayout.PropertyField(collectAnimatonsList_Prop, new GUIContent("Collect Animatons List"));
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
