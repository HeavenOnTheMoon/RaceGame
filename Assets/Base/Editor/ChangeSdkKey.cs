using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary> 纯A包AppLovin SdkKey加密工具 </summary>
public class ChangeSdkKey : MonoBehaviour
{
    [MenuItem("Tools/加密SdkKey")]
    static void ShowSuperBuildWindow()
    {
        string sdkKey = "Fs-cUqJfRU6DI-3nHAtCUubM2g2mHMT4kl_2_v9IyohMfXicNfA0eEwvSJ6gvrtpXtmu2TpTdL-QrLAMqwaXPS";
        string encryptSdkKey = ChangeSdkKey.EncryptDES(sdkKey);
        AppLovinSettings.Instance.SdkKey = encryptSdkKey;
        GameObject.FindObjectOfType<A_ADManager>().SdkKey = encryptSdkKey;

        // 添加自动保存场景的代码
        SaveCurrentScene();
    }

    // 保存当前打开的场景
    private static void SaveCurrentScene()
    {
        // 获取当前打开的场景
        var currentScene = EditorSceneManager.GetActiveScene();

        // 保存场景
        if (EditorSceneManager.SaveScene(currentScene))
        {
            Debug.Log($"<b><color=yellow>基于包名 {Application.identifier} 的加密key已生成，场景 {currentScene.name} 已保存</color></b>");
        }
        else
        {
            Debug.LogError("场景保存失败！可能是场景未命名或被锁定");
        }
    }

    private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    public static string EncryptDES(string encryptString)
    {
        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(Application.identifier.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            cStream.Close();
            return Convert.ToBase64String(mStream.ToArray());
        }
        catch
        {
            //Debug.LogError("StringEncrypt/EncryptDES()/ Encrypt error!");
            return encryptString;
        }
    }
}
