#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(World))]
public class ObjectBuilderEditor : Editor
{
    private World _world;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _world = (World)target;

        if (GUILayout.Button("Render Chunk"))
        {
            GameObject chunk = new GameObject("Chunk");
            RenderChunk renderChunk = chunk.AddComponent<RenderChunk>();
            renderChunk.Render(_world.ChunkX, _world.ChunkZ);
        }

        if (GUILayout.Button("Spawn Player")){
            SpawnPlayer(true);
        }
    }

    private void SpawnPlayer(bool checkDupe)
    {
        if (_world.Player)
        {
            Destroy(_world.Player);
            SpawnPlayer();
        }
        else
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        // Create and spawn the player.
        GameObject playerPrefab = (GameObject)Resources.Load("Prefabs/Player");
        _world.Player = Instantiate(playerPrefab, new Vector3(10, 21, 10), Quaternion.identity);

        InitPlayerCamera();
    }

    private void InitPlayerCamera()
    {
        // Create and attach the camera component.
        GameObject camObject = new GameObject("Camera");
        Camera camera = camObject.AddComponent<Camera>();
        Transform cameraTransform = camera.transform;
        Vector3 offset = _world.Player.transform.position;
        cameraTransform.Translate(offset + new Vector3(0, 10, 0));
        cameraTransform.Rotate(new Vector3(90, 0, 0));
        camObject.transform.parent = _world.Player.transform;
    }
}
#endif