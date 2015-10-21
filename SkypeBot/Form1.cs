using System;
using SKYPE4COMLib;
using System.Windows.Forms;

namespace SkypeBot
{
    public partial class Form1 : Form
    {
        Skype skype;
        String trigger = "!";
        String nickname = "//JACKBOT\\\\ ";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            skype = new Skype();
            skype.Attach(7, false);
            skype.MessageStatus += new _ISkypeEvents_MessageStatusEventHandler(skype_MessageStatus);
        }

        private void skype_MessageStatus(ChatMessage pMessage, TChatMessageStatus Status)
        {
            if (Status == TChatMessageStatus.cmsReceived || Status == TChatMessageStatus.cmsSent)
            {
                String msg = pMessage.Body;
                Chat c = pMessage.Chat;

                /*if (pMessage.Sender.Handle == "eeshwan")
                {
                    c.SendMessage(nickname + "Memer detected...");
                } else if (pMessage.Sender.Handle == "dalton.f7")
                {
                    c.SendMessage(nickname + "ooh a message from dalton-sama");
                } else if (pMessage.Sender.Handle == "dream_3ater")
                {
                    c.SendMessage(nickname + "the creator...");
                }*/

                if (msg.StartsWith(trigger))
                {
                    lstBox.Items.Add(DateTime.Now.ToLongTimeString() + ":" + "command " + "'" + msg + "'" + " from " + pMessage.Sender.Handle);
                    msg = msg.Remove(0, 1).ToLower();

                    if (msg == "jackbot")
                    {
                        c.SendMessage(nickname + "Hello my good friend. Hope you have a jooday.");
                    }else if (msg == "meme")
                    {
                        c.SendMessage(nickname + "Meme I'm enjoying lately: https://www.youtube.com/watch?v=fK1N_vqJPac");
                    }else if (msg == "dnd")
                    {
                        c.SendMessage(nickname + "D&D is scheduled for Saturday October 24th, sometime around 11pm.");
                    } else
                    {
                        c.SendMessage(nickname + "I'm not sure what you want from me...");
                    }
                }
            }
        }
    }
}
