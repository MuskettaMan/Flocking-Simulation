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
    /// Keeps track of the contents inside its collider box
    ///</summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Bucket : MonoBehaviour
    {
        #region Variables
        #region Editor

        #endregion
        #region Public
        /// <summary>
        /// Size of the Bucket
        /// </summary>
        public Vector3 Size
        {
            get => transform.localScale;
            set
            {
                transform.localScale = value;
            }
        }

        /// <summary>
        /// Box collider surrounding the bucket
        /// </summary>
        public BoxCollider Collider { get; set; }

        /// <summary>
        /// All the boids currently in the bucket
        /// </summary>
        public List<Boid> Boids { get; set; }
        #endregion
        #region Private
        #endregion
        #endregion
        #region Methods
        #region Unity
        /// <summary>
        /// Instantiation
        /// </summary>
        private void Awake()
        {
            Boids = new List<Boid>();
            Collider = GetComponent<BoxCollider>();
        }

        /// <summary>
        /// Debug info
        /// </summary>
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
        /// <summary>
        /// When a bucket enters it's colliders it gets added to the bucket
        /// </summary>
        /// <param name="other"></param>
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

        /// <summary>
        /// When a bucket exits the bucket's collider it gets removed from the bucket
        /// </summary>
        /// <param name="other"></param>
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
