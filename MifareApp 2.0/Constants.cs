using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareApp_2._0
{
    public static class Constants
    {
        public static readonly string SHOW_INITIALIZE_CARD_WINDOW = "ShowInitializeWindow";
        public static readonly string SHOW_CHARGE_DISCHARGE_WINDOW = "ShowChargeDischargeWindow";
        public static readonly string SHOW_CREATE_SERVICE_WINDOW = "ShowCreateServiceWindow";

        public static readonly string VIRGIN_MIFARE_KEY = "FFFFFFFFFFFF";
        public static readonly string MAD_KEY = "A0A1A2A3A4A5";
        public static readonly string SERVICE_ACCESS_BITS = "08778F00";
        public static readonly string EMPTY_BLOCK = "00000000000000000000000000000000";
        public static readonly string EMPTY_ELECTRONIC_WALLET = "00000000FFFFFFFF0000000010EF10EF";
        public static readonly string COMPANY_ID = "6B6F6368616D206D6F6A61206D616D65";
        public static readonly string MAD_INITIAL_SECTOR_TRAILER = "A0A1A2A3A4A50C33CFC1";

        public static readonly string CARD_RM = "CARD REMOVED";
        public static readonly string NOT_NONPERSONALIZED_CARD = "PERSONALIZED CARD";
        public static readonly string ACCESS_DENIED = "ACCESS DENIED";

        public static readonly int ASCII_OFFSET = 48;
        public static readonly int MASTER_KEY_LENGTH = 16;
        public static readonly int BLOCKS_IN_SECTOR = 4;
        public static readonly int TRAILER_BLOCK_NUMBER = 3;
        public static readonly int ELECTRONIC_WALLET_BLOCK_NUMBER = 1;

        public static readonly byte KEY_B = 0x61;
        public static readonly byte KEY_A = 0x60;
    }
}
