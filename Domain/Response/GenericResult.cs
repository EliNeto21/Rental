using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class GenericResult<T>
    {
        public T Data { get; set; }

        public int Code { get; set; }

        public List<string> Mensagens { get; set; } = new List<string>();

        public string Mensagem => string.Join(Environment.NewLine, Mensagens);

        public GenericResult()
        {
        }

        public GenericResult(int statusCode, string msg, T data)
        {
            Code = statusCode;
            Mensagens = new List<string>();
            if (msg != null)
            {
                Mensagens.Add(msg);
            }

            Data = data;
        }
    }
}
