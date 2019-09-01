using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class LoginManager : Singleton<LoginManager>
{
    public GameObject loginView;
    public GameObject RegisterView;
    public GameObject selfView;


    public InputField LogUserName;
    public InputField LogPassword;

    public InputField RigUserName;
    public InputField RigPassword;

    public string userName = "";
    public string password = "";

    public int nameLen = 5;
    public int pwdLen = 10;

  
    public void Login()
    {
        userName = LogUserName.text;
        password = LogPassword.text;
        if (!EnterCheck())
        {
            print("用户名或密码长度不符合要求");
            return;
        }
        print("玩家请求登陆-----登陆账号：" + userName + "登陆密码：" + password + "--------");
        ResetData();
        EnterGame();
    }

    public void Rigister()
    {
        userName = RigUserName.text;
        password = RigPassword.text;
        if (!EnterCheck())
        {
            print("用户名或密码长度不符合要求");
            return;
        }
        print("玩家请求注册-----注册账号：" + userName + "注册密码：" + password + "--------");
        ResetData();
        ChangeView(true);
    }

    public void EnterGame()
    {
        selfView.SetActive(false);
    }

    public void ChangeView(bool isLogin)
    {
        loginView.SetActive(isLogin);
        RegisterView.SetActive(!isLogin);
    }

    bool EnterCheck()
    {
        print(userName.Length + "++++" + password.Length);
        if (userName.Length < nameLen || password.Length < pwdLen)
        {
            return false;
        }
        return true;
    }

    public void ResetData()
    {
        userName = "";
        password = "";
    }
}
