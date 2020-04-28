using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;
using Leap.Unity;
using System;

namespace ANR
{
    [RequireComponent(typeof(GestureCounter))]
    public class BaseGesture : MonoBehaviour
    {
        #region Properties

        public GestureManager.GestureType currentType;
        public float CheckingTimeBeforeToggle = 1.5f;
        
        protected Transform player;
        protected GestureManager gestureManager;
        protected List<Hand> hands;
        protected GestureCounter _gestureCounter;
        protected bool _isGestureBlocked;
        protected float handForwardDegree = 30;
        protected float gradStrength = 0.9f;
        protected float smallestVelocity = 0.4f;
        protected float deltaVelocity = 0.7f;
        protected float deltaVelocityZ = 0.7f;
        protected float angleChangeRot = 4;
        protected float diffAngle2Hands = 130;
        protected float diffAngle2Velocity = 150;
        
        #endregion

        // Use this for initialization
        protected virtual void Awake()
        {
            _gestureCounter = GetComponent<GestureCounter>();
            _isGestureBlocked = false;
        }

        // Update is called once per frame
        protected void FixedUpdate()
        {
            UpdateHands();
        }

        public void Init(GestureManager manager)
        {
            gestureManager = manager;
        }

        void UpdateHands()
        {
            if (gestureManager != null)
            {
                Frame frame = gestureManager.GetLeapHand().CurrentFrame;
                if (frame != null && frame.Hands != null)
                {
                    hands = frame.Hands;
                    if (!_isGestureBlocked)
                    {
                        if (hands.Count > 0)
                        {
                            if (CheckConditionGesture())
                            {
                                if (_gestureCounter.CurrentState == GestureCounter.CounterState.Stop)
                                {
                                    _gestureCounter.StartTimerUpdatePercentage(CheckingTimeBeforeToggle,
                                        () => { FireEvent(); }, (float percent) =>
                                        {
                                            if (Math.Abs(CheckingTimeBeforeToggle) > 0.01f)
                                                gestureManager.LoadingGestureProgress(currentType, percent);
                                        });
                                }
                            }
                            else
                            {
                                _gestureCounter.StopTimer();
                                gestureManager.LoadingGestureProgress(currentType, 0);
                            }
                        }
                    }
                }
            }
        }

        #region Utility Methods

        public void SetPlayerTransform(Transform t)
        {
            player = t;
        }

        public void UnblockGesture()
        {
            _isGestureBlocked = false;
        }

        public GestureManager.GestureType GetCurrentType()
        {
            return currentType;
        }

        protected Hand GetCurrent1Hand()
        {
            if (hands.Count == 1)
            {
                return hands[0];
            }
            else
            {
                return null;
            }
        }

        protected List<Hand> GetCurrent2Hands()
        {
            if (hands.Count == 2)
            {
                return hands;
            }
            else
            {
                return null;
            }
        }

        #endregion


        #region Virtual Methods

        protected virtual bool CheckConditionGesture()
        {
            return false;
        }

        protected Action SpecificEvent;

        private void FireEvent()
        {
            bool eventSuccess = gestureManager.ReceiveEvent(currentType);
            if (eventSuccess)
            {
                _isGestureBlocked = true;
                if (SpecificEvent != null)
                    SpecificEvent();
            }
        }

        #endregion

        #region Logic Common

        protected Vector3 GetHandVelocity(Hand hand)
        {
            if (player == null)
                player = transform.root;
            return player.InverseTransformDirection(UnityVectorExtension.ToVector3(hand.PalmVelocity));
        }

        protected bool IsMoveLeft(Hand hand)
        {
            return GetHandVelocity(hand).x < -deltaVelocity && !IsStationary(hand);
        }

        protected bool IsMoveRight(Hand hand)
        {
            return GetHandVelocity(hand).x > deltaVelocity && !IsStationary(hand);
        }

        protected bool IsMoveForward(Hand hand)
        {
            return GetHandVelocity(hand).z > deltaVelocityZ /*&& !isStationary(hand)*/;
        }

        protected bool IsMoveBackward(Hand hand)
        {
            return GetHandVelocity(hand).z < -deltaVelocityZ /*&& !isStationary(hand)*/;
        }

        protected bool IsMoveUp(Hand hand)
        {
            return hand.PalmVelocity.y > deltaVelocity && !IsStationary(hand);
        }

        protected bool IsMoveDown(Hand hand)
        {
            return hand.PalmVelocity.y < -deltaVelocity && !IsStationary(hand);
        }

        protected bool IsStationary(Hand hand)
        {
            return hand.PalmVelocity.Magnitude < smallestVelocity;
        }

        protected bool IsHandConfidence(Hand hand)
        {
            return hand.Confidence > 0.5f;
        }

        protected bool IsCloseHand(Hand hand)
        {
            List<Finger> listOfFingers = hand.Fingers;
            int count = 0;
            for (int f = 0; f < listOfFingers.Count; f++)
            {
                Finger finger = listOfFingers[f];
                if ((finger.TipPosition - hand.PalmPosition).Magnitude < deltaCloseFinger)
                {
                    count++;
                }
            }

            return (count == 5);
        }

        protected bool IsOpenFullHand(Hand hand)
        {
            return Math.Abs(hand.GrabStrength) < 0.01f;
        }

        protected bool IsPalmNormalSameDirectionWith(Hand hand, Vector3 dir)
        {
            return IsSameDirection(hand.PalmNormal, UnityVectorExtension.ToVector(dir));
        }

        protected bool IsHandMoveForward(Hand hand)
        {
            return IsSameDirection(hand.PalmNormal, hand.PalmVelocity) && !IsStationary(hand);
        }

        #region ThumbUp/Down

        float deltaAngleThumb = 30;

        // degree
        float deltaCloseFinger = 0.05f;

        protected bool CheckPalmNormalInXZPlane(Hand hand)
        {
            float anglePalmNormal = Angle2LeapVectors(hand.PalmNormal, UnityVectorExtension.ToVector(Vector3.up));

            return (anglePalmNormal > 70 && anglePalmNormal < 110);
        }

        // check thumb finger up/down
        protected bool IsThumbDirection(Hand hand, Vector3 dir)
        {
            List<Finger> listOfFingers = hand.Fingers;
            for (int f = 0; f < listOfFingers.Count; f++)
            {
                Finger finger = listOfFingers[f];

                if (finger.Type == Finger.FingerType.TYPE_THUMB)
                {
                    float angleThumbFinger = Angle2LeapVectors(finger.Direction,
                        UnityVectorExtension.ToVector(dir));
                    float angleThumbFinger2 = Angle2LeapVectors(
                        finger.TipPosition - hand.PalmPosition,
                        UnityVectorExtension.ToVector(dir)); //TODO: FIX finger.StabilizedTipPosition
                    //Debug.Log (angleThumbFinger + " " + angleThumbFinger2);
                    if (angleThumbFinger < deltaAngleThumb
                        || angleThumbFinger2 < deltaAngleThumb)
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }

        // check 4 fingers tip close to palm position
        protected bool CheckFingerCloseToHand(Hand hand)
        {
            List<Finger> listOfFingers = hand.Fingers;
            int count = 0;
            for (int f = 0; f < listOfFingers.Count; f++)
            {
                Finger finger = listOfFingers[f];
                if ((finger.TipPosition - hand.PalmPosition).Magnitude < deltaCloseFinger)
                {
                    if (finger.Type == Finger.FingerType.TYPE_THUMB)
                    {
                        return false;
                    }
                    else
                    {
                        count++;
                    }
                }
            }
            return (count == 4);
        }

        #endregion

        #endregion

        #region Addition

        protected bool IsOppositeDirection(Vector a, Vector b)
        {
            return Angle2LeapVectors(a, b) > (180 - handForwardDegree);
        }

        protected bool IsSameDirection(Vector a, Vector b)
        {
            return Angle2LeapVectors(a, b) < handForwardDegree;
        }

        private static float Angle2LeapVectors(Leap.Vector a, Leap.Vector b)
        {
            return Vector3.Angle(UnityVectorExtension.ToVector3(a), UnityVectorExtension.ToVector3(b));
        }

        #endregion
    }
}