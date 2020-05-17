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
    ///Class Description
    ///</summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Boid : MonoBehaviour
    {
        #region Variables
        #region Editor
        [SerializeField]
        private float perceptionRadius;

        [SerializeField]
        private float maxSpeed;

        [SerializeField]
        private float maxForce;
        #endregion
        #region Public
        public new Rigidbody rigidbody { get; private set; }
        public float PerceptionRadius
        {
            get => perceptionRadius;
            private set => perceptionRadius = value;
        }

        public Bucket Bucket { get; set; }
        #endregion
        #region Private

        private FlockingForces flockingForces;

        private bool forcesApplied = true;
        #endregion
        #endregion
        #region Methods
        #region Unity

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();

            var size = GameManager.Instance.Size;
            transform.localPosition = new Vector3(Random.Range(0, size.x), Random.Range(0, size.y), Random.Range(0, size.z));
            rigidbody.velocity = new Vector3(GetRandom(), GetRandom(), GetRandom()).normalized;
            rigidbody.velocity *= Random.Range(5.0f, 6.0f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(rigidbody.position, perceptionRadius);
        }
        #endregion
        #region Public
        public void Flock()
        {

            if (Bucket == null)
                return;

            flockingForces.Reset();

            int total = 0;

            Collider[] colliders = Physics.OverlapSphere(transform.position, perceptionRadius);
            List<Bucket> buckets = new List<Bucket>();
            for (int i = 0; i < colliders.Length; i++)
            {
                Bucket tryBucket;
                if(colliders[i].TryGetComponent<Bucket>(out tryBucket))
                {
                    buckets.Add(tryBucket);
                }
            }

            List<Boid> boids = new List<Boid>();
            for(int i = 0; i < buckets.Count; i++)
            {
                boids.AddRange(buckets[i].Boids);
            }

            foreach(Boid boid in boids)
            {
                float d = Vector3.Distance(transform.position, boid.transform.position);

                if (d < perceptionRadius && boid != this)
                {
                    // Alignment
                    flockingForces.alignment += boid.rigidbody.velocity;

                    // Cohesion
                    flockingForces.cohesion += boid.rigidbody.position;

                    // Separation
                    Vector3 diff = rigidbody.position - boid.rigidbody.position;
                    diff /= d;
                    flockingForces.separation += diff;

                    total++;
                }
            }

            if(total > 0)
            {
                // Alignment
                flockingForces.alignment /= total;
                flockingForces.alignment = flockingForces.alignment.normalized * maxSpeed;
                flockingForces.alignment -= rigidbody.velocity;
                flockingForces.alignment = flockingForces.alignment.Limit(maxForce);

                // Cohesion
                flockingForces.cohesion /= total;
                flockingForces.cohesion -= rigidbody.position;
                flockingForces.cohesion = flockingForces.cohesion.normalized * maxSpeed;
                flockingForces.cohesion -= rigidbody.velocity;
                flockingForces.cohesion = flockingForces.cohesion.Limit(maxForce);

                // Separation
                flockingForces.separation /= total;
                flockingForces.separation = flockingForces.separation.normalized * maxSpeed;
                flockingForces.separation -= rigidbody.velocity;
                flockingForces.separation = flockingForces.separation.Limit(maxForce);
            }

            if (forcesApplied)
            {
                flockingForces.alignment *= FlockingManager.Instance.ControlSliders.AlignmentSlider.value;
                flockingForces.cohesion *= FlockingManager.Instance.ControlSliders.CohesionSlider.value;
                flockingForces.separation *= FlockingManager.Instance.ControlSliders.SeparationSlider.value;
                forcesApplied = false;
            }
        }

        public void ApplyForces()
        {
            if (flockingForces.alignment.magnitude > 10)
                Debug.LogError($"High alignment magnitude: {flockingForces.alignment.magnitude}, for {name}");
                
            if(flockingForces.cohesion.magnitude > 10)
                Debug.LogError($"High cohesion magnitude: {flockingForces.cohesion.magnitude}, for {name}");

            if (flockingForces.separation.magnitude > 10)
                Debug.LogError($"High separation magnitude: {flockingForces.separation.magnitude}, for {name}");

            rigidbody.AddForce(flockingForces.alignment);
            rigidbody.AddForce(flockingForces.cohesion);
            rigidbody.AddForce(flockingForces.separation);

            forcesApplied = true;
        }

        public void Edges()
        {
            Vector3 pos = transform.position;

            for(int i = 0; i < 3; i++)
            {
                if (pos[i] > GameManager.Instance.Size[i])
                {
                    pos[i] = 0;
                }
                else if (pos[i] < 0)
                {
                    pos[i] = GameManager.Instance.Size[i];
                }
            }

            transform.position = pos;
        }
        #endregion
        #region Protected

        #endregion
        #region Private
        private int GetRandom()
        {
            int random = Random.Range(0, 2);
            if (random == 0)
                return -1;
            else
                return random;
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
