using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    private World _world;
    private int _chunkSize;

    private bool _started = false;
    private bool _checkChunks = true;

    public void Awake()
    {
        _world = GetComponent<World>();
        _chunkSize = _world.GetChunkSize();
    }

    private GameObject GetChunk(int x, int z) {
        return GameObject.Find("Chunk " + x + " " + z);
    }

    private void LoadChunk(int posX, int posZ, int offsetX, int offsetZ) {
        if (!GetChunk(posX + offsetX, posZ + offsetZ))
        {
            GameObject chunk = new GameObject("Chunk " + (posX + offsetX) + " " + (posZ + offsetZ));
            chunk.transform.parent = gameObject.transform;
            chunk.transform.position = new Vector3((posX + offsetX) * _chunkSize, 0, (posZ + offsetZ) * _chunkSize);

            RenderChunk renderChunk = chunk.AddComponent<RenderChunk>();
            renderChunk.Render(posX + offsetX, posZ + offsetZ);
        }
    }

    private void UnloadChunk(int posX, int posZ, int offsetX, int offsetZ)
    {
        GameObject chunk = GetChunk(posX + offsetX, posZ + offsetZ);
        if (chunk)
        {
            Destroy(chunk);
        }
    }

    private IEnumerator UnloadChunks(int dist)
    {
        while (_checkChunks)
        {
            Transform player = _world.Player.transform;
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++) {
                Transform chunk = transform.GetChild(i);
                
                if (Vector3.Distance(player.position, chunk.position) >= dist) {
                    Destroy(chunk.gameObject);
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator LoadChunks() {
        while (_checkChunks)
        {
            Vector3 position = _world.Player.transform.position;
            int posX = (int)position.x / _chunkSize;
            int posZ = (int)position.z / _chunkSize;

            LoadChunk(posX, posZ, 0, 0);
            LoadChunk(posX, posZ, 1, 0);
            LoadChunk(posX, posZ, 0, 1);
            LoadChunk(posX, posZ, -1, 0);
            LoadChunk(posX, posZ, 0, -1);
            LoadChunk(posX, posZ, 1, 1);
            LoadChunk(posX, posZ, -1, -1);
            LoadChunk(posX, posZ, -1, 1);
            LoadChunk(posX, posZ, 1, -1);

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void Update()
    {
        if (!_started) {
            _started = true;
            StartCoroutine(LoadChunks());
            StartCoroutine(UnloadChunks(_chunkSize * 3));
        }
    }
}
