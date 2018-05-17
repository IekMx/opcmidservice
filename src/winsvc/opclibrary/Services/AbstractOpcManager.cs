using OPCAutomation;
using opclibrary.Mappings;
using System;
using System.Diagnostics;
using System.Linq;

namespace opclibrary.Services
{
    public delegate void DataReceivedDelegate(object sender, Mappings.OpcEventArgs e);
    public delegate void StatusMessageDelegate(Exception e, string message);

    public abstract class AbstractOpcManager
    {
        public event DataReceivedDelegate DataReceived;
        public event StatusMessageDelegate StatusUpdated;

        protected static AbstractOpcManager _instance;
        private OPCAutomation.OPCServer _OpcServer;
        private OPCAutomation.OPCGroup _OpcGroup;

        public string ServerName { get; private set; }
        public OpcConfiguration Config { get; set; }

        public enum CanonicalDataTypes
        {
            CanonDtByte = 17,
            CanonDtChar = 16,
            CanonDtWord = 18,
            CanonDtShort = 2,
            CanonDtDWord = 19,
            CanonDtLong = 3,
            CanonDtFloat = 4,
            CanonDtDouble = 5,
            CanonDtBool = 11,
            CanonDtString = 8,
        }

        public AbstractOpcManager(string serverName)
        {
            ServerName = serverName;
        }

        public static AbstractOpcManager GetInstance()
        {
            return _instance;
        }

        public void Initialize(OpcConfiguration config)
        {
            Config = config;

            try
            {
                _OpcServer = new OPCAutomation.OPCServer();
                _OpcServer.Connect(ServerName, "");
                _OpcServer.OPCGroups.DefaultGroupIsActive = true;
                _OpcServer.OPCGroups.DefaultGroupDeadband = 0;
                _OpcGroup = _OpcServer.OPCGroups.Add("Group0");
                _OpcGroup.UpdateRate = 250;
                _OpcGroup.IsSubscribed = true;

                _OpcGroup.DataChange += OPCGroup_DataChanged;
                _OpcGroup.OPCItems.DefaultIsActive = true;
                _OpcGroup.OPCItems.AddItems(Config.TagCount, Config.ClientTags.Select(x => x.Name).ToArray(), Config.ClientTags.Select(x => x.Handle).ToArray(), out var serverHandles, out var serverErrors);
                Config.ServerHandles = serverHandles;
                Config.ServerErrors = serverErrors;

                bool itemgood = false;

                for (int i = 1; i <= config.TagCount; i++)
                {
                    int ab = (Int32)Config.ServerErrors.GetValue(i);
                    if (ab == 0)
                    {
                        itemgood = true;
                    }
                }
                if (!itemgood)
                {
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
                _OpcServer = null;
            }
        }

        public void Write(int index)
        {
            try
            {
                short ItemCount = 1;
                int[] SyncItemServerHandles = new int[2];
                object[] SyncItemValues = new object[2];
                System.Array SyncItemServerErrors = null;
                OPCAutomation.OPCItem AnOpcItem = default(OPCItem);

                SyncItemServerHandles[1] = (int)Config.ServerHandles.GetValue(index);
                AnOpcItem = _OpcGroup.OPCItems.GetOPCItem((int)Config.ServerHandles.GetValue(index));

                Array ItsAnArray = null;
                short CanonDT = 0;
                short vbArray = 8192;

                CanonDT = AnOpcItem.CanonicalDataType;
                if (CanonDT > vbArray)
                {
                    CanonDT -= vbArray;
                }

                switch (CanonDT)
                {
                    case (short)CanonicalDataTypes.CanonDtByte:
                        if ((int)Config.ItemIsArray.GetValue(index) > 0)
                        {
                            ItsAnArray = Array.CreateInstance(typeof(byte), (int)Config.ItemIsArray.GetValue(index));
                            if (!LoadArray(ref ItsAnArray, CanonDT, Module1.TagList[index].Name))
                            {
                            }
                            SyncItemValues[1] = ItsAnArray;
                        }
                        else
                        {
                            SyncItemValues[1] = Convert.ToByte((string)Module1.TagList[index].Value);
                        }
                        break;

                    case (short)CanonicalDataTypes.CanonDtChar:
                        if ((int)Config.ItemIsArray.GetValue(index) > 0)
                        {
                            ItsAnArray = Array.CreateInstance(typeof(SByte), (int)Config.ItemIsArray.GetValue(index));
                            if (!LoadArray(ref ItsAnArray, CanonDT, Module1.TagList[index].Name))
                            {
                            }
                            SyncItemValues[1] = ItsAnArray;
                        }
                        else
                        {
                            SyncItemValues[1] = Convert.ToSByte((string)Module1.TagList[index].Value);
                        }
                        break;

                    case (short)CanonicalDataTypes.CanonDtWord:
                        if ((int)Config.ItemIsArray.GetValue(index) > 0)
                        {
                            ItsAnArray = Array.CreateInstance(typeof(UInt16), (int)Config.ItemIsArray.GetValue(index));
                            if (!LoadArray(ref ItsAnArray, CanonDT, Module1.TagList[index].Name))
                            {
                            }
                            SyncItemValues[1] = ItsAnArray;
                        }
                        else
                        {
                            SyncItemValues[1] = Convert.ToUInt16(Module1.TagList[index].Value);
                        }
                        break;

                    case (short)CanonicalDataTypes.CanonDtShort:
                        if ((int)Config.ItemIsArray.GetValue(index) > 0)
                        {
                            ItsAnArray = Array.CreateInstance(typeof(Int16), (int)Config.ItemIsArray.GetValue(index));
                            if (!LoadArray(ref ItsAnArray, CanonDT, Module1.TagList[index].Name))
                            {
                            }
                            SyncItemValues[1] = ItsAnArray;
                        }
                        else
                        {
                            SyncItemValues[1] = Convert.ToInt16((string)Module1.TagList[index].Value);
                        }
                        break;

                    case (short)CanonicalDataTypes.CanonDtDWord:
                        if ((int)Config.ItemIsArray.GetValue(index) > 0)
                        {
                            ItsAnArray = Array.CreateInstance(typeof(UInt32), (int)Config.ItemIsArray.GetValue(index));
                            if (!LoadArray(ref ItsAnArray, CanonDT, Module1.TagList[index].Name))
                            {
                            }
                            SyncItemValues[1] = ItsAnArray;
                        }
                        else
                        {
                            SyncItemValues[1] = Convert.ToUInt32((string)Module1.TagList[index].Value);
                        }
                        break;

                    case (short)CanonicalDataTypes.CanonDtLong:
                        if ((int)Config.ItemIsArray.GetValue(index) > 0)
                        {
                            ItsAnArray = Array.CreateInstance(typeof(Int32), (int)Config.ItemIsArray.GetValue(index));
                            if (!LoadArray(ref ItsAnArray, CanonDT, Module1.TagList[index].Name))
                            {
                            }
                            SyncItemValues[1] = ItsAnArray;
                        }
                        else
                        {
                            SyncItemValues[1] = Convert.ToInt32((string)Module1.TagList[index].Value);
                        }
                        break;

                    case (short)CanonicalDataTypes.CanonDtFloat:
                        if ((int)Config.ItemIsArray.GetValue(index) > 0)
                        {
                            ItsAnArray = Array.CreateInstance(typeof(float), (int)Config.ItemIsArray.GetValue(index));
                            if (!LoadArray(ref ItsAnArray, CanonDT, Module1.TagList[index].Name))
                            {
                            }
                            SyncItemValues[1] = ItsAnArray;
                        }
                        else
                        {
                            SyncItemValues[1] = Convert.ToSingle((string)Module1.TagList[index].Value);
                        }
                        break;

                    case (short)CanonicalDataTypes.CanonDtDouble:
                        if ((int)Config.ItemIsArray.GetValue(index) > 0)
                        {
                            ItsAnArray = Array.CreateInstance(typeof(double), (int)Config.ItemIsArray.GetValue(index));
                            if (!LoadArray(ref ItsAnArray, CanonDT, Module1.TagList[index].Name))
                            {
                            }
                            SyncItemValues[1] = ItsAnArray;
                        }
                        else
                        {
                            SyncItemValues[1] = Module1.TagList[index].Value;
                        }
                        break;

                    case (short)CanonicalDataTypes.CanonDtBool:
                        if ((int)Config.ItemIsArray.GetValue(index) > 0)
                        {
                            ItsAnArray = Array.CreateInstance(typeof(bool), (int)Config.ItemIsArray.GetValue(index));
                            if (!LoadArray(ref ItsAnArray, CanonDT, Module1.TagList[index].Name))
                            {
                            }
                            SyncItemValues[1] = ItsAnArray;
                        }
                        else
                        {
                            SyncItemValues[1] = Convert.ToBoolean(Module1.TagList[index].Value);
                        }
                        break;
                    // End case

                    case (short)CanonicalDataTypes.CanonDtString:
                        if ((int)Config.ItemIsArray.GetValue(index) > 0)
                        {
                            ItsAnArray = Array.CreateInstance(typeof(string), (int)Config.ItemIsArray.GetValue(index));
                            if (!LoadArray(ref ItsAnArray, CanonDT, Module1.TagList[index].Name))
                            {
                            }
                            SyncItemValues[1] = ItsAnArray;
                        }
                        else
                        {
                            SyncItemValues[1] = Convert.ToString((string)Module1.TagList[index].Value);
                        }
                        break;
                        // End case

                }

                _OpcGroup.SyncWrite(ItemCount, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);

                if ((int)SyncItemServerErrors.GetValue(1) != 0)
                {
                    //MessageBox.Show("SyncItemServerError: " + SyncItemServerErrors.GetValue(1));
                }
            }
            catch (Exception ex)
            {

            }


        }


        public static bool LoadArray(ref System.Array AnArray, int CanonDT, string wrTxt)
        {
            int ii = 0;
            int loc = 0;
            int Wlen = 0;
            int start = 0;

            try
            {
                start = 1;
                Wlen = wrTxt.Length;
                for (ii = AnArray.GetLowerBound(0); ii <= AnArray.GetUpperBound(0); ii++)
                {
                    loc = wrTxt.IndexOf(",", 0);
                    if (ii < AnArray.GetUpperBound(0))
                    {
                        if (loc == 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        loc = Wlen + 1;
                    }

                    switch (CanonDT)
                    {
                        case (int)CanonicalDataTypes.CanonDtByte:
                            AnArray.SetValue(Convert.ToByte((wrTxt.Substring(start, loc - start))), ii);
                            break;
                        case (int)CanonicalDataTypes.CanonDtChar:
                            AnArray.SetValue(Convert.ToSByte((wrTxt.Substring(start, loc - start))), ii);
                            break;
                        case (int)CanonicalDataTypes.CanonDtWord:
                            AnArray.SetValue(Convert.ToUInt16((wrTxt.Substring(start, loc - start))), ii);
                            break;
                        case (int)CanonicalDataTypes.CanonDtShort:
                            AnArray.SetValue(Convert.ToInt16((wrTxt.Substring(start, loc - start))), ii);
                            break;
                        case (int)CanonicalDataTypes.CanonDtDWord:
                            AnArray.SetValue(Convert.ToInt32((wrTxt.Substring(start, loc - start))), ii);
                            break;
                        case (int)CanonicalDataTypes.CanonDtLong:
                            AnArray.SetValue(Convert.ToInt32((wrTxt.Substring(start, loc - start))), ii);
                            break;
                        case (int)CanonicalDataTypes.CanonDtFloat:
                            AnArray.SetValue(Convert.ToSingle((wrTxt.Substring(start, loc - start))), ii);
                            break;
                        case (int)CanonicalDataTypes.CanonDtDouble:
                            AnArray.SetValue(Convert.ToDouble((wrTxt.Substring(start, loc - start))), ii);
                            break;
                        case (int)CanonicalDataTypes.CanonDtBool:
                            AnArray.SetValue(Convert.ToBoolean((wrTxt.Substring(start, loc - start))), ii);
                            break;
                        case (int)CanonicalDataTypes.CanonDtString:
                            AnArray.SetValue(Convert.ToString((wrTxt.Substring(start, loc - start))), ii);
                            break;
                        default:
                            return false;
                    }

                    start = loc + 1;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private void OPCGroup_DataChanged(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            Mappings.OpcEventArgs e = new Mappings.OpcEventArgs();
            for (int i = 1; i <= ClientHandles.Length; i++)
            {
                if (ItemValues.GetValue(i) == null) continue;
                e.ItemHandle = (int)ClientHandles.GetValue(i);
                if (typeof(double) == ItemValues.GetValue(i).GetType())
                {
                    e.ItemValue = ItemValues.GetValue(i) != null ? (double)ItemValues.GetValue(i) : 0.0;
                }
                else if (typeof(bool) == ItemValues.GetValue(i).GetType())
                {
                    e.ItemValue = ItemValues.GetValue(i) != null ? (bool)ItemValues.GetValue(i) : false;
                    e.IsFault = (bool)e.ItemValue && ((e.ItemHandle <= 18 && e.ItemHandle >= 15) || (e.ItemHandle <= 98 && e.ItemHandle >= 93));
                }
                else if (typeof(byte) == ItemValues.GetValue(i).GetType())
                {
                    e.ItemValue = ItemValues.GetValue(i) != null ? (byte)ItemValues.GetValue(i) : new Byte();
                }
                else if (typeof(string) == ItemValues.GetValue(i).GetType())
                {
                    e.ItemValue = ItemValues.GetValue(i) != null ? (string)ItemValues.GetValue(i) : "";
                }
                else if (typeof(int) == ItemValues.GetValue(i).GetType())
                {
                    e.ItemValue = ItemValues.GetValue(i) != null ? (int)ItemValues.GetValue(i) : -1;
                }
                else if (typeof(short) == ItemValues.GetValue(i).GetType())
                {
                    e.ItemValue = ItemValues.GetValue(i) != null ? (short)ItemValues.GetValue(i) : -1;
                }
                else if (typeof(Single) == ItemValues.GetValue(i).GetType())
                {
                    e.ItemValue = ItemValues.GetValue(i) != null ? (Single)ItemValues.GetValue(i) : -1;
                }

                DataReceived?.Invoke(this, e);
            }
        }
    }
}
