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

        public List<Boid> Boids { get; set; }
        #endregion
        #region Private
        #endregion
        #endregion
        #region Methods
        #region Unity
        private void Awake()
        {
            Boids = new List<Boid>();
            Collider = GetComponent<BoxCollider>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            for(int i = 0; i < Boids.Count; i++)
            {
                Gizmos.DrawLine(transform.localPosition, Boids[i].transform.localPosition);
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
                if (!Boids.Contains(boid))
                    Boids.Add(boid);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Boid boid;
            if (other.TryGetComponent<Boid>(out boid))
            {
                if (Boids.Contains(boid))
                    Boids.Remove(boid);
            }
        }
        #endregion
        #region Protected

        #endregion
        #region Private

        #endregion
        #endregion
    }
}
