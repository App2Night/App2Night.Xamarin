using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App2Night.Model.Enum;

namespace App2Night.Model.Model
{
    public class PartyCommitmentParameter
    {
        public PartyCommitmentState CommitmentState { get; set; }
        public Party Party { get; set; }
    }
}
