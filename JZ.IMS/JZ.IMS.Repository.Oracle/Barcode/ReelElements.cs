using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;

namespace JZ.IMS.Repository.Barcode
{
    public class ReelElements
    {
        public string Coo;
        public string CustomerPN;
        public string DateCode;
        public string LotCode;
        public string MakerCode;
        public string MakerPN;
        public string PartNo;
        public string Qty;
        public string ReelCode;
        public string Ref;
        public string P2Qty;

        public override string ToString()
        {
            return string.Format(
                BarcodeFilter.Reel2DBarcodePattern,
                string.IsNullOrEmpty(this.ReelCode) ? "" : this.ReelCode,
                string.IsNullOrEmpty(this.PartNo) ? "" : this.PartNo,
                string.IsNullOrEmpty(this.MakerPN) ? "" : this.MakerPN,
                string.IsNullOrEmpty(this.Qty) ? "" : this.Qty,
                string.IsNullOrEmpty(this.DateCode) ? "" : this.DateCode,
                string.IsNullOrEmpty(this.LotCode) ? "" : this.LotCode,
                string.IsNullOrEmpty(this.Coo) ? "" : this.Coo,
                string.IsNullOrEmpty(this.MakerCode) ? "" : this.MakerCode,
                string.IsNullOrEmpty(this.CustomerPN) ? "" : this.CustomerPN,
                string.IsNullOrEmpty(this.Ref) ? "" : this.Ref,
                string.IsNullOrEmpty(this.P2Qty) ? "" : this.P2Qty);
        }

        public static implicit operator Reel(ReelElements elements)
        {
            Reel reel = new Reel();
            reel.CODE = elements.ReelCode;
            reel.COO = elements.Coo;
            reel.CustomerPN = elements.CustomerPN;
            reel.DateCode = decimal.Parse(elements.DateCode);
            reel.LotCode = elements.LotCode;
            reel.MakerName = elements.MakerCode;
            reel.MakerPN = elements.MakerPN;
            reel.PART_NO = elements.PartNo;
            reel.Quantity = decimal.Parse(elements.Qty);
            reel.REF = elements.Ref;
            
            return reel;
        }

        public static implicit operator ReelElements(Reel reel)
        {
            ReelElements elem = new ReelElements();
            elem.ReelCode = reel.CODE;
            elem.Coo = reel.COO;
            elem.CustomerPN = reel.CustomerPN;
            elem.DateCode = reel.DateCode.ToString();
            elem.LotCode = reel.LotCode;
            elem.MakerPN = reel.MakerPN;
            elem.P2Qty = reel.Quantity.ToString();
            elem.PartNo = reel.PART_NO;
            elem.Qty = reel.Quantity.ToString();
            elem.Ref = reel.REF;
            return elem;
        }
    }
}
