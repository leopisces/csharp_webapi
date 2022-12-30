using DataService.Shared.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DataService.Shared.Helpers
{
    /// <summary>
    /// 描述：发送邮件
    /// 作者：Leopisces
    /// 创建日期：2022/8/3 14:34:28
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    public class SMTPManager
    {
        private readonly EmailConfig _config;

        public SMTPManager(IOptions<EmailConfig> options)
        {
            _config = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public void SenEmail(string title, string msg)
        {
            //实例化一个发送邮件类。
            MailMessage mailMessage = new MailMessage();
            //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
            mailMessage.From = new MailAddress(_config.AddressFrom);
            //收件人邮箱地址。
            mailMessage.To.Add(new MailAddress(_config.AddressTo));
            //邮件标题。
            mailMessage.Subject = title;
            //邮件内容。
            mailMessage.Body = msg;

            //实例化一个SmtpClient类。
            SmtpClient client = new SmtpClient();
            //在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
            client.Host = _config.Host;
            //使用安全加密连接。
            client.EnableSsl = true;
            //不和请求一块发送。
            client.UseDefaultCredentials = false;
            //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
            client.Credentials = new NetworkCredential(_config.AddressFrom, _config.Password);
            //发送
            client.Send(mailMessage);
        }
    }
}
