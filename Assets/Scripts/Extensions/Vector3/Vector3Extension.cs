using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*	Author  - Ferri de Lange
*	Date	- 17.05.20
*/

namespace FlockingSimulator.Extensions
{

    ///<summary>
    ///Class Description
    ///</summary>
    public static class Vector3Extension
    {
        #region Methods
        #region Public
        public static Vector3 Limit(this Vector3 vector, float limit)
        {
            if(vector.magnitude > limit)
            {
                vector.Normalize();
                vector *= limit;
            }

            return vector;
        }
        #endregion
        #endregion
    }
}
