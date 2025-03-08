using System;
using recipes;
using rhythm;
using UnityEngine;

namespace navigation
{
    class ResourcePlace : NavigationPlace
    {
        [SerializeField] private RhythmComponent rhythmComponent;
        [SerializeField] private RecipeType recipeType;


        private void Start()
        {
            rhythmComponent.Init(recipeType);
        }

        public override void LeavePlace()
        {
            base.LeavePlace();
            rhythmComponent.Stop();
        }
    }
}