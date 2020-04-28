using UnityEngine;
using System.Collections;
using Leap;

namespace ANR {

    public class FaceDownGesture : BaseGesture {

        // Use this for initialization
        protected override void Awake() {
            base.Awake();
            currentType = GestureManager.GestureType.FaceDown;
            // gesture event
            SpecificEvent = GestureEvent;
        }

        protected override bool CheckConditionGesture() {
            Hand hand = GetCurrent1Hand();
            if (hand != null) {
                if (IsOpenFullHand(hand) && IsStationary(hand) && IsPalmNormalSameDirectionWith(hand, -Vector3.up)) {
                    return true;
                }
            }
            return false;
        }
        
        void GestureEvent() {
            EventManager.TriggerEvent("Gesture_FaceDown");
        }
    }
}