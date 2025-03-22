//using CM.Text;
//using CM.Text.BusinessMessaging;
//using CM.Text.BusinessMessaging.Model;
//using CM.Text.BusinessMessaging.Model.MultiChannel;


using Viber.Bot;
using Viber.Bot.NetCore.Models;

namespace ViberMailer;

public class Mailer
{
    private IViberBotClient _viberBotClient;

    private string _authToken;
    private string _webhookUrl;
    private string _adminId;

    public void Init()
    {

        _authToken = "5192d0382ea7e2e9-d35f8cb4f9a26339-3a3429d4a23b0c5f";//config["authToken"];
        _webhookUrl = "https://localhost:7150/Home/SendMessageResponce";//config["webhookUrl"];
        //_adminId = "";// config["adminId"];

        _viberBotClient = new ViberBotClient(_authToken);
        _viberBotClient.SetWebhookAsync(_webhookUrl);


        //var result = _viberBotClient.GetAccountInfoAsync();

        var btnSubscribe = new ViberKeyboardButton();
        btnSubscribe.Text = "Subscribe";


        //btnSubscribe.ActionType = KeyboardActionType.Reply;
        //btnSubscribe.ActionBody = "https://chatapi.viber.com/pa/set_webhook";


    }

    public async Task SendMessage(string message = "hello")
    {
        var result = await _viberBotClient.SendTextMessageAsync(new Viber.Bot.TextMessage
        {
            Receiver = "mGAYKUZzYVMsgpbt9uMCEQ==",
            Sender = new UserBase
            {
                Name = "SkillsHub"
            },
            Text = "messaggge",

        });




    }
}