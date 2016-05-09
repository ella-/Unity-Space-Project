using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour
{
	public GameObject[] m_HatchObjects;
	public Light m_DirLight;
	private Rect[] m_GUIRects = new Rect[4];
	private Shader[] m_Shaders = new Shader[2];
	private int m_HatchType = 0;
	private int m_HatchColor = 0;
	private int m_OutlineColor = 0;
	private float m_OutlineWidth = 0.005f;
	private float m_HatchDensity = 4f;
	private bool m_RimInverse = false;
	
	public bool MouseOnGUI ()
	{
		for (int i = 0; i < m_GUIRects.Length; i++)
		{
			if (m_GUIRects[i].Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
				return true;
		}
		return false;
	}
	void Start ()
	{
		m_GUIRects[0] = new Rect (120, 200, 80, 25);
		m_GUIRects[1] = new Rect (100, 230, 100, 25);
		m_GUIRects[2] = new Rect (100, 230, 100, 25);
		m_GUIRects[3] = new Rect (100, 260, 100, 25);
		m_Shaders[0] = Shader.Find ("NPR Hatch Effect/Hatch Simple");
		m_Shaders[1] = Shader.Find ("NPR Hatch Effect/Hatch Complicated");
	}
	void Update ()
	{
		Color hatchColor = new Color (0f, 0f, 0f, 0f);
		if (0 == m_HatchColor)  hatchColor = new Color (0f, 0f, 0f, 1f);
		if (1 == m_HatchColor)  hatchColor = new Color (72f/255f, 72f/255f, 164f/255f, 1f);
		if (2 == m_HatchColor)  hatchColor = new Color (41f/255f, 41f/255f, 202f/255f, 1f);
		if (3 == m_HatchColor)  hatchColor = new Color (90f/255f, 120f/255f, 111f/255f, 1f);
		
		Color outlineColor = new Color (0f, 0f, 0f, 0f);
		if (0 == m_OutlineColor)  outlineColor = new Color (0f, 0f, 0f, 1f);
		if (1 == m_OutlineColor)  outlineColor = new Color (1f, 0f, 0f, 1f);
		if (2 == m_OutlineColor)  outlineColor = new Color (1f, 1f, 0f, 1f);
		if (3 == m_OutlineColor)  outlineColor = new Color (0f, 0.5f, 1f, 1f);
		
		for (int i = 0; i < m_HatchObjects.Length; i++)
		{
			MeshRenderer rd = m_HatchObjects[i].GetComponent<MeshRenderer>();
			rd.material.SetFloat ("_HatchDensity", m_HatchDensity);
			rd.material.SetColor ("_HatchColor", hatchColor);
			rd.material.SetColor ("_OutlineColor", outlineColor);
			rd.material.SetFloat ("_OutlineWidth", m_OutlineWidth);
			if (m_RimInverse)
				rd.material.EnableKeyword ("NHE_INVERSE_RIM");
			else
				rd.material.DisableKeyword ("NHE_INVERSE_RIM");
		}
	}
	void OnGUI ()
	{
		GUI.Box (new Rect (10, 10, 180, 25), "NPR Hatch Effect Demo");
		
		string[] names1 = { "Simple", "Complicated" };
		m_HatchType = GUI.SelectionGrid (new Rect (10, 40, 180, 25), m_HatchType, names1, 2);
		
		GUI.Box (new Rect (10, 70, 100, 25), "Hatch Color");
		string[] names2 = { "1", "2", "3", "4" };
		m_HatchColor = GUI.SelectionGrid (new Rect (10, 100, 180, 25), m_HatchColor, names2, 4);
		
		GUI.Box (new Rect (10, 130, 100, 25), "Outline Color");
		string[] names3 = { "1", "2", "3", "4" };
		m_OutlineColor = GUI.SelectionGrid (new Rect (10, 160, 180, 25), m_OutlineColor, names3, 4);
		
		GUI.Box (new Rect (10, 190, 100, 25), "Outline Width");
		m_OutlineWidth = GUI.HorizontalSlider (m_GUIRects[0], m_OutlineWidth, 0f, 0.02f);
		
		if (0 == m_HatchType)
		{
			GUI.Box (new Rect (10, 220, 80, 25), "Density");
			m_HatchDensity = GUI.HorizontalSlider (m_GUIRects[1], m_HatchDensity, 1f, 8f);
		}
		if (1 == m_HatchType)
		{
			m_RimInverse = GUI.Toggle (new Rect (10, 220, 100, 25), m_RimInverse, "Inverse Rim");
		}
		Apply ();
	}
	void Apply ()
	{
		for (int i = 0; i < m_HatchObjects.Length; i++)
		{
			MeshRenderer rd = m_HatchObjects[i].GetComponent<MeshRenderer>();
			if (0 == m_HatchType)
			{
				rd.material.shader = m_Shaders[0];
				m_DirLight.intensity = 1f;
			}
			if (1 == m_HatchType)
			{
				rd.material.shader = m_Shaders[1];
				m_DirLight.intensity = 0.4f;
			}
		}
	}
}
