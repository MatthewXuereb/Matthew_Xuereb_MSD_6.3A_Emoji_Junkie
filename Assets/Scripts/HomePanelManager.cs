using UnityEngine;
using UnityEngine.SceneManagement;

namespace EmojiJunkie
{
    public class HomePanelManager : MonoBehaviour
    {
        [SerializeField] private GameObject _homePanel;
        [SerializeField] private GameObject _createLobbyPanel;
        [SerializeField] private GameObject _joinLobbyPanel;

        private void Awake()
        {
            SetHomePanel();
        }

        public void SetHomePanel()
        {
            _homePanel.SetActive(true);
            _createLobbyPanel.SetActive(false);
            _joinLobbyPanel.SetActive(false);
        }
        public void SetCreateLobbyPanel()
        {
            _homePanel.SetActive(false);
            _createLobbyPanel.SetActive(true);
            _joinLobbyPanel.SetActive(false);
        }
        public void SetJoinLobbyPanel()
        {
            _homePanel.SetActive(false);
            _createLobbyPanel.SetActive(false);
            _joinLobbyPanel.SetActive(true);
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}
