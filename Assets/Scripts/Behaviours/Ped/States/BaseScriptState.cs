using UnityEngine;
using SanAndreasUnity.Utilities;

namespace SanAndreasUnity.Behaviours.Peds.States
{

	/// <summary>
	/// Base class for all states that are scripts.
	/// </summary>
	public abstract class BaseScriptState : MonoBehaviour, IPedState
	{

		protected Ped m_ped;
		protected PedModel m_model { get { return m_ped.PlayerModel; } }
	//	protected StateMachine m_stateMachine;
		protected new Transform transform { get { return m_ped.transform; } }
		public bool IsActiveState { get { return m_ped.CurrentState == this; } }



		protected virtual void Awake ()
		{
			m_ped = this.GetComponentInParent<Ped> ();
		}

		protected virtual void OnEnable ()
		{
			
		}

		protected virtual void OnDisable ()
		{
			
		}

		protected virtual void Start ()
		{

		}

		public virtual void OnBecameActive ()
		{
			
		}

		public virtual void OnBecameInactive ()
		{
			
		}

		public virtual bool RepresentsState (System.Type type)
		{
			var myType = this.GetType ();
			return myType.Equals (type) || myType.IsSubclassOf (type);
		}

		public bool RepresentsState<T> () where T : IState
		{
			return this.RepresentsState (typeof(T));
		}

		public virtual void UpdateState() {

			// read input

			// call appropriate function for every input action


			this.ConstrainPosition();
			this.ConstrainRotation();

		}

		public virtual void PostUpdateState()
		{
			if (m_ped.Camera)
				this.UpdateCamera ();
		}

		public virtual void LateUpdateState()
		{

			if (m_ped.shouldPlayAnims)
				this.UpdateAnims ();
			
		}

		public virtual void FixedUpdateState()
		{

			this.UpdateHeading();
			this.UpdateRotation();
			this.UpdateMovement();

		}

		protected virtual void ConstrainPosition()
		{
			m_ped.ConstrainPosition();
		}

		protected virtual void ConstrainRotation ()
		{
			m_ped.ConstrainRotation();
		}

		protected virtual void UpdateHeading()
		{
			m_ped.UpdateHeading ();
		}

		protected virtual void UpdateRotation()
		{
			m_ped.UpdateRotation ();
		}

		protected virtual void UpdateMovement()
		{
			m_ped.UpdateMovement ();
		}

		public virtual void UpdateCamera()
		{
			this.RotateCamera();
			this.UpdateCameraZoom();
			this.CheckCameraCollision ();
		}

		public virtual void RotateCamera()
		{
			BaseScriptState.RotateCamera(m_ped, m_ped.MouseMoveInput, m_ped.CameraClampValue.y);
		}

		public static void RotateCamera(Ped ped, Vector2 mouseDelta, float xAxisClampValue)
		{
			Camera cam = ped.Camera;

			if (mouseDelta.sqrMagnitude < float.Epsilon)
				return;

		//	cam.transform.Rotate( new Vector3(-mouseDelta.y, mouseDelta.x, 0f), Space.World );
			var eulers = cam.transform.eulerAngles;
		//	eulers.z = 0f;
			eulers.x += - mouseDelta.y;
			eulers.y += mouseDelta.x;
			// adjust x
			if (eulers.x > 180f)
				eulers.x -= 360f;
			// clamp
			if (xAxisClampValue > 0)
				eulers.x = Mathf.Clamp(eulers.x, -xAxisClampValue, xAxisClampValue);

			cam.transform.rotation = Quaternion.AngleAxis(eulers.y, Vector3.up)
				* Quaternion.AngleAxis(eulers.x, Vector3.right);
			
		}

		public virtual Vector3 GetCameraFocusPos()
		{
			return m_ped.transform.position + Vector3.up * 0.5f;
		}

		public virtual float GetCameraDistance()
		{
			return m_ped.CameraDistance;
		}

		public virtual void UpdateCameraZoom()
		{
			m_ped.CameraDistance = Mathf.Clamp(m_ped.CameraDistance - m_ped.MouseScrollInput.y, 2.0f, 32.0f);
		}

		public virtual void CheckCameraCollision()
		{
			BaseScriptState.CheckCameraCollision (m_ped, this.GetCameraFocusPos (), -m_ped.Camera.transform.forward, 
				this.GetCameraDistance ());
		}

		public static void CheckCameraCollision(Ped ped, Vector3 castFrom, Vector3 castDir, float cameraDistance)
		{
			
			// cast a ray from ped to camera to see if it hits anything
			// if so, then move the camera to hit point

			Camera cam = ped.Camera;

			float distance = cameraDistance;
			var castRay = new Ray(castFrom, castDir);
			RaycastHit hitInfo;

			if (Physics.SphereCast(castRay, 0.25f, out hitInfo, distance,
				-1 ^ (1 << MapObject.BreakableLayer) ^ (1 << Vehicles.Vehicle.Layer)))
			{
				distance = hitInfo.distance;
			}

			cam.transform.position = castRay.GetPoint(distance);

		}

		protected virtual void UpdateAnims()
		{
			
		}


		public virtual void OnFireButtonPressed()
		{

		}

		public virtual void OnAimButtonPressed()
		{

		}

		public virtual void OnSubmitPressed()
		{

		}

		public virtual void OnJumpPressed()
		{

		}

		public virtual void OnCrouchButtonPressed()
		{

		}

		public virtual void OnNextWeaponButtonPressed()
		{
			m_ped.WeaponHolder.SwitchWeapon (true);
		}

		public virtual void OnPreviousWeaponButtonPressed()
		{
			m_ped.WeaponHolder.SwitchWeapon (false);
		}

		public virtual void OnFlyButtonPressed()
		{

		}

		public virtual void OnFlyThroughButtonPressed()
		{

		}

		public virtual void OnDamaged(DamageInfo info)
		{

		}

	}

}
