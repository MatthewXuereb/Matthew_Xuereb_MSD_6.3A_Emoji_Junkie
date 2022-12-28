using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace EmojiJunkie.Dev
{
    public class HomePanelManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject _homePanel;
        [SerializeField] private GameObject _createLobbyPanel;
        [SerializeField] private GameObject _joinLobbyPanel;
        [SerializeField] public GameObject _waitingPanel;

        [Header("Firebase")]
        [SerializeField] private FirebaseController _firebaseController;

        [Header("UI Elements")]
        [SerializeField] private TMP_InputField _createRoomInputField;
        [SerializeField] private TMP_InputField _joinRoomInputField;
        [SerializeField] private TMP_InputField _player1InputField;
        [SerializeField] private TMP_InputField _player2InputField;

        private void Awake()
        {
            SetHomePanel();
        }

        public void SetHomePanel()
        {
            _homePanel.SetActive(true);
            _createLobbyPanel.SetActive(false);
            _joinLobbyPanel.SetActive(false);
            _waitingPanel.SetActive(false);
        }
        public void SetCreateLobbyPanel()
        {
            _homePanel.SetActive(false);
            _createLobbyPanel.SetActive(true);
            _joinLobbyPanel.SetActive(false);
            _waitingPanel.SetActive(false);
        }
        public void SetJoinLobbyPanel()
        {
            _homePanel.SetActive(false);
            _createLobbyPanel.SetActive(false);
            _joinLobbyPanel.SetActive(true);
            _waitingPanel.SetActive(false);
        }
        public void SetWaitingPanel()
        {
            _homePanel.SetActive(false);
            _createLobbyPanel.SetActive(false);
            _joinLobbyPanel.SetActive(false);
            _waitingPanel.SetActive(true);
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        public void CreateRoom()
        {
            _firebaseController.CreateRoom(_createRoomInputField.text, _player1InputField.text);
        }
        public void JoinRoom()
        {
            _firebaseController.JoinRoom(_joinRoomInputField.text, _player2InputField.text);
        }
    }
}
