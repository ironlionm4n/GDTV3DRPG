using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class Cinematics : MonoBehaviour
    {
        private PlayableDirector _playableDirector;
        private bool _alreadyTriggered;
        private void Start()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!_alreadyTriggered)
                {
                    _alreadyTriggered = true;
                    _playableDirector.Play();
                }
            }
        }
    }
}