using App2Night.Model.Model;
using FreshMvvm;

namespace App2Night.PageModel.SubPages
{
    public class PartyDetailViewModel : FreshBasePageModel 
    {
        public Party Party { get; set; }


        public override void Init(object initData)
        {
            base.Init(initData);
            Party = (Party) initData;
        }
    }
}