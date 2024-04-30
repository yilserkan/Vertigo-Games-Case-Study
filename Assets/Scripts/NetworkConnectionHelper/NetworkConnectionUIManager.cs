using UnityEngine;

namespace CardGame.NetworkConnection
{
    public class NetworkConnectionUIManager : MonoBehaviour, Utils.IObserver<bool>
    {
        [SerializeField] private GameObject _parent;
        
        public void Notify(bool hasConnection)
        {
            EnablePanel(!hasConnection);
        }
    
        private void EnablePanel(bool enable)
        {
            _parent.SetActive(enable);
        }
    }
}