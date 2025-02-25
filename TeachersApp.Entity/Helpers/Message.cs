using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Helpers
{
    public class Message
    {
       
            public IEnumerable<string> To { get; }
            public string Subject { get; }
            public string Content { get; }
            public string? From { get; }

            public Message(IEnumerable<string> to, string subject, string content, string? from = null)
            {
                To = to;
                Subject = subject;
                Content = content;
                From = from;
            }
        }

    }
