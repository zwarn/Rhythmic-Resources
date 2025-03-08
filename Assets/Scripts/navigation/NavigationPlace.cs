using UnityEngine;
using UnityEngine.Serialization;

namespace navigation
{
    public class NavigationPlace : MonoBehaviour
    {
        [SerializeField] private Transform content;

        public virtual void EnterPlace()
        {
            Show();
        }

        public virtual void LeavePlace()
        {
            Hide();
        }

        public void Show()
        {
            content.gameObject.SetActive(true);
        }

        public void Hide()
        {
            content.gameObject.SetActive(false);
        }
    }
}