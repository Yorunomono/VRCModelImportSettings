using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VRCModelImportSettings : AssetPostprocessor
{
    //------------------------------------------------------------
    // ver1.00
    //
    // FBXをインポートした際に、
    // [Read/Write Enabled]に自動でチェックが入ります。、
    // また、[BlendShapeNormals]が[None]に変更されるスクリプトです。
    //
    // テクスチャをインポートした際に、
    // [Streaming Mipmap]に自動でチェックが入ります。
    // テクスチャ名に「Normal」が含まれる場合、
    // ダイアログが表示され、「はい」を押すと、自動でTexture Typeを「Normal」にします。
    //
    //------------------------------------------------------------

    private bool _showConfirmDialog = false;
    private TextureImporterType _newTextureType;


    void OnPostprocessModel(GameObject model)
    {
        // FBXファイルをインポートした後に呼ばれるコールバック

        // [Import Settings]の[Model]カテゴリを取得
        ModelImporter modelImporter = assetImporter as ModelImporter;

        // [Read/Write Enabled]にチェックを入れる
        modelImporter.isReadable = true;

        // [Blend Shape Normals]を[None]に変更
        modelImporter.importBlendShapeNormals = ModelImporterNormals.None;

        // インポート設定を適用
        modelImporter.SaveAndReimport();
    }

    void OnPreprocessTexture()
    {
        // インポートされたテクスチャの名前に "Normal" が含まれている場合、Texture Type を Normal Map に変更する
        if (assetPath.Contains("Normal"))
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.NormalMap;

            // ダイアログを表示する
            string fileName = System.IO.Path.GetFileName(assetPath);
            if (EditorUtility.DisplayDialog("Texture Type変更の確認", fileName + "をNormal Mapに変更しますか？", "はい", "いいえ"))
            {
                textureImporter.textureType = TextureImporterType.NormalMap;
            }
            else
            {
                textureImporter.textureType = TextureImporterType.Default;
            }
        }

        // [Streaming Mipmaps] を有効にする
        TextureImporter importer = (TextureImporter)assetImporter;
        importer.streamingMipmaps = true;
    }

}
