using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using System;
using QEntity;

namespace KinematicCharacterController.Walkthrough.AddingImpulses
{
    [RequireComponent(typeof(UnitEntity))]
    public class QCharacterController : MonoBehaviour, ICharacterController
    {
        [ReadOnly]
        public UnitEntity unit;
        public KinematicCharacterMotor Motor;
        public CapsuleCollider Capsule => Motor.Capsule;

        public float MaxStableMoveSpeed => unit.groundMoveSpeed;
        public float StableMovementSharpness => unit.movementSharpness;

        public float MaxAirMoveSpeed => unit.airMoveSpeed;
        public float AirAccelerationSpeed => unit.airAccelerationSpeed;      // 空中水平阻力
        public float Drag => unit.drag;

        public bool AllowJumpingWhenSliding = false;
        public bool AllowDoubleJump = true;
        public bool AllowWallJump = false;
        public float JumpSpeed => unit.jumpSpeed;
        [HideInInspector] public float JumpPreGroundingGraceTime = 0f;
        [HideInInspector] public float JumpPostGroundingGraceTime = 0f;

        public Vector3 Gravity => unit.gravity;
        public Transform MeshRoot;

        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        private bool _jumpRequested = false;
        private bool _jumpConsumed = false;
        private bool _jumpedThisFrame = false;
        private float _timeSinceJumpRequested = Mathf.Infinity;
        private float _timeSinceLastAbleToJump = 0f;
        private bool _doubleJumpConsumed = false;
        private bool _canWallJump = false;
        private Vector3 _wallJumpNormal;
        private Vector3 _internalVelocityAdd = Vector3.zero;
        private float currentSpeed;

        private void Start()
        {
            // Assign to motor
            Motor.CharacterController = this;
            //Capsule.isTrigger = true;
        }

        private void OnValidate()
        {
            unit = GetComponent<UnitEntity>();
        }

        bool inputMove = false;
        public void Move(Vector3 moveDir)
        {
            inputMove = true;
            _moveInputVector = moveDir;
            _lookInputVector = moveDir;
        }

        public void MoveToPosition(Vector3 position)
        {
            Motor.MoveCharacter(position);
        }

        public void SetPosition(Vector3 position)
        {
            Motor.SetPosition(position);
        }

        public void SetRotation(Quaternion rotation)
        {
            Motor.SetRotation(rotation);
        }

        public void Jump()
        {
            _timeSinceJumpRequested = 0f;
            _jumpRequested = true;
        }

        public void ForceUnGround()
        {
            Motor.ForceUnground();
        }

        public void SetCapsuleDimensions(float radius, float height, float yOffset)
        {
            Motor.SetCapsuleDimensions(radius, height, yOffset);
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        //public float OrientationSharpness = 10;
        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            //if (_lookInputVector != Vector3.zero && OrientationSharpness > 0f)
            //{
            //    // Smoothly interpolate from current to target look direction
            //    Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

            //    // Set the current rotation (which will be used by the KinematicCharacterMotor)
            //    currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
            //}
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            Vector3 targetMovementVelocity = Vector3.zero;
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                // Reorient velocity on slope
                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.zero;
                if (inputMove)
                {
                    inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                }
                Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, Mathf.Exp(-StableMovementSharpness * deltaTime));
            }
            else
            {
                // Add move input
                if (inputMove)
                {
                    targetMovementVelocity = _moveInputVector * MaxAirMoveSpeed;

                    // Prevent climbing on un-stable slopes with air movement
                    if (Motor.GroundingStatus.FoundAnyGround)
                    {
                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                        targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                    }

                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Gravity);
                    currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                }

                // Drag
                currentVelocity *= (1f / (1f + (Drag * deltaTime)));

                // Gravity
                currentVelocity += Gravity * deltaTime;
            }
            inputMove = false;

            // Handle jumping
            {
                _jumpedThisFrame = false;
                _timeSinceJumpRequested += deltaTime;
                if (_jumpRequested)
                {
                    // Handle double jump
                    if (AllowDoubleJump)
                    {
                        if (_jumpConsumed && !_doubleJumpConsumed && (AllowJumpingWhenSliding ? !Motor.GroundingStatus.FoundAnyGround : !Motor.GroundingStatus.IsStableOnGround))
                        {
                            Motor.ForceUnground(0.1f);

                            // Add to the return velocity and reset jump state
                            currentVelocity += (Motor.CharacterUp * JumpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                            _jumpRequested = false;
                            _doubleJumpConsumed = true;
                            _jumpedThisFrame = true;
                        }
                    }

                    // See if we actually are allowed to jump
                    if (_canWallJump ||
                        (!_jumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime)))
                    {
                        // Calculate jump direction before ungrounding
                        Vector3 jumpDirection = Motor.CharacterUp;
                        if (_canWallJump)
                        {
                            jumpDirection = _wallJumpNormal;
                        }
                        else if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                        {
                            jumpDirection = Motor.GroundingStatus.GroundNormal;
                        }

                        // Makes the character skip ground probing/snapping on its next update. 
                        // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                        Motor.ForceUnground(0.1f);

                        // Add to the return velocity and reset jump state
                        currentVelocity += (jumpDirection * JumpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                        _jumpRequested = false;
                        _jumpConsumed = true;
                        _jumpedThisFrame = true;
                    }
                }

                // Reset wall jump
                _canWallJump = false;
            }

            //DebugTool.LogFormat("Is {0}   {1}", Gravity * deltaTime, currentVelocity);
            // Take into account additive velocity
            if (_internalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
            }
            currentSpeed = currentVelocity.magnitude;
        }

        public float GetCurrentSpeed()
        {
            return currentSpeed;
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            // Handle jump-related values
            {
                // Handle jumping pre-ground grace period
                if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
                {
                    _jumpRequested = false;
                }

                if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
                {
                    // If we're on a ground surface, reset jumping values
                    if (!_jumpedThisFrame)
                    {
                        _doubleJumpConsumed = false;
                        _jumpConsumed = false;
                    }
                    _timeSinceLastAbleToJump = 0f;
                }
                else
                {
                    // Keep track of time since we were last able to jump (for grace period)
                    _timeSinceLastAbleToJump += deltaTime;
                }
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            // We can wall jump only if we are not stable on ground and are moving against an obstruction
            if (AllowWallJump && !Motor.GroundingStatus.IsStableOnGround && !hitStabilityReport.IsStable)
            {
                _canWallJump = true;
                _wallJumpNormal = hitNormal;
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AddVelocity(Vector3 velocity)
        {
            ForceUnGround();
            _internalVelocityAdd += velocity;
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }
    }
}