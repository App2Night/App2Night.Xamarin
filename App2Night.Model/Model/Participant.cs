using App2Night.Model.Enum;

namespace App2Night.Model.Model
{
    public class Participant
    {
        public string UserName { get; set; }
        public string UserId { get; set; }

        public PartyCommitmentState UserCommitmentState { get; set; }
    }
}