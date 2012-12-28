using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject.XOP;

namespace DataAvail.XObject
{
    public class XOFieldCalculator
    {
        internal XOFieldCalculator(XOField XOField)
        {
            //Route to a file could have some stupid format, like this
            // [ParentTable;ParentFieldRefOrValue|ChildTable;ChildField;ChildFieldRefOrValue],[...]        

            List<XOFieldCalculatorRouteRelation> rels = new List<XOFieldCalculatorRouteRelation>();

            XOField iterField = null;

            foreach (string refr in XOField.XopField.Calculator.Split(','))
            {
                string[] ets = refr.Split(';');

                if (_issueField == null)
                {
                    _issueField = ets[0];

                    iterField = XOField.XoTable.Fields.Single(p=>p.Name == _issueField);
                }

                XOFieldCalculatorRouteRelation rel = null;

                if (ets.Length == 2)
                {
                    string[] childTablRef = ets[1].Split('-');

                    if (childTablRef.Length == 1)
                    {
                        //Reference to parent table
                        XORelation parRel = iterField.ParentRelation;

                        //Refactoring here. (maybe when post building initialization were implemented)
                        if (parRel == null)
                            throw new NullReferenceException("Refernced table in calculator isn't defined in model file");

                        iterField = parRel.ParentTable.Fields.Single(p=>p.Name == ets[1]);

                        rel = new XOFieldCalculatorRouteRelation(
                                parRel.ParentTable,
                                parRel.ParentField,
                                iterField,
                                null);
                    }
                    else if (childTablRef.Length == 3)
                    {
                        //Reference to parent table
                        XORelation parRel = iterField.ParentRelation;

                        //Refactoring here. (maybe when post building initialization were implemented)
                        if (parRel == null)
                            throw new NullReferenceException("Refernced table in calculator isn't defined in model file");

                        XORelation parChildRel = parRel.ParentTable.ChildrenRelations.FirstOrDefault(p => p.ChildTable.Name == childTablRef[0] && p.ChildField.Name == childTablRef[1]);

                        if (parChildRel != null)
                        {
                            string filter = null;

                            string childColumn = childTablRef[2];

                            int filterStratBraceIndex = childColumn.IndexOf("[", 0);

                            int filterEndBraceIndex = -1;

                            if (filterStratBraceIndex != -1)
                                filterEndBraceIndex = childColumn.IndexOf("]", filterStratBraceIndex);

                            //Some formatting check and exceptions are needed here

                            if (filterStratBraceIndex != -1 && filterEndBraceIndex != -1)
                            {
                                filter = childColumn.Substring(filterStratBraceIndex + 1, filterEndBraceIndex - filterStratBraceIndex - 1);

                                childColumn = childColumn.Remove(filterStratBraceIndex, filterEndBraceIndex - filterStratBraceIndex + 1);
                            }


                            iterField = parChildRel.ChildTable.Fields.First(p => p.Name == childColumn);

                            rel = new XOFieldCalculatorRouteRelation(
                                parChildRel.ChildTable,
                                parChildRel.ChildField,
                                iterField,
                                filter);
                        }
                    }

                }

                if (rel == null)
                {
                    throw new Exception("Can't parse calculator expression string");
                }

                rels.Add(rel);
            }

            _routeRelations = rels.ToArray();
        }

        private string _issueField;

        private readonly XOFieldCalculatorRouteRelation[] _routeRelations;

        public string IssueField
        {
            get { return _issueField; }
        }

        public XOFieldCalculatorRouteRelation[] RouteRelations
        {
            get { return _routeRelations; }
        }
    }
}
