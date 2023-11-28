using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities {

    public static T ParseEnum<T>(string value, bool ignoreCase = true) {
        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

    public static T GetOrAddComponent<T>(GameObject obj) where T : UnityEngine.Component {
        T component = obj.GetComponent<T>() ?? obj.AddComponent<T>();
        return component;
    }

    public static T FindChild<T>(GameObject obj, string name = null, bool recursive = false) where T : UnityEngine.Object {
        if (obj == null) return null;
        if (recursive == false) {
            Transform transform = obj.transform.Find(name);
            if (transform != null) return transform.GetComponent<T>();
        }
        else {
            foreach (T component in obj.GetComponentsInChildren<T>()) {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        return null;
    }
    public static GameObject FindChild(GameObject obj, string name = null, bool recursive = false) {
        Transform transform = FindChild<Transform>(obj, name, recursive);
        if (transform != null) return transform.gameObject;
        return null;
    }

    public static Vector2 GetRandomSpawnPosition(Vector2 position, float minSpawnDistance = 20f, float maxSpawnDistance = 25f) {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        float distance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);
        float xDistance = Mathf.Cos(angle) * distance;
        float yDistance = Mathf.Sin(angle) * distance;

        Vector2 spawnPosition = position + new Vector2(xDistance, yDistance);

        return spawnPosition;
    }

    public static Quaternion GetRotationLoockAt(Vector3 position) {
        float angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}