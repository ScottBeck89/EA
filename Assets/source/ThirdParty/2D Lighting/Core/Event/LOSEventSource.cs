using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LOS.Event {

	public class LOSEventSource : MonoBehaviour {

		private Transform _trans;

		[Tooltip ("Related light. Can be null if the event source is not a light.")]
		public LOSLightBase lightSource;
		public LayerMask triggerLayers;
		public LayerMask obstacleLayers;

		[Tooltip ("Event source detect range.")]
		public float distance;


		public delegate void HandleTriggersDelegate (List<LOSEventTrigger> triggers);
		public event HandleTriggersDelegate OnNewTriggersDetected;
		public event HandleTriggersDelegate OnTriggersExitDetected;

		private List<LOSEventTrigger> _triggeredTriggers;


		void Awake () {
			_trans = transform;
			_triggeredTriggers = new List<LOSEventTrigger>();
		}

		void OnEnable () {
			LOSEventManager.instance.AddEventSource(this);
		}
		
		void OnDisable () {
			if (LOSEventManager.TryGetInstance() != null) {
				LOSEventManager.instance.RemoveEventSource(this);
			}
		}

		public void Clear () {
			if (OnTriggersExitDetected != null) {
				OnTriggersExitDetected(_triggeredTriggers);
			}
			_triggeredTriggers.Clear();
		}

		public void Process (List<LOSEventTrigger> triggers) {
			RaycastHit hit;

			List<LOSEventTrigger> triggeredTriggers = new List<LOSEventTrigger>();
			List<LOSEventTrigger> notTriggeredTriggers = new List<LOSEventTrigger>();

			foreach (LOSEventTrigger trigger in triggers) {

				if (!SHelper.CheckGameObjectInLayer(trigger.gameObject, triggerLayers)) continue;

				bool triggered = false;

				Vector3 direction = trigger.position - _trans.position;
				float degree = SMath.VectorToDegree(direction);
				if (lightSource != null) {
					if (!lightSource.CheckDegreeWithinCone(degree)) {
						notTriggeredTriggers.Add(trigger);
						continue;
					}
				}

				if (direction.sqrMagnitude <= distance * distance) {	// Within distance
					if (triggeredTriggers.Contains(trigger)) continue;		// May be added previously

					LayerMask mask = 1 << trigger.gameObject.layer | obstacleLayers;

                    //Custom addition for 2d raycasting
                    if ( LOSManager.instance.physicsOpt == LOSManager.PhysicsOpt.Physics_2D )
                    {
                        RaycastHit2D rayHit2D = Physics2D.Raycast( _trans.position, direction, distance, mask ); ;

                        //List<Vector3> triggerExtents = trigger.GetComponent<Collider2D>().s;

                        List<int> degreeOffsets = new List<int> { 1, -1, 2, -2, 4, -4, 8, -8, 16, -16, 32, -32 };

                        if ( rayHit2D.collider == null || rayHit2D.collider.gameObject != trigger.gameObject )
                        {
                            foreach ( int degreeOffset in degreeOffsets )
                            {
                                direction = SMath.DegreeToUnitVector( degree + degreeOffset );

                                rayHit2D = Physics2D.Raycast( _trans.position, direction, distance, mask );

                                if ( rayHit2D.collider != null && rayHit2D.collider.gameObject == trigger.gameObject )
                                    break;
                            }
                        }


                        if ( rayHit2D.collider != null )
                        {
                            GameObject hitGo = rayHit2D.collider.gameObject;

                            if ( hitGo == trigger.gameObject )
                            {
                                Debug.DrawRay( _trans.position, direction * 15f, Color.red, 0.2f );
                                triggered = true;
                            }
                            else if ( hitGo.layer == trigger.gameObject.layer )
                            {
                                LOSEventTrigger triggerToAdd = hitGo.GetComponentInChildren<LOSEventTrigger>();
                                if ( triggerToAdd == null )
                                {
                                    triggerToAdd = hitGo.GetComponentInParent<LOSEventTrigger>();
                                }

                                if ( triggerToAdd != null )
                                {
                                    triggeredTriggers.Add( triggerToAdd );
                                }
                            }
                        }
                    }
                    else
                    {
                        if ( Physics.Raycast( _trans.position, direction, out hit, distance, mask ) )
                        {
                            GameObject hitGo = hit.collider.gameObject;

                            if ( hitGo == trigger.gameObject )
                            {
                                triggered = true;
                            }
                            else if ( hitGo.layer == trigger.gameObject.layer )
                            {
                                LOSEventTrigger triggerToAdd = hitGo.GetComponentInChildren<LOSEventTrigger>();
                                if ( triggerToAdd == null )
                                {
                                    triggerToAdd = hitGo.GetComponentInParent<LOSEventTrigger>();
                                }

                                if ( triggerToAdd != null )
                                {
                                    triggeredTriggers.Add( triggerToAdd );
                                }
                            }
                        }
                    }
				}

				if (triggered) {
					triggeredTriggers.Add(trigger);
				}
				else {
					notTriggeredTriggers.Add(trigger);
				}

			}

			List<LOSEventTrigger> newTriggers = new List<LOSEventTrigger>();
			foreach (LOSEventTrigger trigger in triggeredTriggers) {
				trigger.TriggeredBySource(this);

				if (!_triggeredTriggers.Contains(trigger)) {
					newTriggers.Add(trigger);
				}
			}
			if (OnNewTriggersDetected != null && newTriggers.Count > 0) {
				OnNewTriggersDetected(newTriggers);
			}

			List<LOSEventTrigger> triggersExit = new List<LOSEventTrigger>();
			foreach (LOSEventTrigger trigger in _triggeredTriggers) {
				if (!triggeredTriggers.Contains(trigger)) {
					triggersExit.Add(trigger);
				}
			}
			if (OnTriggersExitDetected != null && triggersExit.Count > 0) {
				OnTriggersExitDetected(triggersExit);
			}

			foreach (LOSEventTrigger trigger in notTriggeredTriggers) {
				trigger.NotTriggeredBySource(this);
			}

			_triggeredTriggers = triggeredTriggers;
		}
	}

}