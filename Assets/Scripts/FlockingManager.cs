using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
*	Author  - Ferri de Lange
*	Date	- 17.05.20
*/

namespace FlockingSimulator
{

    ///<summary>
    /// Manages all the boids
    ///</summary>
    public class FlockingManager : MonoBehaviour
    {
        #region Variables
        #region Editor
        /// <summary>
        /// The amount of boids that will be spawned at the start of the simulation
        /// </summary>
        [SerializeField, Tooltip("The amount of boids that will be spawned at the start of the simulation")]
        private int boidAmount;

        /// <summary>
        /// The type of boid to be used in the simulation
        /// </summary>
        [SerializeField, Tooltip("The type of boid to be used in the simulation")]
        private Boid boidPrefab;

        /// <summary>
        /// The sliders to control the flock behaviour
        /// </summary>
        [SerializeField, Tooltip("The sliders to control the flock behaviour")]
        private ControlSliders controlSliders;

        #endregion
        #region Public
        /// <summary>
        /// Every boid in the simulation
        /// </summary>
        public List<Boid> Boids { get; private set; }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static FlockingManager Instance { get; private set; }

        /// <summary>
        /// Public accessor for the control sliders
        /// <para>The sliders to control the flock behaviour</para>
        /// </summary>
        public ControlSliders ControlSliders => controlSliders;
        #endregion
        #region Private
        #endregion
        #endregion
        #region Methods
        #region Unity
        /// <summary>
        /// Setup singleton instance
        /// </summary>
        private void Awake()
        {
            if(Instance != this && Instance == null)
            {
                Instance = this;
            } else
            {
                Debug.LogError("Found more than instance of FlockManager Singleton", this);
            }
        }

        /// <summary>
        /// Instantiates all the boids 
        /// </summary>
        private void Start()
        {
            Boids = new List<Boid>();

            for(int i = 0; i < boidAmount; i++)
            {
                var clone = Instantiate(boidPrefab, transform);
                clone.name = $"Boid: {i}";
                Boids.Add(clone);
            }
        }

        /// <summary>
        /// Updates the flock behaviour for every boid
        /// </summary>
        private void Update()
        {
            foreach (Boid boid in Boids)
            {
                boid.Flock();
                boid.Edges();
            }

            foreach (Boid boid in Boids)
            {
                boid.ApplyForces();
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

    /// <summary>
    /// Data struct for the different flock behaviour sliders
    /// </summary>
    [Serializable]
    public struct ControlSliders
    {
        [SerializeField]
        private Slider alignmentSlider;

        [SerializeField]
        private Slider cohesionSlider;

        [SerializeField]
        private Slider separationSlider;

        public Slider AlignmentSlider => cohesionSlider;

        public Slider CohesionSlider => cohesionSlider;

        public Slider SeparationSlider => separationSlider;
    }
}
