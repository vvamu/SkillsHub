

using Microsoft.AspNetCore.Mvc;
//using Viber.Bot;
using Viber.Bot.NetCore.Infrastructure;
using Viber.Bot.NetCore.Models;
using Viber.Bot.NetCore.RestApi;

public class ViberController : Controller
{
    private readonly IViberBotApi _viberBotApi;

    public ViberController(IViberBotApi viberBotApi)
    {
        _viberBotApi = viberBotApi;
    }

    // The service sets a webhook automatically, but if you want sets him manually then use this
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        //var response = await _viberBotApi.SetWebHookAsync(new ViberWebHook.WebHookRequest("YOUR_WEBHOOK_URL"));

        var msg = new ViberMessage.TextMessage();
        //msg.Text = "SUka";



        var response = await _viberBotApi.SendMessageAsync<ViberResponse.ViberApiResponseBase>(msg);

        if (response.Content.Status == ViberErrorCode.Ok)
        {
            return Ok("Viber-bot is active");
        }
        else
        {
            return BadRequest(response.Content.StatusMessage);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ViberCallbackData update)
    {
        //var str = String.Empty;

        //switch (update.Message.Type)
        //{
        //    case ViberMessageType.Text:
        //        {
        //            var mess = update.Message as ViberMessage.TextMessage;

        //            str = mess.Text;

        //            break;
        //        }

        //    default: break;
        //}

        var str = "heeello";

        // you should to control required fields
        var message = new ViberMessage.TextMessage()
        {
            //required
            Receiver = "mGAYKUZzYVMsgpbt9uMCEQ==",
            Sender = new ViberUser.User()
            {
                //required
                Name = "SkillsHub",
                //Avatar = "http://dl-media.viber.com/1/share/2/long/bots/generic-avatar%402x.png"
            },
            //required
            Text = str
        };

        // our bot returns incoming text
        var response = await _viberBotApi.SendMessageAsync<ViberResponse.SendMessageResponse>(message);

        return Ok();
    }
}