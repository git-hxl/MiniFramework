using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class TxtEncrypt : MonoBehaviour {
	public string Txt;
	public string Key;
	public string IV;
	// Use this for initialization
	void Start () {
		string encryptTxt = SecurityUtil.DesEncrypt(Txt,Key,IV);
		Debug.Log("des加密后："+encryptTxt);
		string decryptTxt = SecurityUtil.DesDecrypt(encryptTxt,Key,IV);
		Debug.Log("des解密后："+decryptTxt);

		string md5 = SecurityUtil.MD5Encrypt(Txt);
		Debug.Log("MD5加密后："+md5);

		string publicKey;
		string privateKey;
		SecurityUtil.GenerateRsaKey(out privateKey,out publicKey);

		string rsaTxt = SecurityUtil.RsaEncrypt(Txt,publicKey);
		Debug.Log("rsa加密后："+rsaTxt);
		string rsadecryptTxt = SecurityUtil.RsaDecrypt(rsaTxt,privateKey);
		Debug.Log("rsa解密后："+rsadecryptTxt);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
