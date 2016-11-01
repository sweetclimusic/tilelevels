using UnityEngine;
using UnityEngine.UI;
namespace SweetLevelManager
{

	public class GameController : MonoBehaviour {
		//Win states variables
		public static GameController _instance;
		private int orbsCollected;
		private int orbsTotal;
		GameObject[] orbs;

		public Text scoreTxt;
		public GameObject dynamicObjects;
		private int[][] sampleLevel = new int[][]
		{
			new int[]{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			new int[]{1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 1, 1, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			new int[]{1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			new int[]{1, 0, 0, 0, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			new int[]{1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			new int[]{1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
			new int[]{1, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
			new int[]{1, 0, 0, 0, 0, 3, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1},
			new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
			new int[]{1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
			new int[]{1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
			new int[]{1, 0, 2, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
			new int[]{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1} 
		};
		public Transform wall;
		public Transform ground;
		public GameObject orb;
		public GameObject player;
		public Transform goal;
		private ParticleSystem goalPS;
		[SerializeField]
		private Transform[] component;
		//when building a top down or platformer level.
		[SerializeField]
		private string levelAxis = "Y"; 
		bool buildFloor = false;
		
		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// Detects the player object
		/// </summary>
		void Awake(){
			//this is just a test generating a prefab, 
			//well finding a prefab as we don't use instaniate here
			if(player == null){
				//grab player prefab,
				player =  Resources.Load("Player", typeof(GameObject)) as GameObject;
			}
			if(orb == null){
				orb =  Resources.Load("Collectible", typeof(GameObject)) as GameObject;
			}
			//Check _instance and make it a singleton
				_instance = this;
			
		}
		void Start () {

			if(levelAxis.ToLower() != "y"){
				buildFloor = true;
			}
			component = new Transform[16];
			component[0] = ground;
			component[1] = wall;
			component[2] = player.transform;
			component[3] = orb.transform;
			component[4] = goal;
			BuildLevel();
			//find the Instantiate goal
			goalPS = GameObject.FindGameObjectWithTag("Goal").GetComponent<ParticleSystem>();
			//collect all orbs
			orbsTotal = GameObject.FindGameObjectsWithTag("Orb").Length;
			orbsCollected = 0;
		}
		///<summary>
		/// function from the Unity Game Development Blueprints
		///</summary>
		void BuildLevel(){
			//for everypositive number, use a object.
			///TODO: flip loop based on levelAxis
			int maxRowLength = sampleLevel[0].Length;
			
			for (int column = 0; column < sampleLevel.Length; column++){
				for (int row = 0; row < sampleLevel[column].Length; row++){
					//check the current column length for building a floor.
					if(buildFloor && sampleLevel[column].Length > maxRowLength){
						maxRowLength = sampleLevel[column].Length;
					}
					int currentCoord = sampleLevel[column][row];
					Transform generateObject = null;
					//curent value is positive
					if(currentCoord > 0){
						generateObject = generateProceduralObject(
							component[currentCoord],row,column); 
					}
					/// generate a floor if we have rendered in the z direction
					if(currentCoord <= 0 && buildFloor){
						generateObject = generateProceduralObject(
							component[currentCoord],row,column,-1); 
					}
					if(generateObject != null){
						generateObject.parent = dynamicObjects.transform;	
					}
				}
			}
			
		}

		///<summary>
		/// generate a floor if we have rendered in the z direction
		///</summary>
			Transform generateProceduralObject(Transform prefab,int firstPosition, int secondPosition, int heightOffset = 0){
				/// based on the AXIS desired return a vector3 in that position.
			Vector3 position = 
				( levelAxis.ToLower() == "y" ) ?
				new Vector3(firstPosition,(sampleLevel.Length - secondPosition),heightOffset)
					: new Vector3(firstPosition,heightOffset, sampleLevel.Length - secondPosition);
			Transform generatedObject =  Instantiate(
				prefab, 
				position,
				Quaternion.identity //ALIGN TO parent ignore any rotation
			) as Transform;

			generatedObject.transform.parent = dynamicObjects.transform;
			//adjust the offset of the floor
			return generatedObject;
		}

		public void OrbCollected(){
			orbsCollected++;
			Debug.Log("point");
			//win state reach, show goal particle to player.
			if(orbsCollected >= orbsTotal){
				goalPS.Play();
			}
		} 

		/// <summary>
		/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
		/// </summary>
		void FixedUpdate()
		{
			scoreTxt.text = orbsCollected + " / " + orbsTotal;
		}
		//TODO
		//modify player position to a starting point template
		//add the concept of templates.
		//scan the level via a json object
		//scan the level via an image
		//build and plan level subsection template
	}
}