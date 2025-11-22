using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using Unity.VisualScripting;

/// <summary>
/// A component copier - should filter out unwanted components and handle most errors
/// 
/// The code needs a refactor for readability
/// </summary>
public class SimpleComponentCopy : EditorWindow
{
    private GameObject sourceGameObject;
    private Dictionary<Component, bool> componentToCopyDictionary = new Dictionary<Component, bool>();
    private List<Component> sourceComponents = new List<Component>();
    private bool includeSerializedValues = true;
    private Type[] nonCopyableTypes = { typeof(Transform), typeof(MeshFilter), typeof(MeshRenderer) };

    [MenuItem("Tools/SimpleComponentCopy")]
    public static void ShowWindow()
    {
        GetWindow<SimpleComponentCopy>("SimpleComponentCopy");
    }


    void OnGUI()
    {
        DrawSourceGameObjectButton();
        DrawSourceGameObjectComponents(); // Ensure this is called here to draw toggles
        DrawCopyParametersToggle();
        DrawCopyToSelectedGameObjectsButton();

    }

    private void DrawCopyParametersToggle()
    {
        includeSerializedValues = EditorGUILayout.ToggleLeft("Include serialized values", includeSerializedValues);
    }

    private void DrawSourceGameObjectComponents()
    {
        if (componentToCopyDictionary != null && componentToCopyDictionary.Count > 0)
        {
            foreach (var componentEntry in componentToCopyDictionary.ToList()) // Use ToList to allow modification
            {
                // Draw a toggle for each component and update its value in the dictionary
                bool newValue = EditorGUILayout.Toggle(componentEntry.Key.GetType().Name, componentEntry.Value);
                componentToCopyDictionary[componentEntry.Key] = newValue;
            }
        }
    }

    private void DrawCopyToSelectedGameObjectsButton()
    {
        GUI.enabled = componentToCopyDictionary.Any(kvp => kvp.Value);

        if (GUILayout.Button("Copy to Selected GameObjects"))
        {
            CopyComponentsToSelectedGameObjects();
        }

        GUI.enabled = true; // Reset GUI state
    }

    private void DrawSourceGameObjectButton()
    {
        if (GUILayout.Button("Select Source GameObject"))
        {
            SetSourceGameObject();
            DrawSourceGameObjectComponents();
        }
    }

    /// <summary>
    /// Set the source object and get the components 
    /// </summary>
    private void SetSourceGameObject()
    {
        var selectedObjects = CopyUtils.GetSelectedObjects();
        if (selectedObjects.Count != 1)
        {
            SendMessageToUser("Select only one source object");
            return;
        }

        sourceGameObject = selectedObjects.FirstOrDefault(); // Get first object
        sourceComponents = CopyUtils.GetObjectComponents(sourceGameObject); // Get Components of object
        componentToCopyDictionary.Clear(); // Clear the dictionary

        foreach (var component in sourceComponents)
        {
            if (component != null && !nonCopyableTypes.Contains(component.GetType()))
            {
                componentToCopyDictionary[component] = false; // Initialize to false
            }
        }

        SendMessageToUser("Source Set: " + sourceGameObject.name);
    }

    private void CopyComponentsToSelectedGameObjects()
    {
        if (sourceGameObject == null) { Debug.Log("Null Source"); return; }
        foreach (var item in componentToCopyDictionary.Where(kvp => kvp.Value))
        {
            Debug.Log("Copying: " + item.Key.ToString());
            foreach (GameObject obj in CopyUtils.GetSelectedObjects())
            {
                Debug.Log("Copying: " + item.Key.ToString() + " To Object: " + obj.gameObject.name);
                CopyUtils.Copy(item.Key, obj, includeSerializedValues);
            }
        }
    }

    void SendMessageToUser(string message)
    {
        // For simplicity, could use Debug.Log for now
        Debug.Log(message);
    }
}

public static class CopyUtils
{
    public static void Copy(Component comp, GameObject obj, bool copyParameters)
    {
        Component clonedComponent = obj.AddComponent(comp.GetType());

        if (clonedComponent == null)
        {
            Debug.LogWarning($"Failed to add component {comp.GetType().Name} to {obj.name}");
            return;
        }

        if (copyParameters)
        {
            // EditorUtility.CopySerialized covers serialized fields/properties so the new component
            // mirrors the configured parameters on the source component.
            EditorUtility.CopySerialized(comp, clonedComponent);
        }
    }

    public static List<GameObject> GetSelectedObjects()
    {
        List<GameObject> selectedGameObjects = Selection.gameObjects.ToList(); // Ensures list is not null
        return selectedGameObjects;
    }

    public static List<Component> GetObjectComponents(GameObject obj)
    {
        List<Component> components = new List<Component>();
        components.AddRange(obj.GetComponents<Component>());
        return components;
    }
}