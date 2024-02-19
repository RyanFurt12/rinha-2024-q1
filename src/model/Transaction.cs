using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rinha_2024_q1.model
{
    public class Transaction
    {
        public required int valor { get; set; }
        public required string tipo { get; set; }
        public required string descricao { get; set; }
        public DateTime realizada_em { get; } = DateTime.Now;
    }
}
