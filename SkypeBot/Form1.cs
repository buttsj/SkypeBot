using System;
using System.Collections.Generic;
using SKYPE4COMLib;
using System.Windows.Forms;
using System.IO;

namespace SkypeBot
{
    public partial class Form1 : Form
    {
        Skype skype;
        string trigger = "!";
        string nickname = "";
        Random rand = new Random();
        string winDir = System.Environment.GetEnvironmentVariable("windir");
        Boolean scored = false;

        StreamWriter writer;
        StreamReader reader;

        Dictionary<string, int> scoreboard = new Dictionary<string, int>();

        public Form1()
        {
            if (File.Exists("C:\\Scoreboard.txt")) {
                reader = new StreamReader("C:\\Scoreboard.txt");
                try
                {
                    do
                    {
                        string tmp = reader.ReadLine();
                        string[] words = tmp.Split(' ');
                        string username = words[0];
                        int score = Int32.Parse(words[1]);
                        scoreboard.Add(username, score);
                    }
                    while (reader.Peek() != -1);
                }
                catch
                {
                    // empty
                }
                finally
                {
                    reader.Close();
                }
            } 
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
                string msg = pMessage.Body;
                Chat c = pMessage.Chat;

                if (msg.StartsWith(trigger))
                {
                    lstBox.Items.Add(DateTime.Now.ToLongTimeString() + ":" + "command " + "'" + msg + "'" + " from " + pMessage.Sender.Handle);
                    msg = msg.Remove(0, 1).ToLower();

                    if (msg == "jackbot") {
                        c.SendMessage(nickname + "Hello my good friend. Hope you have a jooday.");
                    } else if (msg == "meme") {
                        c.SendMessage(nickname + "Meme I'm enjoying lately: https://www.youtube.com/watch?v=fK1N_vqJPac");
                    } else if (msg == "dnd") {
                        c.SendMessage(nickname + "D&D is scheduled for Saturday October 24th, sometime around 11pm.");
                    } else if (msg == "rock") {
                        int num = rand.Next(0, 3); // 0 = rock; 1 = paper; 2 = scissors
                        if (num == 0)
                        {
                            c.SendMessage(nickname + "I chose rock! We tie!");
                        } else if (num == 1)
                        {
                            c.SendMessage(nickname + "I chose paper! I win!");
                        } else
                        {
                            c.SendMessage(nickname + "I chose scissors! You win!");
                            scored = true;
                        }
                    } else if (msg == "paper") {
                        int num = rand.Next(0, 3); // 0 = rock; 1 = paper; 2 = scissors
                        if (num == 0)
                        {
                            c.SendMessage(nickname + "I chose rock! You win!");
                            scored = true;
                        }
                        else if (num == 1)
                        {
                            c.SendMessage(nickname + "I chose paper! We tie!");
                        }
                        else
                        {
                            c.SendMessage(nickname + "I chose scissors! I win!");
                        }
                    } else if (msg == "scissors") {
                        int num = rand.Next(0, 3); // 0 = rock; 1 = paper; 2 = scissors
                        if (num == 0)
                        {
                            c.SendMessage(nickname + "I chose rock! I win!");
                        }
                        else if (num == 1)
                        {
                            c.SendMessage(nickname + "I chose paper! You win!");
                            scored = true;
                        }
                        else
                        {
                            c.SendMessage(nickname + "I chose scissors! We tie!");
                        }
                    } else if (msg == "help") {
                        c.SendMessage("Commands include !jackbot !meme !dnd !rock/paper/scissors");
                    } else if (msg == "quit") {
                        c.SendMessage("*beep boop* Exiting... (saving scores)");
                        writer = new StreamWriter("C:\\Scoreboard.txt");
                        foreach (KeyValuePair<string, int> pair in scoreboard)
                        {
                            writer.WriteLine(pair.Key + " " + pair.Value);
                        }
                        writer.Close();
                        System.Environment.Exit(1);
                    } else if (msg == "scores") {
                        string txtScores = "";
                        foreach (KeyValuePair<string, int> pair in scoreboard)
                        {
                            txtScores += (pair.Key + ": " + pair.Value + "\n");
                        }
                        c.SendMessage(txtScores);
                    }
                    if (scored == true)
                    {
                        if (scoreboard.ContainsKey(pMessage.Sender.Handle))
                        {
                            scoreboard[pMessage.Sender.Handle] += 1;
                        } else {
                            scoreboard.Add(pMessage.Sender.Handle, 1);
                        }
                        scored = false;
                    }
                }
            }
        }
    }
}
