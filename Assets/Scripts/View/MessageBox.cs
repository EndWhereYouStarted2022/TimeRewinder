using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class MessageBox : MonoBehaviour
{
    [SerializeField] private Button btnOK;
    [SerializeField] private Button btnCancel;
    [SerializeField] private Text txtTip;
    [SerializeField] private Text txtCountDown;
    private int _countDownTime;
    private UnityAction _OnBtnOk;
    private UnityAction _OnBtnCancel;
    void Start()
    {
        btnOK.onClick.AddListener(() =>
        {
            _OnBtnOk?.Invoke();
        });
        btnCancel.onClick.AddListener(() =>
        {
            _OnBtnCancel?.Invoke();
        });
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetText(string tip)
    {
        txtTip.text = tip;
    }

    public void SetCountDown(int seconds)
    {
        _countDownTime = seconds;
        if (seconds > 0)
        {
            StartCoroutine("CountDown");
        }
        else
        {
            txtCountDown.gameObject.SetActive(false);
        }
    }

    public void SetDelegate(UnityAction onOK, UnityAction onCancel)
    {
        _OnBtnOk = null;
        _OnBtnOk += onOK;
        _OnBtnCancel = null;
        _OnBtnCancel += onCancel;
    }
    IEnumerator CountDown()
    {
        while (_countDownTime > 0)
        {
            txtCountDown.text = _countDownTime.ToString();
            yield return new WaitForSeconds(1);
            _countDownTime = _countDownTime - 1;
        }

        _OnBtnOk?.Invoke();
        UIMgr.Instance.CloseMsgBox();
    }
}
