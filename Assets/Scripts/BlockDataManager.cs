using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDataManager : MonoBehaviour
{
	public static float TextureOffset = 0.001f;
	public static float tilesSizeX, tilesSizeY;
	public static Dictionary<BlockType, TextureData> blockTextureDataDictionary = new Dictionary<BlockType, TextureData>();
	
	public BlockDataSO textureData;

	private void Awake()
	{
		foreach(var item in textureData.textureDataList)
		{
			if(blockTextureDataDictionary.ContainsKey(item.blockType) == false)
			{
				blockTextureDataDictionary.Add(item.blockType, item);
			}
		}

		tilesSizeX = textureData.textureSizeX;
		tilesSizeY = textureData.textureSizeY;
	}
}
