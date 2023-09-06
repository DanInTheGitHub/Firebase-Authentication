using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.Tilemaps;

public class ButtSignUp : MonoBehaviour
{
    [SerializeField]
    private Button _registrationButton;
    private Coroutine _registrationCorutine;

    void Reset()
    {
        _registrationButton = GetComponent<Button>();
    }
    void Start()
    {
        _registrationButton.onClick.AddListener(HandleRegisterButtonClicked);
    }
    void HandleRegisterButtonClicked()
    {
        string email = GameObject.Find("InpitEmail").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("InpitPassword").GetComponent<TMP_InputField>().text;

        _registrationCorutine = StartCoroutine(RegisterUser(email, password));

    }

    private IEnumerator RegisterUser(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.IsCanceled)
        {
            Debug.LogError($"CreateUserWithEmailAndPasswordAsync is canceled");

        }
        else if (registerTask.IsFaulted)
        {
            Debug.LogError($"CreateUserWithEmailAndPasswordAsync encoutered error");
        }
        Firebase.Auth.AuthResult result = registerTask.Result;
        Debug.LogFormat("Firebase user created successfully: {0} ({1})",
            result.User.DisplayName, result.User.UserId);
    }
}