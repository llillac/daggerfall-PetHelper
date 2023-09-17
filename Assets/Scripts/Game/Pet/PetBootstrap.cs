using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using UnityEngine;

namespace Game.Pet
{
    public class PetBootstrap : MonoBehaviour
    {
        [SerializeField] private MobileTypes petType;
        [SerializeField] private GameObject petPrefab;

        private PetFactory _petFactory;
        private GameObject _petObject;

        private void Awake()
        {
            PlayerEnterExit.OnRespawnerComplete += OnPlayerRespawned;
            _petFactory = new PetFactory(petPrefab);
        }

        private void OnPlayerRespawned()
        {
            PlayerEnterExit.OnRespawnerComplete -= OnPlayerRespawned;
            Spawn();
        }

        private void Spawn()
        {
            if (_petObject != null) return;

            _petObject = _petFactory.Instantiate(petType, GameManager.Instance.PlayerObject.transform.position);
        }
    }
}