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
    ///Class Description
    ///</summary>
    public class BucketManager : MonoBehaviour
    {
        #region Variables
        #region Editor
        [SerializeField]
        private int resolution;

        [SerializeField]
        private Bucket bucketPrefab;

        [SerializeField]
        private FlockingManager flockingManager;
        #endregion
        #region Public

        #endregion
        #region Private
        private Bucket[,,] buckets;
        private List<Boid> boids;
        #endregion
        #endregion
        #region Methods
        #region Unity
        private void Awake()
        {
            flockingManager.BoidsInstantiated += OnBoidsInstantiated;

            buckets = new Bucket[resolution, resolution, resolution];
            var fieldSize = GameManager.Instance.Size;

            for (int i = 0; i < resolution; i++)
            {
                for(int j = 0; j < resolution; j++)
                {
                    for(int k = 0; k < resolution; k++)
                    {
                        var bucket = buckets[i, j, k] = Instantiate(bucketPrefab, transform);
                        bucket.Size = fieldSize / resolution;
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
        private void OnBoidsInstantiated(List<Boid> boids)
        {
            this.boids = boids;
            StartCoroutine(SetInitialBuckets());
        }

        private IEnumerator SetInitialBuckets()
        {
            yield return new WaitForEndOfFrame(); 

            for (int i = 0; i < boids.Count; i++)
            {
                foreach (Bucket bucket in buckets)
                {
                    if (bucket.IsInBucket(boids[i]))
                    {
                        bucket.AddBoid(boids[i]);
                        break;
                    }
                }
            }
        }
        #endregion
        #endregion
    }
}
