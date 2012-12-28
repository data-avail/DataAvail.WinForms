using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XtraObjectProperties;

namespace DataAvail.XOP
{
    // [ParentTable;ParentFieldRefOrValue|ChildTable;ChildField;ChildFieldRefOrValue],[...]
    public class XOPFieldCalculator 
    {
        internal XOPFieldCalculator(XtraFieldProperties XtraFieldProperties)
        {
            List<XOPFieldCalculatorRouteRelation>  rels = new List<XOPFieldCalculatorRouteRelation>();

            XtraFieldProperties iterField = null;

            foreach (string refr in XtraFieldProperties.CalculatorExpression.Split(','))
            {
                string [] ets = refr.Split(';');

                if (_issueField == null)
                {
                    _issueField = ets[0];

                    iterField = XtraFieldProperties.XtraObjectProperties.Fields[_issueField];
                }

                XOPFieldCalculatorRouteRelation rel = null;

                if (ets.Length == 2)
                {
                    string[] childTablRef = ets[1].Split('-');

                    if (childTablRef.Length == 1)
                    {

                        //Reference to parent table

                        DataAvail.XtraObjectProperties.XtraObjectRelation parRel = iterField.ParentRelation;

                        iterField = parRel.ParentObject.Fields[ets[1]];

                        rel = new XOPFieldCalculatorRouteRelation(
                            parRel.ParentObject,
                            parRel.ParentField,
                            iterField,
                            null);
                    }
                    else if (childTablRef.Length == 3)
                    {
                        //Reference to child table

                        DataAvail.XtraObjectProperties.XtraObjectRelation childRel = iterField.ParentRelation.ParentObject.GetChildRelation(childTablRef[0], childTablRef[1]);

                        if (childRel != null)
                        {
                            string filter = null;

                            string childColumn = childTablRef[2];

                            int filterStratBraceIndex = childColumn.IndexOf("[",0);

                            int filterEndBraceIndex = -1;

                            if (filterStratBraceIndex != -1)
                                filterEndBraceIndex = childColumn.IndexOf("]",filterStratBraceIndex);

                            //Some formatting check and exceptions are needed here

                            if (filterStratBraceIndex != -1 && filterEndBraceIndex != -1)
                            {
                                filter = childColumn.Substring(filterStratBraceIndex + 1, filterEndBraceIndex - filterStratBraceIndex - 1);

                                childColumn = childColumn.Remove(filterStratBraceIndex, filterEndBraceIndex - filterStratBraceIndex + 1);
                            }


                            iterField = childRel.ChildObject.Fields[childColumn];

                            rel = new XOPFieldCalculatorRouteRelation(
                                childRel.ChildObject,
                                childRel.ChildField,
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

            _routeRealtions = rels.ToArray();
        }

        private readonly string _issueField;

        private readonly XOPFieldCalculatorRouteRelation[] _routeRealtions;

        public string IssueField
        {
            get { return _issueField; }
        } 

        public XOPFieldCalculatorRouteRelation[] RouteRealtions
        {
            get { return _routeRealtions; }
        }
    }

    public class XOPFieldCalculatorRouteRelation 
    {
        internal XOPFieldCalculatorRouteRelation(DataAvail.XtraObjectProperties.XtraObjectProperties ObjectForFill,
            DataAvail.XtraObjectProperties.XtraFieldProperties KeyFieldForFill,
            DataAvail.XtraObjectProperties.XtraFieldProperties FieldForRetrieve,
            string AuxFilter)
        {
            this.objectToFill = ObjectForFill;

            this.keyFieldForFill = KeyFieldForFill;

            this.fieldForRetrieve = FieldForRetrieve;

            this.auxFilter = AuxFilter;
        }

        public readonly string auxFilter;

        public readonly DataAvail.XtraObjectProperties.XtraObjectProperties objectToFill;

        public readonly DataAvail.XtraObjectProperties.XtraFieldProperties keyFieldForFill;

        public readonly DataAvail.XtraObjectProperties.XtraFieldProperties fieldForRetrieve;
    }
}
