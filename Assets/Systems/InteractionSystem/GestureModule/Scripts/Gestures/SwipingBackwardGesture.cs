using UnityEngine;
using System.Collections;
using Leap;

namespace ANR {

    public class SwipingBackwardGesture : BaseGesture {

        // Use this for initialization
        protected override void Awake() {
            base.Awake();
            currentType = GestureManager.GestureType.SwipingBackward;
            // gesture event
            SpecificEvent = GestureEvent;
        }

        protected override bool CheckConditionGesture() {
            Hand hand = GetCurrent1Hand();
            if (hand != null) {
                if (IsOpenFullHand(hand) && !IsMoveForward(hand) && IsPalmNormalSameDirectionWith(hand, Vector3.back)) {
                    return true;
                }
            }
            return false;
        }

        void GestureEvent() {
            EventManager.TriggerEvent("Gesture_SwipingBackward");
        }
    }
}
