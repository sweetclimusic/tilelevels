using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SweetPrefabPainter
{
	//Does all the drawing to the scene view.
	public class PrefabPainter : Editor {
		public static PrefabPainter _instance;
		public Sprite mouseCurser;
		public ParticleSystem cursor;

		public SelectedTool {
			get{
				EditorPrefer.GetString(value);
			}
			set{
				switch (value)
				{
					// on choice of button choosen set it as opposite.
					case "End":
					case "Paint":
					EditorPrefer.SetBool("SweetPrefabPainting", 
						!EditorPrefer.GetBool("SweetPrefabPainting")
					)
					EditorPrefer.SetBool("SweetPrefabPaintingEnd",!EditorPrefer.GetBool("SweetPrefabPaintingEnd"));
					default:
				}
			}
		}

		[MenuItem ("SweetPrefabPainter/PrefabPainterBrush")]
		private static void CreatePrefabPainterBrush(){
			//mouseCurser = new Sprite();
			mouseCurser = EditorGUIUtility.Load("cursor_pointerFlat.png") as Sprite;
			EditorPrefer.SetBool("SweetPrefabPainting",true);
			EditorPrefer.SetBool("SweetPrefabPaintingEnd",false);
		}
		/// <summary>
		/// This function is called when the object becomes enabled and active.
		/// </summary>
		void OnEnable()
		{
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			SceneView.onSceneGUIDelegate += OnSceneGUI;
			//Editor update function, runs at 30fps 
			EditorApplication.update -= DrawCursorOnScene;
			EditorApplication.update += DrawCursorOnScene;
		}
		/// <summary>
		/// This function is called when the behaviour becomes disabled or inactive.
		/// </summary>
		void OnDisable()
		{
			//deregister delegate.
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			EditorApplication.update -= DrawCursorOnScene;
		}

		///<summary>
		/// redraws the mouseSprite or selected prefab.
		/// </summary>
		static void DrawCursorOnScene(){
			//This is the handle.
			Handle.Color = new Color (0.3,0.3,0.3,1);

		}

		//use the EditorPreference to store the selected options in the view.

		///<summary>
		/// draws the Menu for the PrefabPainter
		/// </summary>
		static void DrawToolsMenu(){
			//properties
			float width;
			float height
			float count;

			//Rendering loop(?) of the handle, require a EndGUI when done
			Handle.BeginGUI();
			GUILayout.BeginVertical("prefabpainterbox");
			string[] labels = new string[]{"End","Paint"}
			SelectedTool = GUILayout.SelectedGrid(
				SelectedTool, // get current Selected...
				labels,
				2,
				EditorStyle.toolbarButton,
				GUILayout.Width(20)
			);
			//spacing and sliders
			GUILayout.Label("Scale width:");
			width = GUILayout.HorizontalSlider(width,1.0f,10.0f);
			GUILayout.Space(5);
			GUILayout.Label("Scale height:");
			height = GUILayout.HorizontalSlider(height,1.0f,10.0f);
			GUILayout.Space(5);
			count = GUILayout.HorizontalSlider(width,1.0f,10.0f);
			//finished layout group
			GUILayout.EndVertical();
			
			Handle.EndGUI();

			//send the properties to the controller?
		}
		
	}
}