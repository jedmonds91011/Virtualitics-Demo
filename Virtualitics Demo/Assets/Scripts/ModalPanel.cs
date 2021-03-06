﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ModalPanel : MonoBehaviour {

	public Text question;
	public Image iconImage;
	public Button yesButton;
	public Button noButton;
	public Button cancelButton;
	public GameObject modalPanelObject;

	private static ModalPanel modalPanel;

	public static ModalPanel instance() {
		if (!modalPanel) {
			modalPanel = FindObjectOfType (typeof(ModalPanel)) as ModalPanel;
			if (!modalPanel) {
				Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
			}
		}
		return modalPanel;
	}

	public void choice(string question, UnityAction yesEvent, UnityAction noEvent, UnityAction cancelEvent){
		modalPanelObject.SetActive (true);

		yesButton.onClick.RemoveAllListeners ();
		yesButton.onClick.AddListener (yesEvent);
		yesButton.onClick.AddListener (closePanel);

		noButton.onClick.RemoveAllListeners ();
		noButton.onClick.AddListener (noEvent);
		noButton.onClick.AddListener (closePanel);

		cancelButton.onClick.RemoveAllListeners ();
		cancelButton.onClick.AddListener (cancelEvent);
		cancelButton.onClick.AddListener (closePanel);

		this.question.text = question;
		this.iconImage.gameObject.SetActive (false);

		yesButton.gameObject.SetActive (true);
		noButton.gameObject.SetActive (true);
		cancelButton.gameObject.SetActive (true);
	}

	void closePanel(){
		modalPanelObject.SetActive (false);
	}
}
