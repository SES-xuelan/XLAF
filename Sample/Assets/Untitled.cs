 using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XLAF.Public;

public class Untitled : MonoBehaviour
{

	// Use this for initialization
	IEnumerator Start ()
	{
		yield return new WaitForSeconds (5);
		Application.targetFrameRate = 60;
		Image i = ModUIUtils.GetChild<Image> (transform, "Image");
		float curr_x = i.transform.position.x;
		float curr_y = i.transform.position.y;
		i.rectTransform.position = new Vector3 (curr_x + Screen.width, curr_y);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}



}
