using System;
using UnityEngine;

public static class Chunk
{
	public static void LoopThroughTheBlocks(ChunkData chunkData, Action<int, int, int> actionToPerform)
	{
		for (int i = 0; i < chunkData.blocks.Length; i++)
		{
			var position = GetPositionFromIndex(chunkData, i);
			actionToPerform(position.x, position.y, position.z);
		}
	}

	private static Vector3Int GetPositionFromIndex(ChunkData chunkData, int index)
	{
		int x = index % chunkData.chunkSize;
		int y = (index / chunkData.chunkSize) % chunkData.chunkHeight;
		int z = index / (chunkData.chunkSize * chunkData.chunkHeight);
		return new Vector3Int(x, y, z);
	}

	// In chunk coordinate system
	private static bool InRange(ChunkData chunkData, int axisCoordinate)
	{
		if (axisCoordinate < 0 || axisCoordinate >= chunkData.chunkSize)
			return false;

		return true;
	}

	// In chunk coordinate system
	private static bool InRangeHeight(ChunkData chunkData, int yCoordinate)
	{
		if (yCoordinate < 0 || yCoordinate >= chunkData.chunkHeight)
			return false;

		return true;
	}

	// In chunk coordinate system
	private static bool PositionInRange(ChunkData chunkData, Vector3Int localPosition)
	{
		return InRange(chunkData, localPosition.x) && InRangeHeight(chunkData, localPosition.y) && InRange(chunkData, localPosition.z);
	}

	public static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, Vector3Int chunkCoordinates)
	{
		if (!PositionInRange(chunkData, chunkCoordinates))
			return chunkData.worldReference.GetBlockFromChunkCoordinates(chunkData, chunkData.worldPosition.x + chunkCoordinates.x, chunkData.worldPosition.y + chunkCoordinates.y, chunkData.worldPosition.z + chunkCoordinates.z);

		int index = GetIndexFromPosition(chunkData, chunkCoordinates.x, chunkCoordinates.y, chunkCoordinates.z);
		return chunkData.blocks[index];
	}

	public static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
	{
		return GetBlockFromChunkCoordinates(chunkData, new Vector3Int(x, y, z));
	}

	public static void SetBlock(ChunkData chunkData, Vector3Int localPosition, BlockType block)
	{
		if (!PositionInRange(chunkData, localPosition))
			throw new Exception("Need to ask World for appropriate chunk");

		int index = GetIndexFromPosition(chunkData, localPosition.x, localPosition.y, localPosition.z);
		chunkData.blocks[index] = block;
	}

	public static Vector3Int ChunkPositionFromBlockCoords(World world, int x, int y, int z)
	{
		Vector3Int pos = new Vector3Int
		{
			x = Mathf.FloorToInt(x / (float)world.chunkSize) * world.chunkSize,
			y = Mathf.FloorToInt(y / (float)world.chunkHeight) * world.chunkHeight,
			z = Mathf.FloorToInt(z / (float)world.chunkSize) * world.chunkSize,
		};
		return pos;
	}

	private static int GetIndexFromPosition(ChunkData chunkData, int x, int y, int z)
	{
		return x + chunkData.chunkSize * y + chunkData.chunkSize * chunkData.chunkHeight * z;
	}

	public static Vector3Int GetBlockInChunkCoordinates(ChunkData chunkData, Vector3Int pos)
	{
		return new Vector3Int
		{
			x = pos.x - chunkData.worldPosition.x,
			y = pos.y - chunkData.worldPosition.y,
			z = pos.z - chunkData.worldPosition.z,
		};
	}

	public static MeshData GetChunkMeshData(ChunkData chunkData)
	{
		MeshData meshData = new MeshData(true);

		LoopThroughTheBlocks(chunkData, 
			(x, y, z) => 
			meshData = BlockHelper.GetMeshData(chunkData, x, y, z, meshData, chunkData.blocks[GetIndexFromPosition(chunkData, x, y, z)]));

		return meshData;
	}
}