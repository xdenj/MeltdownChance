using Unity.Netcode;

namespace MeltdownChance
{
    internal class MeltdownChanceBehaviour : NetworkBehaviour
    {
        public static MeltdownChanceBehaviour? Instance { get; private set; }
        private NetworkVariable<bool>? _isMeltdown;
        public bool IsMeltdown
        {
            get => _isMeltdown.Value;
            internal set => _isMeltdown.Value = value;
        }


        public override void OnNetworkSpawn()
        {
            _isMeltdown = new NetworkVariable<bool>(false);
            if (Instance == null)
            {
                Instance = this;
            }
            MeltdownChanceBase.logger.LogInfo("Spawned MeltdownChanceBehaviour");
            base.OnNetworkSpawn();
        }

        public override void OnDestroy()
        {
            MeltdownChanceBase.logger.LogInfo("Destroyed MeltdownChanceBehaviour");
            if (ReferenceEquals(Instance, this))
            {
                Instance = null;
            }
            base.OnDestroy();
        }
    }
}
