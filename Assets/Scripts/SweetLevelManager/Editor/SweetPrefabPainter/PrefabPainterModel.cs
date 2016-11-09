using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SweetPrefabPainter {
	
	public class PrefabPainterModel : Editor {

		public static Vector3 CurrentHandlePosition = Vector3.zero;
		static Vector3 prevHandlePosition = Vector3.zero;

		//properties based on selection.
		static float width = 1.0f;
		static float height = 1.0f;
		static float count = 1.0f;
		//Tools selected is 0 or 1, 
		// 0 no tool or actively painting.
		// 1 erase
		// 2 paint
		public static int SelectedTool {
			get{
				return EditorPrefs.GetInt("SelectedSweetPrefabValue",0);
			}
			set{
				//don't do anything if equal
				if(value == SelectedTool){
					return;
				}
				EditorPrefs.GetInt("SelectedSweetPrefabValue",value);
				switch (value)
				{
				// on choice of button choosen set it as opposite.
				case 0: 
				case 1:
				case 2: //paint
					//End
					EditorPrefs.SetBool("SweetPrefabPaintingEnd",!EditorPrefs.GetBool("SweetPrefabPaintingEnd"));

					//Paint
					EditorPrefs.SetBool("SweetPrefabPainting", 
						!EditorPrefs.GetBool("SweetPrefabPainting")
					);
					//hide tools if painting or not.
					Tools.hidden = EditorPrefs.GetBool("SweetPrefabPainting");
					break;
				default:
					break;
				}
			}
		}//end SelectedTool property

		//using a vector 3 to store 3 float variables
		// x = scale height
		// y = scale width
		// z = number of prefabs to draw.
		public static Vector3 SelectedScale{
			get{
				return new Vector3 (
					EditorPrefs.GetFloat ("SweetPrefabYScale", height),
					EditorPrefs.GetFloat ("SweetPrefabXScale", width),
					EditorPrefs.GetFloat ("SweetPrefabCount", count) 
				);
			}
			set{
				EditorPrefs.SetFloat ("SweetPrefabYScale", value.y);
				EditorPrefs.SetFloat ("SweetPrefabXScale", value.x);
				EditorPrefs.SetFloat ("SweetPrefabCount", value.z); 
			}
		}//end SelectedScale prefab
			
		public static void ViewToRepaint(){
				//If the cube handle position has changed, repaint the scene
				if( CurrentHandlePosition != prevHandlePosition )
				{
					PrefabPainter.RepaintPrefabPainter = true;
					//update of controller position in the controller.
					prevHandlePosition = CurrentHandlePosition;
				}
		}
	}//end class
}//end namespace