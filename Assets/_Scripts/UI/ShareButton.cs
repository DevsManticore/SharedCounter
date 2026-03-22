using SharedCounter.WebGL;
using UnityEngine;

namespace SharedCounter.UI
{
    public class ShareButton : MonoBehaviour
    {
        public void OnClickShare()
        {
            string url = BuildJoinLink();
            WebGLBridge.Copy(url);
        }

        private string BuildJoinLink()
        {
            string baseUrl = Application.absoluteURL;
            string name = PlayerNameInput.DisplayName;

            if (!string.IsNullOrEmpty(name))
            {
                return $"{baseUrl}?name={name}";
            }

            return baseUrl;
        }
    }
}