using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FreeCamera : MonoBehaviour
{
    public float m_MoveSpeed = 0f;
    public float m_RotateSpeed = 0f;
    public KeyCode m_ForwardButton = KeyCode.W;
    public KeyCode m_BackwardButton = KeyCode.S;
    public KeyCode m_RightButton = KeyCode.D;
    public KeyCode m_LeftButton = KeyCode.A;
    public KeyCode m_UpButton = KeyCode.Q;
    public KeyCode m_DownButton = KeyCode.E;
	
	public GameObject[] m_SketchObjects;
	public PencilContourEffect m_PencilContourEffect;
	private int m_OutlineEffect = 0;
	private float m_SketchTile = 5f;
	private float m_OutlineWidth = 0.005f;
	private Rect[] m_GUIRects = new Rect[2];

	void Start ()
	{
		m_GUIRects[0] = new Rect (110, 110, 100, 25);
		m_GUIRects[1] = new Rect (110, 140, 100, 25);
	}
    void Update ()
    {
        // translation
        {
            Vector3 dir = Vector3.zero;
			Move (m_ForwardButton, ref dir, transform.forward);
			Move (m_BackwardButton, ref dir, -transform.forward);
			Move (m_RightButton, ref dir, transform.right);
			Move (m_LeftButton, ref dir, -transform.right);
			Move (m_UpButton, ref dir, transform.up);
			Move (m_DownButton, ref dir, -transform.up);
			transform.position += dir * m_MoveSpeed * Time.deltaTime;
        }
        // rotation
        {
            if (Input.GetMouseButton (0) && !MouseOnGUI ())
            {
                Vector3 eulerAngles = transform.eulerAngles;
				eulerAngles.x += -Input.GetAxis ("Mouse Y") * 359f * m_RotateSpeed;
				eulerAngles.y += Input.GetAxis ("Mouse X") * 359f * m_RotateSpeed;
                transform.eulerAngles = eulerAngles;
            }
        }
    }
    void Move (KeyCode key, ref Vector3 moveTo, Vector3 dir)
    {
        if (Input.GetKey (key))
			moveTo = dir;
    }
	void OnGUI ()
	{
		GUI.Box (new Rect (10, 10, 180, 25), "NPR Sketch Effect Demo");
		
		string[] names = { "Outline", "Pencil Contour" };
		m_OutlineEffect = GUI.SelectionGrid (new Rect (10, 40, 100, 50), m_OutlineEffect, names, 1);
		GUI.Box (new Rect (10, 100, 90, 25), "Sketch Tile");
		m_SketchTile = GUI.HorizontalSlider (m_GUIRects[0], m_SketchTile, 1f, 10f);
		if (m_OutlineEffect == 0)
		{
			GUI.Box (new Rect (10, 130, 90, 25), "Outline Width");
			m_OutlineWidth = GUI.HorizontalSlider (m_GUIRects[1], m_OutlineWidth, 0f, 0.02f);
		}
		Apply ();
	}
	void Apply ()
	{
		float outlineWidth = 0f;
		if (m_OutlineEffect == 0)
		{
			m_PencilContourEffect.enabled = false;
			outlineWidth = m_OutlineWidth;
		}
		else
		{
			m_PencilContourEffect.enabled = true;
			outlineWidth = 0f;
		}
		for (int i = 0; i < m_SketchObjects.Length; i++)
		{
			Renderer rd = m_SketchObjects[i].GetComponent<Renderer>();
			rd.material.SetFloat ("_OutlineWidth", outlineWidth);
			rd.material.SetFloat ("_Tile", m_SketchTile);
		}
	}
	bool MouseOnGUI ()
	{
		for (int i = 0; i < m_GUIRects.Length; i++)
		{
			if (m_GUIRects[i].Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
				return true;
		}
		return false;
	}
}
