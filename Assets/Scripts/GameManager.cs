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
    ///Class Description
    ///</summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables
        #region Editor
        [SerializeField]
        private Vector3 size = new Vector3(50, 50, 50);
        #endregion
        #region Public
        public Vector3 Size => size;

        public static GameManager Instance;
        #endregion
        #region Private

        #endregion
        #endregion
        #region Methods
        #region Unity
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + size / 2, size);
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
