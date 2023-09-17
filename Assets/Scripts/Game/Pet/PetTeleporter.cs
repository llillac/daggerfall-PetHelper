using DaggerfallWorkshop.Game;
using UnityEngine;

namespace Game.Pet
{
    public class PetTeleporter : MonoBehaviour
    {
        [SerializeField] private PetSenses petSenses;
        [SerializeField] private float maxDistance;

        private bool IsTooFarFromPlayer =>
            Vector3.Distance(transform.position, GameManager.Instance.PlayerObject.transform.position) >
            maxDistance || petSenses.DistanceToTarget > maxDistance;

        private void FixedUpdate()
        {
            if (GameManager.IsGamePaused)
                return;

            if (IsTooFarFromPlayer)
                transform.position = GameManager.Instance.PlayerObject.transform.position;
        }
    }
}