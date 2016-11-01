using UnityEngine;
using UnityStandardAssets.Cameras;
namespace SweetLevelManager
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof (UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter))]
	[RequireComponent(typeof (UnityStandardAssets.Cameras.AutoCam))]
	[RequireComponent(typeof (UnityStandardAssets.Cameras.LookatTarget))]
	public class PlayerController : MonoBehaviour {
		Rigidbody player_RigidBody;
		// Set to true when the player can jump
		private bool shouldJump;
		
		void Start(){
			player_RigidBody = GetComponent<Rigidbody>();
			freezePositon();
			AssignToCamera();
		}
		///<summary>
		/// Unity standard assest pack unlocks the inspector changes to the ethan prefab.
		/// want to lock it based on the level type generated.
		///</summary>
		void freezePositon()
		{
			//for levels generated in the Y axis, freeze Z position
			player_RigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}
		// Use this for initialization
		void Awake() {
			
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		// coroutine function to allow a limit window when a player is no longer grounded 
		//but can still jump, try 14frames
		bool canJump(){
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// OnCollisionEnter is called when this collider/rigidbody has begun
		/// touching another rigidbody/collider.
		/// </summary>
		/// <param name="other">The Collision data associated with this collision.</param>
		void OnTriggerEnter(Collider other)
		{
			Debug.Log(other.gameObject.tag);
			if(other.gameObject.tag == "Orb"){
				Debug.Log("found");
				GameController._instance.OrbCollected();
			}
		}

		///<summary>
		/// function to bind the Camera to this player
		///</summary>
		void AssignToCamera(){
			GameObject camera = GameObject.Find("MultipurposeCameraRig");
			var autoCamScriptObject = camera.GetComponent<AutoCam>();
			var lookAtTargetScriptObject = camera.GetComponent<LookatTarget>();

			autoCamScriptObject.SetTarget(this.transform);
			lookAtTargetScriptObject.SetTarget(this.transform); 

		}
	}

	
	
}