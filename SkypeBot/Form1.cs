using System;
using System.Collections;
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
        string urlTrigger = "http";
        string admin = "dream_3ater"; // change value here for admin rights

        Random rand = new Random();
        Boolean RPS = false;
        Boolean win = false;
        Boolean loss = false;

        StreamWriter linksWriter;
        StreamWriter scoreWriter;
        StreamReader scoreReader;
        StreamReader linksReader;

        Dictionary<string, int> scoreboard = new Dictionary<string, int>();
        ArrayList links = new ArrayList();

        public Form1()
        {
            if (File.Exists("C:\\Scoreboard.txt")) {
                scoreReader = new StreamReader("C:\\Scoreboard.txt");
                try
                {
                    do
                    {
                        string tmp = scoreReader.ReadLine();
                        string[] words = tmp.Split(' ');
                        string username = words[0];
                        int score = Int32.Parse(words[1]);
                        scoreboard.Add(username, score);
                    }
                    while (scoreReader.Peek() != -1);
                }
                catch
                {
                    // empty
                }
                finally
                {
                    scoreReader.Close();
                }
            }
            if (File.Exists("C:\\Links.txt"))
            {
                linksReader = new StreamReader("C:\\Links.txt");
                try
                {
                    do
                    {
                        string tmp = linksReader.ReadLine();
                        links.Add(tmp);
                    }
                    while (linksReader.Peek() != -1);
                }
                catch
                {
                    // empty
                }
                finally
                {
                    linksReader.Close();
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
                        c.SendMessage("Hello my good friend. Hope you have a jooday.");
                    } else if (msg == "meme") {
                        c.SendMessage("Meme I'm enjoying lately: https://www.youtube.com/watch?v=fK1N_vqJPac");
                    } else if (msg == "dnd") {
                        c.SendMessage("D&D is scheduled for Sunday November 1st, sometime around 1pm.");
                    } else if (msg == "rock") {
                        RPS = true;
                        int num = rand.Next(0, 3); // 0 = rock; 1 = paper; 2 = scissors
                        if (num == 0)
                        {
                            c.SendMessage("I chose rock! We tie!");
                        } else if (num == 1)
                        {
                            c.SendMessage("I chose paper! I win! (-1 score)");
                            loss = true;
                        } else
                        {
                            c.SendMessage("I chose scissors! You win! (+1 score)");
                            win = true;
                        }
                    } else if (msg == "paper") {
                        RPS = true;
                        int num = rand.Next(0, 3); // 0 = rock; 1 = paper; 2 = scissors
                        if (num == 0)
                        {
                            c.SendMessage("I chose rock! You win! (+1 score)");
                            win = true;
                        }
                        else if (num == 1)
                        {
                            c.SendMessage("I chose paper! We tie!");
                        }
                        else
                        {
                            c.SendMessage("I chose scissors! I win! (-1 score)");
                            loss = true;
                        }
                    } else if (msg == "scissors") {
                        RPS = true;
                        int num = rand.Next(0, 3); // 0 = rock; 1 = paper; 2 = scissors
                        if (num == 0)
                        {
                            c.SendMessage("I chose rock! I win! (-1 score)");
                            loss = true;
                        }
                        else if (num == 1)
                        {
                            c.SendMessage("I chose paper! You win! (+1 score)");
                            win = true;
                        }
                        else
                        {
                            c.SendMessage("I chose scissors! We tie!");
                        }
                    } else if (msg == "help") {
                        c.SendMessage("Commands include !jackbot !meme !dnd !rock/paper/scissors !spam");
                    } else if (msg == "spam") {
                        c.SendMessage("Sorry for someone spamming me :( I promise it's not my fault...");
                    } else if (msg == "fk alec") {
                        c.SendMessage("Fk alec");
                    } else if (msg == "quit" && pMessage.Sender.Handle == admin) {
                        c.SendMessage("*beep boop* Exiting... (saving scores)");
                        scoreWriter = new StreamWriter("C:\\Scoreboard.txt");
                        foreach (KeyValuePair<string, int> pair in scoreboard)
                        {
                            scoreWriter.WriteLine(pair.Key + " " + pair.Value);
                        }
                        scoreWriter.Close();

                        linksWriter = new StreamWriter("C:\\Links.txt");
                        foreach (string s in links)
                        {
                            linksWriter.WriteLine(s);
                        }
                        linksWriter.Close();
                        System.Environment.Exit(1);
                    } else if (msg == "scores") {
                        string txtScores = "";
                        foreach (KeyValuePair<string, int> pair in scoreboard)
                        {
                            txtScores += (pair.Key + ": " + pair.Value + "\n");
                        }
                        c.SendMessage(txtScores);
                    } else if (msg == "links")
                    {
                        string concat = "";
                        foreach (string s in links)
                        {
                            concat = concat + s + "\n";
                        }
                        c.SendMessage(concat);
                    }
                    if (RPS == true)
                    {
                        if (!scoreboard.ContainsKey(pMessage.Sender.Handle))
                        {
                            scoreboard.Add(pMessage.Sender.Handle, 0);
                        }
                        if (win == true)
                        {
                            scoreboard[pMessage.Sender.Handle] += 1;
                        }
                        else if (loss == true)
                        {
                            scoreboard[pMessage.Sender.Handle] -= 1;
                            if (scoreboard[pMessage.Sender.Handle] == -1)
                            {
                                scoreboard[pMessage.Sender.Handle] = 0;
                            }
                        }
                        RPS = false;
                        win = false;
                        loss = false;
                    }
                }
                else if (msg.StartsWith(urlTrigger))
                {
                    if (pMessage.Sender.Handle != "jackles.bot")
                    {
                        links.Add(msg);
                    }
                }
            }
        }
    }
}
