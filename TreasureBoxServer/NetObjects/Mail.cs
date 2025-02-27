using CSharpLike;
using System.Collections.Generic;



namespace TreasureBox
{
	public sealed class Mail : Mail_Base
    {
//#if FREE_VERSION//We recommend set `appendix` type in NotObject `Mail` as `Dictionary<int, int>` in C#Like, but must set as `string` in C#LikeFree.
        [KissJsonDontSerialize]
        Dictionary<int, int> mAppendix = null;
        [KissJsonDontSerialize]
        public Dictionary<int, int> Appendix
        {
            get
            {
                if (mAppendix == null)
                    mAppendix = NetObjectUtils.StringToDictionary<int, int>(appendix);
                return mAppendix;
            }
        }
//#endif
    }
}
