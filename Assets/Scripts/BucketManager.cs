using nl.DTT.KVA.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*	Author  - Ferri de Lange
*	Date	- 17.05.20
*/

namespace FlockingSimulator.Buckets
{

    ///<summary>
    /// Keeps track of all the buckets
    ///</summary>
    public class BucketManager : MonoBehaviour
    {
        #region Variables
        #region Editor
        /// <summary>
        /// Resolution of the amount of buckets (1 -> 1, 2 -> 8, 3 -> 27, etc.)
        /// </summary>
        [SerializeField, Tooltip("Resolution of the amount of buckets (1 -> 1, 2 -> 8, 3 -> 27, etc.)")]
        private Vector3Int resolution;

        /// <summary>
        /// Prefab of what bucket to instantiate
        /// </summary>
        [SerializeField, Tooltip("Prefab of what bucket to instantiate")]
        private Bucket bucketPrefab;

        /// <summary>
        /// Reference to the Flocking Manager
        /// </summary>
        [SerializeField, Tooltip("Reference to the Flocking Manager")]
        private FlockingManager flockingManager;
        #endregion
        #region Public

        #endregion
        #region Private
        /// <summary>
        /// All the buckets in a 3d array for intuitive access
        /// </summary>
        private Bucket[,,] buckets;
        #endregion
        #endregion
        #region Methods
        #region Unity
        /// <summary>
        /// Setsup all the buckets
        /// </summary>
        private void Start()
        {
            buckets = new Bucket[resolution.x, resolution.y, resolution.z];
            var fieldSize = GameManager.Instance.FieldSize;

            for (int i = 0; i < resolution.x; i++)
            {
                for(int j = 0; j < resolution.y; j++)
                {
                    for(int k = 0; k < resolution.z; k++)
                    {
                        var bucket = buckets[i, j, k] = Instantiate(bucketPrefab, transform);
                        bucket.Size = new Vector3(fieldSize.x / resolution.x, fieldSize.y / resolution.y, fieldSize.z / resolution.z);
                        bucket.transform.localPosition = new Vector3(bucket.Size.x * i, bucket.Size.y * j, bucket.Size.z * k);
                    }
                }
            }
        }
        #endregion
        #region Public

        #endregion
        #region Protected

        #endregion
        #region Private
        #endregion
        #endregion
    }
}
