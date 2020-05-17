using FlockingSimulator.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
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
        #endregion
        #region Private
        public float PerceptionRadius { 
            get => perceptionRadius; 
            private set => perceptionRadius = value; 
        }

        private List<Boid> boids;

        private FlockingForces flockingForces;
        #endregion
        #endregion
        #region Methods
        #region Unity

        private void Start()
        {
            boids = FlockingManager.Instance.Boids;
            rigidbody = GetComponent<Rigidbody>();

            var size = FlockingManager.Size;
            transform.position = new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
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
            int total = 0;

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

            flockingForces.alignment *= FlockingManager.Instance.ControlSliders.AlignmentSlider.value;
            flockingForces.cohesion *= FlockingManager.Instance.ControlSliders.CohesionSlider.value;
            flockingForces.separation *= FlockingManager.Instance.ControlSliders.SeparationSlider.value;
        }

        public void ApplyForces()
        {
            rigidbody.AddForce(flockingForces.alignment);
            rigidbody.AddForce(flockingForces.cohesion);
            rigidbody.AddForce(flockingForces.separation);
        }

        public void Edges()
        {
            Vector3 pos = transform.position;

            for(int i = 0; i < 3; i++)
            {
                if (pos[i] > FlockingManager.Size[i] / 2)
                {
                    pos[i] = -FlockingManager.Size[i] / 2;
                }
                else if (pos[i] < -FlockingManager.Size[i] / 2)
                {
                    pos[i] = FlockingManager.Size[i] / 2;
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
    }
}
