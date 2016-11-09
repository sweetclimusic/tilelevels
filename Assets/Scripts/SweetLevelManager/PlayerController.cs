using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;
namespace SweetLevelManager
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof (UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter))]
	[RequireComponent(typeof (UnityStandardAssets.Cameras.AutoCam))]
	[RequireComponent(typeof (UnityStandardAssets.Cameras.LookatTarget))]
	public class PlayerController : MonoBehaviour {
		Rigidbody player_RigidBody;
		// Set to true when the player can jump
		private bool canJump;
		private bool collidingWall;
		private int jumpCount = 0;
		private ThirdPersonUserControl tpControl;
		private ThirdPersonCharacter tpc;
		
		void Start(){
			player_RigidBody = GetComponent<Rigidbody>();
			freezePositon();
			AssignToCamera();
			//get the ThirdPersonCharacter script component.
			tpControl = GetComponent<ThirdPersonUserControl>();
			tpc = GetComponent<ThirdPersonCharacter>();
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

		void FixedUpdate () {
			CheckWallStatus();
		}

		/// <summary>
		/// Movement is handle by the ThirdPersonCharacter script from the standard Assest
		/// This is all additional Functionality I want my player to perform
		/// </summary>
		void CheckWallStatus(){
			//Stop force when colliding with a wall.
			float speed = 0.5f;
			//grab the input of player and find speed in X direction.
			float xMove = Input.GetAxis("Horizontal");
			if(xMove != 0 ){
				float xSpeed = Mathf.Abs(xMove * player_RigidBody.velocity.x);
				Vector3 movementForce = Vector3.right;
				movementForce *= xMove * speed;
				//get playerSpeed and the maxSpeed
				if( xSpeed <  3.0f){
					RaycastHit hit;
					if(!player_RigidBody.SweepTest(tpControl.getMove(),out hit,0.05f)){
						player_RigidBody.AddForce(movementForce);
					}
				}
			}
			// The logic from blueprint is that if there isn't a hit then move.
			// TPC already moves.. so reverse logic on hit stop movement... but what is stop?
			//Inspect tpControl more to determine.
		}
		/*
		* Jump function test, Functions that test various states of the Player and state if they can jump or not.
		*/
		bool WallJump(bool playerGroundedState){
			//Ninja Gaiden Wall Jump;
			//Rule 1
			//1st Test if player is colliding with a wall?
			//2nd Must be falling
			//3rd 0 in the Jump counter
			if(collidingWall && !playerGroundedState && this.jumpCount == 0 ){
				return true;
			}
			return false;
			// Only then on Jump pressed can a jump occur

		}

		Vector3 WallBounce(ControllerColliderHit hitObject){
			//Theory to implement Unity Physics wall jumping
			//1st Player direction vector..
			Vector3 playerMoveDirection = Vector3.zero;
			//Collision with the wall
			if(collidingWall && hitObject.normal.y <= 0.1f ){
				//2nd Grab Triangle mesh collision occured
				//3rd Retrive World poistion of Triangle mesh of the Wall
				//all found in hitObject.point
				//Retrive Normal vector.. this is given with hitObject.normal;
				//Test Slope of Normal Vector if ok for a wall jump, < 0.5 == Ok to wall jump.
				//return the normal direction vector we want the player to head into
					//updateJumpforce
					//ie. verticalVelocity = jumpforce.
					playerMoveDirection = hitObject.normal; // * jumpMovementSpeed
					//TODO cleanup with ninja Gaiden wall jump logic.
				}
			return playerMoveDirection;
		}
		// coroutine function to allow a limit window when a player is no longer grounded 
		//but can still jump, try 14frames
		bool DelayJump(){
			return false;
		}
		


		/// <summary>
		/// OnCollisionEnter is called when this collider/rigidbody has begun
		/// touching another rigidbody/collider.
		/// </summary>
		/// <param name="other">The Collision data associated with this collision.</param>
		void OnTriggerEnter(Collider other)
		{
			if(other.gameObject.tag == "Orb"){
				GameController._instance.OrbCollected();
			}
		}

		/// <summary>
		/// OnCollisionExit is called when this collider/rigidbody has
		/// stopped touching another rigidbody/collider.
		/// </summary>
		/// <param name="other">The Collision data associated with this collision.</param>
		void OnCollisionExit(Collision other)
		{
			// specfic tag of a Jumpwall
			if(other.gameObject.tag == "JumpWall"){
				//should this be a trigger?
				resetJumpCounter();
			}
		}

		/// <summary>
		/// OnControllerColliderHit is called when the controller hits a
		/// collider while performing a Move.
		/// </summary>
		/// <param name="hit">The ControllerColliderHit data associated with this collision.</param>
		void OnControllerColliderHit(ControllerColliderHit hit){
			// specfic tag of a Jumpwall
			if(hit.collider.tag == "JumpWall"){
				//visualize the Jump line.
				#if UNITY_EDITOR
				Debug.DrawRay(hit.point,hit.normal,Color.red,1.25f);
				#endif
				this.collidingWall = true;
				WallBounce(hit);
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

		/*
		*GETTER SETTER LAND
		* public exposed functions mainly to talk to the Standard Assest Controllers if need be
		*/
		public int JumpCount{
			get{
				return jumpCount;
			}
			set{
				jumpCount = value;
			}
		}
		public bool CanJump{
			get{
				return this.canJump;
			}
			set{
				this.canJump = value;
			}
		}
		

		//Rule 2
		//If they no longer Colliding with the wall
		//OR grounded reset Jump Counter to 0
		void resetJumpCounter(){
			this.jumpCount = 0;
			this.collidingWall = false;
		}

	}

	
	
}