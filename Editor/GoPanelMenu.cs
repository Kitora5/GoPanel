#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using VRC.SDK3.Avatars.Components;

public class GoPanelMenu : EditorWindow
{
    const string GoPanel_Path = "Packages/gopanel/Runtime/GoPanel";

    private GameObject GoPanelVRCFPrefab;
    private GameObject GoPanelMAPrefab;

    private Texture2D headerImage;
    private GameObject avatarTarget;
    private string errorLabel = "";

    /**
    *  Load the GoPanel ressources
    */
    private void OnEnable()
    {
        headerImage = AssetDatabase.LoadAssetAtPath<Texture2D>(GoPanel_Path + "/Icons/GoPanel Icon.png");
        GoPanelVRCFPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GoPanel_Path + "/Prefab/GoGoLoco Panel (VRCFury).prefab");
        GoPanelMAPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GoPanel_Path + "/Prefab/GoGoLoco Panel (Modular Avatar).prefab");
        avatarTarget = Selection.activeGameObject;
    }

    /**
    *  Load the GoPanel Prefab Window
    */
    [MenuItem("Tools/GoGoLoco/GoPanel")]
    public static void ShowWindow()
    {
        GetWindow<GoPanelMenu>("GoPanel Prefabs");
    }

    /**
    *  Flow of the GoPanel Prefab Window
    */
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        // GoPanel Logo
        GUILayout.Label(headerImage, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(headerImage.height));
        // GoPanel Title
        GUILayout.Label("GoPanel", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(headerImage.height));
        GUILayout.EndHorizontal();

        // Help Box
        GUILayout.Label("Select your avatar in the hierarchy");
        // Avatar Selector 
        avatarTarget = EditorGUILayout.ObjectField(avatarTarget, typeof(GameObject), true) as GameObject;

        if (avatarTarget == null)
        {
            errorLabel = "Error: No object selected in the Hierarchy.";
            GUI.color = Color.red;
            GUILayout.Label(errorLabel);
            GUI.color = Color.white;
        }
        else if (avatarTarget.GetComponent<VRCAvatarDescriptor>() == null)
        {
            errorLabel = "Error: Selected object isn't an avatar (Doesn't have an AvatarDescriptor Component).";
            GUI.color = Color.red;
            GUILayout.Label(errorLabel);
            GUI.color = Color.white;
        }
        else
        {
            errorLabel = "";
        }

        // Disable buttons if wrong avatar selected
        GUI.enabled = errorLabel == "";
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Label("VRCFury Prefabs");
        if (GUILayout.Button("Add GoPanel"))
        {
            //childObject.transform.IsChildOf(parentObject.transform)
            GameObject instantiatedPrefab = PrefabUtility.InstantiatePrefab(GoPanelVRCFPrefab) as GameObject;
            instantiatedPrefab.transform.SetParent(avatarTarget.transform);
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Modular Avatar Prefabs");
        if (GUILayout.Button("Add GoPanel"))
        {
            GameObject instantiatedPrefab = PrefabUtility.InstantiatePrefab(GoPanelMAPrefab) as GameObject;
            instantiatedPrefab.transform.SetParent(avatarTarget.transform);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        
        GUI.enabled = true;
    }
}
#endif