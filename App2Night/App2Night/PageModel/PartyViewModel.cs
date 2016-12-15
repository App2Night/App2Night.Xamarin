using App2Night.Model.Model;
using FreshMvvm;

namespace App2Night.PageModel
{
    public class PartyViewModel : FreshBasePageModel
    {
        public Party Party { get; private set; }

        public override void Init(object initData)
        {
            base.Init(initData);
            Party = (Party) initData;
        }
    }
}
