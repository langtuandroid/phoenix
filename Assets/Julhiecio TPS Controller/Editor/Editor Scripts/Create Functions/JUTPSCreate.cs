using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using JUTPS;
using JUTPS.JUInputSystem;
using JUTPS.CameraSystems;
using JUTPS.WeaponSystem;
using JUTPS.Utilities;

namespace JUTPSEditor
{
    public class JUTPSCreate
    {
        [MenuItem("GameObject/JUTPS Create/Game Manager", false, -100)]
        public static void CreateGameManager()
        {
            var gameManager = new GameObject("Game Manager");
            Undo.RegisterCreatedObjectUndo(gameManager, "game manager creation");
            gameManager.AddComponent<JUGameManager>();
        }
        [MenuItem("GameObject/JUTPS Create/Input Manager", false, -99)]
        public static void CreateInputManager()
        {
            var inputManager = new GameObject("JU Input Manager");
            Undo.RegisterCreatedObjectUndo(inputManager, "input manager creation");
            inputManager.AddComponent<JUInputManager>();
        }

        [MenuItem("GameObject/JUTPS Create/Camera/Simple Camera Controller", false, 1)]
        public static void CreateNewCamera()
        {
            var CameraRoot_Controller = new GameObject("Camera Controller");
            Undo.RegisterCreatedObjectUndo(CameraRoot_Controller, "CameraRoot_Controller create");
            CameraRoot_Controller.AddComponent<TPSCameraController>();
            CameraRoot_Controller.transform.position = SceneViewInstantiatePosition();



            var mainCamera = new GameObject("Main Camera");
            Undo.RegisterCreatedObjectUndo(mainCamera, "Main Camera create");
            mainCamera.AddComponent<Camera>();
            mainCamera.AddComponent<AudioListener>();
            mainCamera.transform.tag = "MainCamera";
            mainCamera.transform.position = CameraRoot_Controller.transform.position - CameraRoot_Controller.transform.forward * 1f;
            mainCamera.transform.parent = CameraRoot_Controller.transform;

            var mainCamera_Pivot = new GameObject("Camera Pivot");
            Undo.RegisterCreatedObjectUndo(mainCamera_Pivot, "mainCamera_Pivot create");
            mainCamera_Pivot.transform.position = mainCamera.transform.position;
            mainCamera.transform.parent = mainCamera_Pivot.transform;
            mainCamera_Pivot.transform.parent = CameraRoot_Controller.transform;
        }

        [MenuItem("GameObject/JUTPS Create/Quick Scene Setup", false, -200)]
        public static void QuickSceneSetup()
        {
            CreateInputManager();
            CreateGameManager();
            CreateNewCamera();
        }



        //>>>> WEAPONS

        //[MenuItem("GameObject/JUTPS Create/Weapons.../Weapon Aim Rotation Center", false, 0)]
        public static void CreateNewWeaponRotationCenter()
        {
            var WeaponPivot = new GameObject("Weapon Aim Rotation Center");
            Undo.RegisterCreatedObjectUndo(WeaponPivot, "Hit Box Setup");

            WeaponPivot.AddComponent<WeaponAimRotationCenter>();
            if (Selection.activeGameObject != null)
            {
                WeaponPivot.transform.position = Selection.transforms[0].position + WeaponPivot.transform.up * 1.2f;
                WeaponPivot.transform.SetParent(Selection.transforms[0]);
            }
            else
            {
                WeaponPivot.transform.position = SceneViewInstantiatePosition();
            }
            var WeaponPositionsParent = new GameObject("Weapons Reference Positions");
            WeaponPositionsParent.transform.position = WeaponPivot.transform.position;
            WeaponPositionsParent.transform.parent = WeaponPivot.transform;

            WeaponPivot.GetComponent<WeaponAimRotationCenter>().CreateWeaponPositionReference("Small Weapon Position Reference");
            WeaponPivot.GetComponent<WeaponAimRotationCenter>().WeaponPositionTransform[0].localPosition = new Vector3(0.212f, 0.065f, 0.361f);
            WeaponPivot.GetComponent<WeaponAimRotationCenter>().WeaponPositionTransform[0].localRotation = Quaternion.Euler(0, 11.383f, -94.913f);

            WeaponPivot.GetComponent<WeaponAimRotationCenter>().CreateWeaponPositionReference("Big Weapon Position Reference");
            WeaponPivot.GetComponent<WeaponAimRotationCenter>().WeaponPositionTransform[1].localPosition = new Vector3(0.138f, -0.013f, 0.24f);
            WeaponPivot.GetComponent<WeaponAimRotationCenter>().WeaponPositionTransform[1].localRotation = Quaternion.Euler(0, 11.383f, -94.913f);

            WeaponPivot.GetComponent<WeaponAimRotationCenter>().StoreLocalTransform();
        }



        //>>>> UTILITIES
        [MenuItem("GameObject/JUTPS Create/Utilities/Trigger Message", false, 0)]
        public static void CreateTriggerMessage()
        {
            var triggermessage = new GameObject("Trigger Message");
            Undo.RegisterCreatedObjectUndo(triggermessage, "triggermessage creation");
            triggermessage.AddComponent<BoxCollider>();
            triggermessage.GetComponent<BoxCollider>().isTrigger = true;
            triggermessage.AddComponent<Rigidbody>();
            triggermessage.GetComponent<Rigidbody>().isKinematic = true;
            triggermessage.AddComponent<UIMenssengerTrigger>();

            triggermessage.gameObject.layer = 2;
            triggermessage.transform.position = SceneViewInstantiatePosition();
        }

        //>>>> UTILITIES
        [MenuItem("GameObject/JUTPS Create/Camera/Create Camera State Trigger", false, 0)]
        public static void CreateCameraStateTrigger()
        {
            var camstatetrigger = new GameObject("Camera State Trigger");
            Undo.RegisterCreatedObjectUndo(camstatetrigger, "camstatetrigger creation");

            camstatetrigger.AddComponent<CameraStateTrigger>();

            camstatetrigger.gameObject.layer = 2;
            camstatetrigger.transform.position = SceneViewInstantiatePosition();
        }
        //>>>> UTILITIES
        [MenuItem("GameObject/JUTPS Create/AI/Create Waypoint Path", false, 0)]
        public static void CreateWaypointPath()
        {
            var waypointpath = new GameObject("Waypoint Path");
            Undo.RegisterCreatedObjectUndo(waypointpath, "waypointpath creation");
            waypointpath.AddComponent<JUTPS.AI.WaypointPath>().RefreshWaypoints();
            waypointpath.transform.position = SceneViewInstantiatePosition();
        }
        public static Vector3 SceneViewInstantiatePosition()
        {
            var view = SceneView.lastActiveSceneView.camera;
            if (view != null)
            {
                Vector3 pos = view.transform.position + view.transform.forward * 10;
                return pos;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}