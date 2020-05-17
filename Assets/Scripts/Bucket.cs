using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/*
*	Author  - Ferri de Lange
*	Date	- 17.05.20
*/

namespace FlockingSimulator.Buckets
{

    ///<summary>
    ///Class Description
    ///</summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Bucket : MonoBehaviour
    {
        #region Variables
        #region Editor

        #endregion
        #region Public
        public Vector3 Size
        {
            get => transform.localScale;
            set
            {
                transform.localScale = value;
            }
        }

        public BoxCollider Collider { get; set; }

        public List<Boid> Boids => boids;
        #endregion
        #region Private
        private List<Boid> boids;
        #endregion
        #endregion
        #region Methods
        #region Unity
        private void Awake()
        {
            boids = new List<Boid>();
            Collider = GetComponent<BoxCollider>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            for(int i = 0; i < boids.Count; i++)
            {
                Gizmos.DrawLine(transform.localPosition, boids[i].transform.localPosition);
            }
        }
        #endregion
        #region Public
        public bool IsInBucket(Boid boid)
        {
            bool insideX = false;
            bool insideY = false;
            bool insideZ = false;

            insideX = boid.transform.localPosition.x < transform.localScale.x + transform.localPosition.x &&
                boid.transform.localPosition.x > transform.localPosition.x;

            insideY = boid.transform.localPosition.y < transform.localScale.y + transform.localPosition.y &&
                boid.transform.localPosition.y > transform.localPosition.y;

            insideZ = boid.transform.localPosition.z < transform.localScale.z + transform.localPosition.z &&
                boid.transform.localPosition.z > transform.localPosition.z;


            return insideX && insideY && insideZ;
        }

        private void OnTriggerEnter(Collider other)
        {
            Boid boid;
            if(other.TryGetComponent<Boid>(out boid))
            {
                boid.Bucket = this;
                if (!boids.Contains(boid))
                    boids.Add(boid);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Boid boid;
            if (other.TryGetComponent<Boid>(out boid))
            {
                if (boids.Contains(boid))
                    boids.Remove(boid);
            }
        }

        public void AddBoid(Boid boid)
        {
            if (boids.Contains(boid))
                return;

            boids.Add(boid);
            boid.Bucket = this;
        }

        public void RemoveBoid(Boid boid)
        {
            if (!boids.Contains(boid))
                return;

            boids.Remove(boid);
        }
        #endregion
        #region Protected

        #endregion
        #region Private

        #endregion
        #endregion
    }
}
