using System.IO;
using UnityEditor;
using UnityEngine;


public class SpriteImportPostProcessor : AssetPostprocessor {

	private void OnPreprocessTexture() {
		if (!assetPath.Contains("/Sprites"))
			return;

		TextureImporter importer = (TextureImporter)assetImporter;

		importer.textureType = TextureImporterType.Sprite;
		importer.spriteImportMode = SpriteImportMode.Single;
		importer.alphaIsTransparency = true;
		importer.mipmapEnabled = false;
		importer.filterMode = FilterMode.Point;
		importer.textureCompression = TextureImporterCompression.Uncompressed;

		string fileName = Path.GetFileNameWithoutExtension(importer.assetPath);

		Debug.Log($"Imported icon \"{fileName}\" at: {importer.assetPath}.");
	}

}