﻿using App2Night.Model.Model;
using MvvmNano;

namespace App2Night.ViewModel
{
    public class PartyViewModel : MvvmNanoViewModel<Party>
    {
        public Party Party { get; private set; }

        public override void Initialize(Party pParty)
        {
            base.Initialize(pParty);
            Party = pParty;
        }
    }
}
