using System;
using Uniconta.DataModel;

namespace Algorithms.DataFetch
{
    public class UnicontaItem : InvItem
    {
        public string ProductNumber
        {
            get { return GetUserFieldString(0); }
            set { SetUserFieldString(0, value); }
        }

        public string Condition
        {
            get { return GetUserFieldString(1); }
            set { SetUserFieldString(1, value); }
        }

        public string ExternalCondition
        {
            get { return GetUserFieldString(2); }
            set { SetUserFieldString(2, value); }
        }

        public string WorkCondition
        {
            get { return GetUserFieldString(3); }
            set { SetUserFieldString(3, value); }
        }

        public string Agreement
        {
            get { return GetUserFieldString(4); }
            set { SetUserFieldString(4, value); }
        }

#pragma warning disable IDE1006 // Naming Styles
        public string eBayNotes
#pragma warning restore IDE1006 // Naming Styles
        {
            get { return GetUserFieldString(5); }
            set { SetUserFieldString(5, value); }
        }

        public long BatchSize
        {
            get { return GetUserFieldInt64(6); }
            set { SetUserFieldInt64(6, value); }
        }

        public double LossPercent
        {
            get { return GetUserFieldDouble(7); }
            set { SetUserFieldDouble(7, value); }
        }

        public string InBox
        {
            get { return GetUserFieldString(8); }
            set { SetUserFieldString(8, value); }
        }

        public string SKU
        {
            get { return GetUserFieldString(9); }
            set { SetUserFieldString(9, value); }
        }

        public string HSCode
        {
            get { return GetUserFieldString(10); }
            set { SetUserFieldString(10, value); }
        }

        public string CountryProduced
        {
            get { return GetUserFieldString(11); }
            set { SetUserFieldString(11, value); }
        }

        public double ProductLength
        {
            get { return GetUserFieldDouble(12); }
            set { SetUserFieldDouble(12, value); }
        }

        public double ProductWeight
        {
            get { return GetUserFieldDouble(13); }
            set { SetUserFieldDouble(13, value); }
        }

#pragma warning disable IDE1006 // Naming Styles
        public string eBayProductText
#pragma warning restore IDE1006 // Naming Styles
        {
            get { return GetUserFieldString(14); }
            set { SetUserFieldString(14, value); }
        }

        public string ItemGroup1
        {
            get { return GetUserFieldString(15); }
            set { SetUserFieldString(15, value); }
        }

        public string ItemGroup2
        {
            get { return GetUserFieldString(16); }
            set { SetUserFieldString(16, value); }
        }

        public string SellingType
        {
            get { return GetUserFieldString(17); }
            set { SetUserFieldString(17, value); }
        }

        public double StartingPrice
        {
            get { return GetUserFieldDouble(18); }
            set { SetUserFieldDouble(18, value); }
        }

        public double ProductWidth
        {
            get { return GetUserFieldDouble(19); }
            set { SetUserFieldDouble(19, value); }
        }

        public double ProductHeight
        {
            get { return GetUserFieldDouble(20); }
            set { SetUserFieldDouble(20, value); }
        }

        public bool SoldOut
        {
            get { return GetUserFieldBoolean(21); }
            set { SetUserFieldBoolean(21, value); }
        }

        public long UnitCount
        {
            get { return GetUserFieldInt64(22); }
            set { SetUserFieldInt64(22, value); }
        }

        public string ProductReference
        {
            get { return GetUserFieldString(23); }
            set { SetUserFieldString(23, value); }
        }

        public DateTime GarantiPeriod
        {
            get { return GetUserFieldDateTime(24); }
            set { SetUserFieldDateTime(24, value); }
        }

        public DateTime InventoryAge
        {
            get { return GetUserFieldDateTime(25); }
            set { SetUserFieldDateTime(25, value); }
        }

        public string SendingType
        {
            get { return GetUserFieldString(26); }
            set { SetUserFieldString(26, value); }
        }

        public string StoragePosition
        {
            get { return GetUserFieldString(27); }
            set { SetUserFieldString(27, value); }
        }

        public string EbayLink
        {
            get { return GetUserFieldString(28); }
            set { SetUserFieldString(28, value); }
        }

        public string ProductVersion
        {
            get { return GetUserFieldString(29); }
            set { SetUserFieldString(29, value); }
        }

        public string Status
        {
            get { return GetUserFieldString(30); }
            set { SetUserFieldString(30, value); }
        }

        public string InternalNote
        {
            get { return GetUserFieldString(31); }
            set { SetUserFieldString(31, value); }
        }

        public string Registrator
        {
            get { return GetUserFieldString(32); }
            set { SetUserFieldString(32, value); }
        }

        public DateTime DatePriced
        {
            get { return GetUserFieldDateTime(33); }
            set { SetUserFieldDateTime(33, value); }
        }

        public DateTime DateListed
        {
            get { return GetUserFieldDateTime(34); }
            set { SetUserFieldDateTime(34, value); }
        }

    }
}