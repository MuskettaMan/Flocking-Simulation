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
    ///Class Description
    ///</summary>
    public class FlockingManager : MonoBehaviour
    {
        #region Variables
        #region Editor
        [SerializeField]
        private int boidAmount;

        [SerializeField]
        private Boid boidPrefab;

        [SerializeField]
        private ControlSliders controlSliders;

        #endregion
        #region Public
        public List<Boid> Boids { get; private set; }

        public static FlockingManager Instance { get; private set; }

        public ControlSliders ControlSliders => controlSliders;
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
                Debug.LogError("Found more than instance of FlockManager Singleton", this);
            }
        }

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

    [Serializable]
    public struct ControlSliders
    {
        [SerializeField]
        private Slider alignmentSlider;

        [SerializeField]
        private Slider cohesionSlider;

        [SerializeField]
        private Slider separationSlider;

        public Slider AlignmentSlider
        {
            get => alignmentSlider;
            private set => alignmentSlider = value;
        }

        public Slider CohesionSlider
        {
            get => cohesionSlider;
            private set => cohesionSlider = value;
        }

        public Slider SeparationSlider
        {
            get => separationSlider;
            private set => separationSlider = value;
        }
    }
}
