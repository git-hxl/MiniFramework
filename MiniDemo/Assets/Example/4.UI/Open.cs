using System.Collections;
using System.Collections.Generic;
using MiniFramework;
using UnityEngine;
using UnityEngine.UI;

public class Open : MonoBehaviour {
	public string panelName;
	// Use this for initialization
	void Start () {
		Button bt =GetComponent<Button>();
		bt.onClick.AddListener(()=>{
			UIManager.Instance.Open(panelName);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
