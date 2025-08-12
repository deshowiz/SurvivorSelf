using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Rendering;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponPositions", menuName = "Scriptable Objects/WeaponPositions")]
public class WeaponPositions : ScriptableObject
{
    #if UNITY_EDITOR
    [SerializeField]
    public int testPositions = 1;
    public void TestNewPositions()
    {
        Player player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogError("Player missing from scene");
            return;
        }

        Weapon[] weapons = FindObjectsByType<Weapon>(FindObjectsSortMode.None);

        if (weapons.Length == 0)
        {
            Debug.LogError("Weapons missing from scene");
            return;
        }

        SetNewPositions(weapons.ToList());
    }
    #endif
    [SerializeField]
    List<WeaponPositionList> _weaponPositions = new List<WeaponPositionList>();

    [Serializable]
    private struct WeaponPositionList
    {
        [SerializeField]
        public List<Vector2> _positions;
    }

    public void SetNewPositions(List<Weapon> equippedWeapons)
    {
        int numWeapons = equippedWeapons.Count - 1;

        if (numWeapons >= _weaponPositions.Count)
        {
            Debug.LogError("More weapons equipped than weapon position list supports");
            return;
        }
        List<Vector2> newPositions = _weaponPositions[numWeapons]._positions;
        bool isSingleWeapon = newPositions.Count == 1;
        for (int i = 0; i < newPositions.Count; i++)
        {
            Weapon currentWeapon = equippedWeapons[i];
            currentWeapon.transform.localPosition = newPositions[i];
            currentWeapon._onlyWeapon = isSingleWeapon;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WeaponPositions))]
public class WeaponPositionsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WeaponPositions positionsScript = (WeaponPositions)target;
        if (GUILayout.Button("Rebuild Positions"))
        {
            positionsScript.TestNewPositions();
        }
        base.OnInspectorGUI();
    }
}

#endif