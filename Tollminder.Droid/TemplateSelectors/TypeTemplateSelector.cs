using System;
using System.Collections.Generic;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Tollminder.Droid.TemplateSelectors
{
    public class TypeTemplateSelector : IMvxTemplateSelector
    {
        private readonly Dictionary<Type, int> modelToViewResourceMapping;

        public TypeTemplateSelector(Dictionary<Type, int> modelToViewResourceMapping)
        {
            this.modelToViewResourceMapping = modelToViewResourceMapping;
        }

        public int GetItemLayoutId(int fromViewType)
            => fromViewType;

        public int GetItemViewType(object forItemObject)
            => modelToViewResourceMapping[forItemObject.GetType()];
    }
}
