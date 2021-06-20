using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
    public class Reel
    {
        private string code;
        private string part_no;
        private string makerPN;
        private string customerPN;
        private string coo;
        private decimal dateCode;
        private decimal quantity;
        private string makerName;
        private string lotCode;
        private string refvalue;
        private string vendorName;
        private string vendorCode;
        private decimal vendorId;
        private decimal caseQty;
        private decimal makerId;
        private decimal makerPnId;
        private decimal partId;
        private decimal id;
        private string part_desc;
        private string part_name;
        private decimal status;
        private decimal onhandQty;
        private string statusName;
        public decimal ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

       
        public string VendorName
        {
            get { return this.vendorName; }
            set { this.vendorName = value; }
        }

       
        public string VendorCode
        {
            get { return this.vendorCode; }
            set { this.vendorCode = value; }
        }

       
        public decimal VendorID
        {
            get { return this.vendorId; }
            set { this.vendorId = value; }
        }

       
        public decimal MakerID
        {
            get { return this.makerId; }
            set { this.makerId = value; }
        }

       
        public decimal MakerPartID
        {
            get { return this.makerPnId; }
            set { this.makerPnId = value; }
        }

        
        public decimal PartID
        {
            get { return this.partId; }
            set { this.partId = value; }
        }

       
        public string CODE
        {
            get { return this.code; }
            set { this.code = value; }
        }
       
        public string PART_NO
        {
            get { return this.part_no; }
            set { this.part_no = value; }
        }
       
        public string PART_DESC
        {
            get { return this.part_desc; }
            set { this.part_desc = value; }
        }
       
        public string PART_NAME
        {
            get { return this.part_name; }
            set { this.part_name = value; }
        }
       
        public string MakerPN
        {
            get { return this.makerPN; }
            set { this.makerPN = value; }
        }
       
        public string CustomerPN
        {
            get { return this.customerPN; }
            set { this.customerPN = value; }
        }
       
        public string COO
        {
            get { return this.coo; }
            set { this.coo = value; }
        }
       
        public decimal DateCode
        {
            get { return this.dateCode; }
            set { this.dateCode = value; }
        }
       
        public decimal Quantity
        {
            get { return this.quantity; }
            set { this.quantity = value; }
        }
       
        public string MakerName
        {
            get { return this.makerName; }
            set { this.makerName = value; }
        }
       
        public string LotCode
        {
            get { return this.lotCode; }
            set { this.lotCode = value; }
        }
       
        public string REF
        {
            get { return this.refvalue; }
            set { this.refvalue = value; }
        }

       
        public decimal CaseQty
        {
            get { return this.caseQty; }
            set { this.caseQty = value; }
        }
       
        public decimal STATUS
        {
            get { return this.status; }
            set { this.status = value; }
        }
       
        public decimal OnhandQty
        {
            get { return this.onhandQty; }
            set { this.onhandQty = value; }
        }

       
        public string STATUS_NAME
        {
            get { return this.statusName; }
            set { this.statusName = value; }
        }

        /// <summary>
        /// 構造函數
        /// </summary>
        public Reel()
        {
            this.code = string.Empty;
            this.part_no = string.Empty;
            this.part_desc = string.Empty;
            this.makerPN = string.Empty;
            this.customerPN = string.Empty;
            this.coo = string.Empty;
            this.Quantity = 0;
            this.makerName = string.Empty;
            this.lotCode = string.Empty;
            this.refvalue = string.Empty;
            this.caseQty = 0;
            this.makerId = -1;
            this.makerPnId = -1;
            this.vendorId = -1;
            this.status = 0;
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            this.code = string.Empty;
            this.part_no = string.Empty;
            this.part_desc = string.Empty;
            this.makerPN = string.Empty;
            this.customerPN = string.Empty;
            this.coo = string.Empty;
            this.Quantity = 0;
            this.makerName = string.Empty;
            this.lotCode = string.Empty;
            this.refvalue = string.Empty;
            this.caseQty = 0;
            this.makerId = -1;
            this.makerPnId = -1;
            this.vendorId = -1;
            this.status = 0;
        }
    }
}
