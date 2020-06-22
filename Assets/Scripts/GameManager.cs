using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*	Author  - Ferri de Lange
*	Date	- 17.05.20
*/

namespace nl.DTT.KVA.Example
{

    ///<summary>
    /// Handles generic functionality for tying the game together
    ///</summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables
        #region Editor
        /// <summary>
        /// Size of the the field all the boids get spawned on
        /// </summary>
        [SerializeField, Tooltip("Size of the the field all the boids get spawned on")]
        private Vector3 fieldSize = new Vector3(50, 50, 50);
        #endregion
        #region Public
        /// <summary>
        /// Public accessor for field size
        /// <para>Size of the the field all the boids get spawned on</para>
        /// </summary>
        public Vector3 FieldSize => fieldSize;

        /// <summary>
        /// Singleton access
        /// </summary>
        public static GameManager Instance;
        #endregion
        #region Private
        #endregion
        #endregion
        #region Methods
        #region Unity
        /// <summary>
        /// Singleton setup
        /// </summary>
        private void Awake()
        {
            if(Instance != this && Instance == null)
            {
                Instance = this;
            } else
            {
                Debug.LogError("More than one GameManager Singleton", this);
            }
        }

        /// <summary>
        /// Debug information
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + fieldSize / 2, fieldSize);

            var startPos = FieldSize * 0.1f + transform.position;
            var endPos = FieldSize * 0.9f + transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(startPos, 0.5f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(endPos, 0.5f);
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
