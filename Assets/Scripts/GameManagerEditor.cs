using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager gameManagerTarget = (GameManager)target;
        if (GUILayout.Button("Play"))
        {
            gameManagerTarget.StartGame();
        }

    }
}
