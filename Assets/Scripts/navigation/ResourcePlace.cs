using rhythm;
using UnityEngine;
using Zenject;

namespace navigation
{
    class ResourcePlace : NavigationPlace
    {
        [SerializeField] private Rhythm rhythm;

        [Inject] private RhythmController _rhythmController;

        public override void EnterPlace()
        {
            base.EnterPlace();
            _rhythmController.Stop();
            _rhythmController.currentRhythm = rhythm;
        }

        public override void LeavePlace()
        {
            base.LeavePlace();
            _rhythmController.Stop();
        }
    }
}