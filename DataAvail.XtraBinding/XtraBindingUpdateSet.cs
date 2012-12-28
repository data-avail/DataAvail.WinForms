using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBindings
{
    public class XtraBindingUpdateSet
    {
        public XtraBindingUpdateSet(IXtraBindingUpdateSetDataConverter UpdateSetDataConverter)
        {
            _updateSetDataConverter = UpdateSetDataConverter;
        }

        IXtraBindingUpdateSetDataConverter _updateSetDataConverter;

        List<XtraBindingUpdateSetEntity> _entyties = new List<XtraBindingUpdateSetEntity>();

        List<XtraBindingUpdateSetUpdateResult> _updateSet = new List<XtraBindingUpdateSetUpdateResult>();

        List<XtraBindingUpdateSetRejectResult> _rejectSet = new List<XtraBindingUpdateSetRejectResult>();

        public void Add(XtraBinding XtraBinding, object DataSourceItem, XtraBindingStoredItemState State)
        {
            object underlyingItem = XtraBinding.ItemAdapter.GetUnderlyingDataSourceItem(DataSourceItem);

            XtraBindingUpdateSetEntity enty = _entyties.FirstOrDefault(p => p.xtraBinding == XtraBinding);

            if (enty == null)
            {
                enty = new XtraBindingUpdateSetEntity(XtraBinding);

                _entyties.Add(enty);
            }

            XtraBindingStoredItem item = GetStoredItem(enty, underlyingItem);

            if (item == null)
            {
                item = new XtraBindingStoredItem();

                enty.StoredItems.Add(item);
            }

            item.Update(GetFieldsValues(XtraBinding, underlyingItem), State);

            item.UpdateDataSourceItem(DataSourceItem);
        }

        public void Clear(XtraBinding XtraBinding, IEnumerable<object> PKs)
        {
            XtraBindingUpdateSetEntity enty = _entyties.FirstOrDefault(p => p.xtraBinding == XtraBinding);

            if (enty != null)
            { 
                string key = GetPkFieldName(XtraBinding);

                foreach (XtraBindingStoredItem item in enty.StoredItems.Where(p => PKs.Contains(p.FieldsValues[key])).ToArray())
                {
                    enty.StoredItems.Remove(item);
                }

                if (enty.StoredItems.Count == 0)
                {
                    _entyties.Remove(enty);
                }
            }
        }

        public IEnumerable<XtraBindingUpdateSetUpdateResult> Update(XtraBinding XtraBinding, IEnumerable<object> PKs)
        {
            XtraBinding.xtraBindingOperation.BeginTransaction();

            _updateSet.Clear();

            try
            {
                OnUpdate(XtraBinding, PKs, null);
            }
            catch (System.Exception e)
            {
                XtraBinding.xtraBindingOperation.RollbackTransaction();

                throw e;
            }

            XtraBinding.xtraBindingOperation.CommitTransaction();

            return _updateSet;
        }

        public IEnumerable<XtraBindingUpdateSetRejectResult> Reject(XtraBinding XtraBinding, IEnumerable<object> PKs)
        {
            _rejectSet.Clear();

            OnReject(XtraBinding, PKs);

            return _rejectSet;
        }

        /// <summary>
        /// Get stored items only for XtraBinding
        /// </summary>
        internal IEnumerable<XtraBindingStoredItem> GetStoredItems(XtraBinding XtraBinding)
        {
            return _entyties.Where(p => p.xtraBinding == XtraBinding).SelectMany(p => p.StoredItems);
        }

        /// <summary>
        /// Get stored items for XtraBinding and his descendants
        /// </summary>
        internal IEnumerable<XtraBindingStoredItem> GetStoredItemsBatch(XtraBinding XtraBinding, IEnumerable<object> PKs)
        {
            List<XtraBindingStoredItem> items = new List<XtraBindingStoredItem>();

            IEnumerable<XtraBindingStoredItem> ims = _entyties.Where(p => p.xtraBinding == XtraBinding).SelectMany(p => p.StoredItems);

            if (PKs != null)
                ims = ims.Where(p => PKs.Contains(p.FieldsValues[GetPkFieldName(XtraBinding)]));

            items.AddRange(ims);

            if (XtraBinding is XtraBindingContainer)
            {
                foreach(XtraBindingChild childBinding in ((XtraBindingContainer)XtraBinding).Children)
                {
                    IEnumerable<object> childrenPKs = PKs == null ? null : GetChildrenPKS(childBinding, PKs);

                    items.AddRange(GetStoredItemsBatch(childBinding, childrenPKs));
                }
            }

            return items;
        }

        private IDictionary<string, object> GetFieldsValues(XtraBinding XtraBinding, object UnderlyingItem)
        {
            return GetFieldsValues(XtraBinding.DataSource, UnderlyingItem);
        }

         private IDictionary<string, object> GetFieldsValues(object DatSource, object UnderlyingItem)
        {
            return _updateSetDataConverter.GetFieldsNames(DatSource).ToDictionary(k => k, v => _updateSetDataConverter.GetValue(UnderlyingItem, v));
        }

        private XtraBindingStoredItem GetStoredItem(XtraBindingUpdateSetEntity UpdateSetEntity, object UnerlyingItem)
        {
            string key = _updateSetDataConverter.GetPrimaryKeyFieldName(UpdateSetEntity.xtraBinding.DataSource);

            return UpdateSetEntity.StoredItems.Where(p => p.FieldsValues[key].Equals(_updateSetDataConverter.GetValue(UnerlyingItem, key))).FirstOrDefault();
        }

        private void OnUpdate(XtraBinding XtraBinding, IEnumerable<object> PKs, IDictionary<object, object> ParentOldNewPKS)
        {
            XtraBindingUpdateSetEntity entry = _entyties.FirstOrDefault(p => p.xtraBinding == XtraBinding);

            IDictionary<object, object> oldNewPks = PKs == null ? null : PKs.ToDictionary(k => k, v => v);

            if (entry != null)
            {
                IEnumerable<object> pks = PKs ?? entry.StoredItems.Select(p => p.FieldsValues[GetPkFieldName(XtraBinding)]);

                IDictionary<XtraBindingStoredItem, object> items = GetUnderlyingObjects(entry, pks);

                if (ParentOldNewPKS != null)
                    SubstituteOldNewFKS(XtraBinding, items.Where(p=>p.Key.State != XtraBindingStoredItemState.Removed).Select(p=>p.Value), ParentOldNewPKS);

                XtraBinding.xtraBindingOperation.Update(items.Values);

                IEnumerable<object> newPKs = GetObjectsPKS(XtraBinding, items.Select(p=>p.Key.State != XtraBindingStoredItemState.Removed ? p.Value : null));

                oldNewPks = pks.Select((p, k) => new { p, k }).ToDictionary(k => k.p, v => newPKs.ElementAt(v.k));

                _updateSet.Add(new XtraBindingUpdateSetUpdateResult(XtraBinding, GetPkFieldName(XtraBinding), items.Keys, newPKs));

            }

            if (XtraBinding is XtraBindingContainer)
            {
                foreach (XtraBindingChild childBinding in ((XtraBindingContainer)XtraBinding).Children)
                {
                    IEnumerable<object> childrenPKs = PKs == null ? null : GetChildrenPKS(childBinding, PKs);

                    OnUpdate(childBinding, childrenPKs, oldNewPks);
                }
            }
        }

        private string GetPkFieldName(XtraBinding XtraBinding)
        {
            return _updateSetDataConverter.GetPrimaryKeyFieldName(XtraBinding.DataSource);
        }

        internal IEnumerable<object> GetObjectsPKS(XtraBinding XtraBinding, IEnumerable<object> UnderlyingObjects)
        {
            string key = GetPkFieldName(XtraBinding);

            return UnderlyingObjects.Select(p => p != null ? _updateSetDataConverter.GetValue(p, key) : null);
        }

        internal IEnumerable<object> GetStoredItemsPKS(XtraBinding XtraBinding, IEnumerable<XtraBindingStoredItem> StoredItems)
        {
            string key = GetPkFieldName(XtraBinding);

            return StoredItems.Select(p => p.FieldsValues[key]);
        }

        private void SubstituteOldNewFKS(XtraBinding XtraBinding, IEnumerable<object> UnderlyingObjects, IDictionary<object, object> ParentOldNewPKS)
        {
            if (XtraBinding is XtraBindingChild)
            {
                string fk = ((XtraBindingChild)XtraBinding).ChildProperties.fkFieldName;

                string pk = GetPkFieldName(XtraBinding);

                foreach (object underlyingItem in UnderlyingObjects)
                {
                    object oldVal = _updateSetDataConverter.GetValue(underlyingItem, fk);

                    if (ParentOldNewPKS.Keys.Contains(oldVal))
                    {
                        object newVal = ParentOldNewPKS[oldVal];

                        if (newVal != null && !newVal.Equals(oldVal))
                            _updateSetDataConverter.SetValue(underlyingItem, fk, newVal);
                    }
                }
            }
        }

        private IDictionary<XtraBindingStoredItem, object> GetUnderlyingObjects(XtraBindingUpdateSetEntity UpdateSetEntity, IEnumerable<object> PKS)
        {
            IEnumerable<XtraBindingStoredItem> storedItems =  UpdateSetEntity.StoredItems.Where(p => PKS.Contains(p.FieldsValues[GetPkFieldName(UpdateSetEntity.xtraBinding)]));

            return _updateSetDataConverter.GetUnderlyingObjects(UpdateSetEntity.xtraBinding.DataSource, storedItems, false).Select((p, i) => new { p = storedItems.ElementAt(i), s = p }).ToDictionary(k => k.p, v => v.s);
        }

        private IEnumerable<object> GetChildrenPKS(XtraBindingChild XtraBinding, IEnumerable<object> ParentPKS)
        {
            XtraBindingUpdateSetEntity entry = _entyties.FirstOrDefault(p => p.xtraBinding == XtraBinding);

            if (entry != null)
            {
                string fk = XtraBinding.ChildProperties.fkFieldName;

                string pk = GetPkFieldName(XtraBinding);

                return entry.StoredItems.Where(p=>ParentPKS.Contains(p.FieldsValues[fk])).Select(p => p.FieldsValues[pk]);
            }
            else
            {
                return null;
            }
        }

        private void OnReject(XtraBinding XtraBinding, IEnumerable<object> PKs)
        {
            if (XtraBinding is XtraBindingContainer)
            {
                foreach (XtraBindingChild childBinding in ((XtraBindingContainer)XtraBinding).Children)
                {
                    IEnumerable<object> childrenPKs = PKs == null ? null : GetChildrenPKS(childBinding, PKs);

                    OnReject(childBinding, childrenPKs);
                }
            }

            XtraBindingUpdateSetEntity enty = _entyties.FirstOrDefault(p => p.xtraBinding == XtraBinding);

            if (enty != null)
            {
                string key = GetPkFieldName(XtraBinding);

                IEnumerable<XtraBindingStoredItem> items = PKs == null ? enty.StoredItems : enty.StoredItems.Where(p => PKs.Contains(p.FieldsValues[key]));

                _rejectSet.Add(new XtraBindingUpdateSetRejectResult(XtraBinding, key, items.ToArray()));

                foreach (XtraBindingStoredItem item in items.ToArray())
                {
                    enty.StoredItems.Remove(item);
                }

                if (enty.StoredItems.Count == 0)
                {
                    _entyties.Remove(enty);
                }
            }
        }

        /// <summary>
        /// Update values of the items which contain XtraBinding's data source by the corresonding values from the stored items.
        /// </summary>
        internal void RefreshItems(XtraBinding XtraBinding)
        { 
            string key = GetPkFieldName(XtraBinding);

            IEnumerable<XtraBindingStoredItem> storedItems = GetStoredItems(XtraBinding);

            if (XtraBinding is XtraBindingChild)
            {
                string fk = ((XtraBindingChild)XtraBinding).ChildProperties.fkFieldName;

                _updateSetDataConverter.GetUnderlyingObjects(XtraBinding.DataSource,
                    storedItems.Where(p => p.FieldsValues[fk].Equals(((XtraBindingChild)XtraBinding).ParentPKValue) 
                        && p.State == XtraBindingStoredItemState.Added)
                    , true);
            }

            var r = storedItems.Select(p => new { i = XtraBinding.GetDataSourceItemsByValue(key, p.FieldsValues[key]).SingleOrDefault(), s = p }).Where(p => p.i != null);

            foreach (var i in r.Where(p=>p.s.State == XtraBindingStoredItemState.Modifyed))
            {
                foreach (string fieldName in _updateSetDataConverter.GetFieldsNames(XtraBinding.DataSource))
                {
                    XtraBinding.SetDataSourceItemValue(i.i, fieldName, i.s.FieldsValues[fieldName]);
                }

                i.s.UpdateDataSourceItem(i.i);
            }

            foreach (var i in r.Where(p => p.s.State == XtraBindingStoredItemState.Removed))
            {
                XtraBinding.BindingSource.Remove(i.i);

                i.s.UpdateDataSourceItem(i.i);
            }

            foreach (var i in r.Where(p => p.s.State == XtraBindingStoredItemState.Added))
            {
                i.s.UpdateDataSourceItem(i.i);
            }
        }

        internal IEnumerable<IDictionary<string, object>> ConvertToItems(object DataSource)
        {
            return from i in _updateSetDataConverter.GetUnderlyingObjects(DataSource, null, true)
                    select GetFieldsValues(DataSource, i);
        }
    }
}
