using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SweetPrefabPainter {
	//Does all the drawing to the scene view.
	public class PrefabPainter : Editor {
		//as we have public methods, make variables public.
		public static PrefabPainter _prefabPainterInstance;
		public static Sprite _pointer;
		public static GameObject _cursor;

		static bool repaint = false;

		public static bool RepaintPrefabPainter {
			get {
				return repaint;
			}set {
				repaint = value;
			}
		}

		[MenuItem ("SweetPrefabPainter/PrefabPainterBrush")]
		private static void CreatePrefabPainterBrush(){
			if (_prefabPainterInstance == null) {
				//seems Editor is a scriptable object, must create a instance of it and not just
				//use new keyword.
				_prefabPainterInstance = CreateInstance<PrefabPainter>();
			}
			_pointer = EditorGUIUtility.Load("cursor_pointerFlat.png") as Sprite;
			_cursor = EditorGUIUtility.Load("cursor_pointerFlat.png") as GameObject;

			PrefabPainterModel.SelectedTool = 2;

		}
		/// <summary>
		/// Constructor
		/// </summary>
		static PrefabPainter()
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
		void OnDestroy()
		{
			//deregister delegate.
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			EditorApplication.update -= DrawCursorOnScene;
		}

		static void OnSceneGUI( SceneView sceneView){
			//update position on screen
			//check if we can use our tool
			//draw handles tool menu
			DrawToolsMenu(sceneView.position);
			//draw the mouse cursor or the painter tool.
			DrawCursorOnScene();

		}

		///<summary>
		/// redraws the mouseSprite or selected prefab.
		/// </summary>
		static void DrawCursorOnScene(){
			//This is the handle.
			Handles.color = new Color (0.3f,0.3f,0.3f,1f);

		}

		//use the EditorPreference to store the selected options in the view.

		///<summary>
		/// draws the Menu for the PrefabPainter
		/// params Rect position, sceneview x,y positon from onSceneGui
		/// </summary>
		static void DrawToolsMenu(Rect position){

			//Rendering loop(?) of the handle, require a EndGUI when done
			Handles.BeginGUI();
			//x,y ,width, height, 
			GUILayout.BeginArea (new Rect(10,position.height - 200,110,180),EditorStyles.helpBox);
			GUILayout.BeginVertical();
			string[] labels = new string[]{"End","Paint","Delete"};
			PrefabPainterModel.SelectedTool = GUILayout.SelectionGrid(
				PrefabPainterModel.SelectedTool, // get current Selected...
				labels,
				1,
				EditorStyles.miniButton,
				GUILayout.Width(100)
			);
			//grab editorPrefs or default to 1
			float height  = EditorPrefs.GetFloat("SweetPrefabYScale",1.0f);
			float width = EditorPrefs.GetFloat("SweetPrefabXScale",1.0f);
			float count = (float)EditorPrefs.GetInt("SweetPrefabCount",1); 

			//spacing and sliders
			GUILayout.Label("Scale width:");
			width = GUILayout.HorizontalSlider(width,1.0f,10.0f,GUILayout.Width (100));
			GUILayout.Space(5);
			GUILayout.Label("Scale height:");
			height = GUILayout.HorizontalSlider(height,1.0f,10.0f,GUILayout.Width (100));
			GUILayout.Space(5);
			GUILayout.Label("Scale count:");
			count = GUILayout.HorizontalSlider(count,1.0f,10.0f,GUILayout.Width (100));
			//finished layout group
			GUILayout.EndVertical();
			GUILayout.EndArea ();
			Handles.EndGUI();
			//SelectedScale a vector to record selected options in EditorPrefs
			PrefabPainterModel.SelectedScale = new Vector3 (width, height, count);

		}
		//repaint the scene from vector movement.
		static void UpdateRepaint(){
			//If the cube handle position has changed, repaint the scene
			if( repaint ){
				SceneView.RepaintAll();
			}
		}
		
	}
}