using CSharpLike;
using System.Collections.Generic;

namespace TreasureBox
{
	public sealed class Mail : Mail_Base
    {
        [KissJsonDontSerialize]
        Dictionary<int, int> mAppendix = null;
        [KissJsonDontSerialize]
        public Dictionary<int, int> Appendix
        {
            get
            {
                if (mAppendix == null)
                    mAppendix = Global.StringToDictionary(appendix);
                return mAppendix;
            }
        }
    }
}
