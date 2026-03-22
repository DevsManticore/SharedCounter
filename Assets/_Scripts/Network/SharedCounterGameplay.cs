using Mirror;
using TMPro;
using UnityEngine;

namespace SharedCounter.Network
{
    public class SharedCounterGameplay : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnValueChanged))]
        public int Value;

        [SerializeField] private TMP_Text counterText;

        public override void OnStartClient()
        {
            base.OnStartClient();
            UpdateUI();
        }

        [Command(requiresAuthority = false)]
        public void CmdIncrease()
        {
            Value++;
        }

        [Command(requiresAuthority = false)]
        public void CmdDecrease()
        {
            Value--;
        }

        private void OnValueChanged(int oldValue, int newValue)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (counterText != null)
                counterText.text = Value.ToString();
        }
    }
}