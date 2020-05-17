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
    public class Bucket : MonoBehaviour
    {
        #region Variables
        #region Editor

        #endregion
        #region Public
        public Vector3 Size
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }
        #endregion
        #region Private

        #endregion
        #endregion
        #region Methods
        #region Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + Size / 2, Size); 
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
