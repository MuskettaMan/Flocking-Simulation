using FlockingSimulator.Buckets;
using FlockingSimulator.Extensions;
using nl.DTT.KVA.Example;
using System.Collections.Generic;
using UnityEngine;
/*
*	Author  - Ferri de Lange
*	Date	- 17.05.20
*/

namespace FlockingSimulator
{

    ///<summary>
    /// Boid behaviour
    ///</summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Boid : MonoBehaviour
    {
        #region Variables
        #region Editor
        /// <summary>
        /// The radius it can 'sense' other boids
        /// </summary>
        [SerializeField, Tooltip("The radius it can 'sense' other boids")]
        private float perceptionRadius;


        /// <summary>
        /// The maximum velocity the boid can go
        /// </summary>
        [SerializeField, Tooltip("The maximum velocity the boid can go")]
        private float maxSpeed;

        /// <summary>
        /// The maximum force that can be applied to the rigidbody
        /// </summary>
        [SerializeField, Tooltip("The maximum force that can be applied to the rigidbody")]
        private float maxForce;

        /// <summary>
        /// The angle the boids can see in front of them
        /// </summary>
        [SerializeField, Tooltip("The angle the boids can see in front of them"), Range(0, 180)]
        private float angle;
        #endregion
        #region Public
        /// <summary>
        /// Reference to the rigidbody component
        /// </summary>
        public new Rigidbody rigidbody { get; private set; }

        /// <summary>
        /// Public accessor for the perception radius
        /// <para>The radius it can 'sense' other boids</para>
        /// </summary>
        public float PerceptionRadius
        {
            get => perceptionRadius;
            private set => perceptionRadius = value;
        }

        /// <summary>
        /// The bucket this boid is contained inside
        /// </summary>
        public Bucket Bucket { get; set; }
        #endregion
        #region Private
        /// <summary>
        /// The flocking forces that will be / are applied this frame
        /// </summary>
        private FlockingForces flockingForces;

        private Vector3 edgeForces;
        #endregion
        #endregion
        #region Methods
        #region Unity

        /// <summary>
        /// Gets components
        /// Setup starting position and velocity
        /// </summary>
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();

            ResetBoid();
        }

        private void Update()
        {
            rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity);
        }

        /// <summary>
        /// Debug information
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Vector3 dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)) * perceptionRadius;
            dir = transform.TransformDirection(dir);
            Gizmos.DrawRay(transform.position, dir);

            dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * -angle), 0, Mathf.Cos(Mathf.Deg2Rad * -angle)) * perceptionRadius;
            dir = transform.TransformDirection(dir);
            Gizmos.DrawRay(transform.position, dir);
        }
        #endregion
        #region Public
        /// <summary>
        /// The flock behaviour
        /// </summary>
        public void Flock()
        {
            if (Bucket == null)
                return;

            flockingForces.Reset();
            edgeForces = Vector3.zero;

            int total = 0;

            var boids = GetBoidsFromSurroundingBuckets();

            foreach(Boid boid in boids)
            {
                Vector3 delta = transform.position - boid.transform.position;

                if (delta.sqrMagnitude < perceptionRadius * perceptionRadius && boid != this)
                {
                    if (Vector3.Angle(transform.forward, delta) > -angle && Vector3.Angle(transform.forward, delta) < angle)
                    {
                        // Alignment
                        flockingForces.alignment += boid.rigidbody.velocity;

                        // Cohesion
                        flockingForces.cohesion += boid.rigidbody.position;

                        // Separation
                        Vector3 diff = rigidbody.position - boid.rigidbody.position;
                        diff = new Vector3(diff.x * Mathf.Abs(diff.x), diff.y * Mathf.Abs(diff.y), diff.z * Mathf.Abs(diff.z));
                        diff /= delta.sqrMagnitude;
                        flockingForces.separation += diff;

                        total++;
                    }
                }
            }

            var startPos = GameManager.Instance.FieldSize * 0.1f + GameManager.Instance.transform.position;
            var endPos = GameManager.Instance.FieldSize * 0.9f + GameManager.Instance.transform.position;

            var pos = rigidbody.position;
            Vector3 edgeForce;
            // Out of bounds
            edgeForce = new Vector3(
                pos.x < startPos.x ? -(pos.x - startPos.x) : 0,
                pos.y < startPos.y ? -(pos.y - startPos.y) : 0,
                pos.z < startPos.z ? -(pos.z - startPos.z) : 0
            );
            edgeForces += edgeForce * 10;

            edgeForce = new Vector3(
                pos.x > endPos.x ? -(pos.x - endPos.x) : 0,
                pos.y > endPos.y ? -(pos.y - endPos.y) : 0,
                pos.z > endPos.z ? -(pos.z - endPos.z) : 0
            );
            edgeForces += edgeForce * 10;

            if (total > 0)
            {
                // Alignment
                flockingForces.alignment /= total;
                flockingForces.alignment = flockingForces.alignment.normalized * maxSpeed;
                flockingForces.alignment = flockingForces.alignment.Limit(maxForce);

                // Cohesion
                flockingForces.cohesion /= total;
                flockingForces.cohesion -= rigidbody.position;
                flockingForces.cohesion = flockingForces.cohesion.normalized * maxSpeed;
                flockingForces.cohesion = flockingForces.cohesion.Limit(maxForce);

                // Separation
                flockingForces.separation /= total;
                flockingForces.separation = flockingForces.separation.normalized * maxSpeed;
                flockingForces.separation = flockingForces.separation.Limit(maxForce);
            }

            // Increases behaviours from UI sliders
            flockingForces.alignment *= FlockingManager.Instance.ControlSliders.AlignmentSlider.value;
            flockingForces.cohesion *= FlockingManager.Instance.ControlSliders.CohesionSlider.value;
            flockingForces.separation *= FlockingManager.Instance.ControlSliders.SeparationSlider.value;
        }

        /// <summary>
        /// Applies the forces computed from <see cref="Flock"/> onto the boids rigidbody
        /// </summary>
        public void ApplyForces()
        {
            if (!gameObject.activeSelf)
                return;
            
            rigidbody.AddForce(flockingForces.alignment);
            rigidbody.AddForce(flockingForces.cohesion);
            rigidbody.AddForce(flockingForces.separation);
            rigidbody.AddForce(edgeForces);
            rigidbody.velocity = rigidbody.velocity.Limit(maxSpeed);
        }

        /// <summary>
        /// Resets the boids velocity and position
        /// </summary>
        public void ResetBoid()
        {
            var size = GameManager.Instance.FieldSize;
            transform.localPosition = new Vector3(Random.Range(0, size.x), Random.Range(0, size.y), Random.Range(0, size.z));
            rigidbody.velocity = new Vector3(GetRandom(), GetRandom(), GetRandom()).normalized;
            rigidbody.velocity *= Random.Range(3.0f, 4.0f);
        }
        #endregion
        #region Protected

        #endregion
        #region Private
        /// <summary>
        /// Used for setting the initial velocity
        /// </summary>
        /// <returns>A random value of 1 or -1</returns>
        private int GetRandom()
        {
            int random = Random.Range(0, 2);
            if (random == 0)
                return -1;
            else
                return random;
        }

        /// <summary>
        /// Get all the buckets that are visible in the perception radius
        /// </summary>
        /// <returns></returns>
        private List<Boid> GetBoidsFromSurroundingBuckets()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, perceptionRadius);
            List<Bucket> buckets = new List<Bucket>();
            for (int i = 0; i < colliders.Length; i++)
            {
                Bucket tryBucket;
                if (colliders[i].TryGetComponent<Bucket>(out tryBucket))
                {
                    buckets.Add(tryBucket);
                }
            }

            List<Boid> boids = new List<Boid>();
            for (int i = 0; i < buckets.Count; i++)
            {
                boids.AddRange(buckets[i].Boids);
            }

            return boids;
        }
        #endregion
        #endregion
    }

    internal struct FlockingForces
    {
        public Vector3 alignment;
        public Vector3 cohesion;
        public Vector3 separation;

        public FlockingForces(Vector3 alignment, Vector3 cohesion, Vector3 separation)
        {
            this.alignment = alignment;
            this.cohesion = cohesion;
            this.separation = separation;
        }

        public void Reset()
        {
            alignment = Vector3.zero;
            cohesion = Vector3.zero;
            separation = Vector3.zero;
        }
    }
}
