using UnityEngine;
using UnityEngine.UI;
using Utils = uMoba.Utils;
public class UIChat : MonoBehaviour {
    public InputField messageInput;
    [SerializeField] Button sendButton;
    [SerializeField] Transform content;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject textPrefab;
    [SerializeField] KeyCode[] activationKeys = {KeyCode.Return, KeyCode.KeypadEnter};

    void Start() {
        // scrolling makes content visible
        scrollbar.onValueChanged.AddListener((val) => {
            ReshowMessages();
        });
    }

    void Update() {
        var player = Utils.ClientLocalPlayer();
        if (!player) return;

        // character limit
        var chat = player.GetComponent<PlayerChat>();
        messageInput.characterLimit = chat.maxLength;

        // activation        
        if (Utils.AnyKeyUp(activationKeys)) messageInput.Select();

        // end edit listener
        messageInput.onEndEdit.SetListener((value) => {
            // submit key pressed?
            if (Utils.AnyKeyDown(activationKeys)) {
                // submit
                var newinput = chat.OnSubmit(value);

                // set new input text
                messageInput.text = newinput;
                messageInput.MoveTextEnd(false);
            }

            // unfocus the whole chat in any case. otherwise we would scroll or
            // activate the chat window when doing wsad movement afterwards
            UIUtils.DeselectCarefully();
        });

        // send button
        sendButton.onClick.SetListener(() => {
            // submit
            var newinput = chat.OnSubmit(messageInput.text);

            // set new input text
            messageInput.text = newinput;
            messageInput.MoveTextEnd(false);

            // unfocus the whole chat in any case. otherwise we would scroll or
            // activate the chat window when doing wsad movement afterwards
            UIUtils.DeselectCarefully();
        });
    }

    void AutoScroll() {
        // value + velocity works best
        scrollbar.value = 0f;
        scrollRect.velocity = Vector2.one * 1000;
    }

    public void AddMessage(MessageInfo msg) {
        // reshow all previous messages (without the new one yet, it will be
        // shown by default)
        ReshowMessages();

        // instantiate the text
        var g = (GameObject)Instantiate(textPrefab);

        // set parent to Content object
        g.transform.SetParent(content.transform, false);

        // set text and color
        g.GetComponent<Text>().text = msg.content;
        g.GetComponent<Text>().color = msg.color;

        // TODO set sender for click reply
        //g.GetComponent<ChatTextEntry>().sender = sender;

        AutoScroll();
    }

    void ReshowMessages() {
        foreach (var fa in GetComponentsInChildren<UIFadeAlpha>())
            fa.ShowAndFade();
    }
}
