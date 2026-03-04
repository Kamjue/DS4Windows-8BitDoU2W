using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightBitDoU2WReader.Device
{
    public readonly struct EightBitDoU2WReport
    {
        private readonly Memory<byte> rawReport;

        public EightBitDoU2WReport(Memory<byte> rawReport)
        {
            if (rawReport.Length < 32)
                throw new ArgumentException("Invalid report length", nameof(rawReport));
            this.rawReport = rawReport;
        }

        private byte dpad_state => (byte)(rawReport.Span[1] & 0x0F);

        public bool IsDPadUpPressed => dpad_state == 0 || dpad_state == 1 || dpad_state == 7;
        public bool IsDPadRightPressed => dpad_state == 1 || dpad_state == 2 || dpad_state == 3;
        public bool IsDPadDownPressed => dpad_state == 3 || dpad_state == 4 || dpad_state == 5;
        public bool IsDPadLeftPressed => dpad_state == 5 || dpad_state == 6 || dpad_state == 7;

        public byte LS_X => rawReport.Span[2];
        public byte LS_Y => rawReport.Span[3];

        public byte RS_X => rawReport.Span[4];
        public byte RS_Y => rawReport.Span[5];

        public byte RT => rawReport.Span[6];
        public byte LT => rawReport.Span[7];

        private byte buttons0 => rawReport.Span[8];
        private byte buttons1 => rawReport.Span[9];
        private byte buttons2 => rawReport.Span[10];

        public bool IsAPressed => (buttons0 & (1 << 0)) != 0;
        public bool IsBPressed => (buttons0 & (1 << 1)) != 0;
        public bool IsXPressed => (buttons0 & (1 << 3)) != 0;
        public bool IsYPressed => (buttons0 & (1 << 4)) != 0;

        public bool IsLBPressed => (buttons0 & (1 << 6)) != 0;
        public bool IsRBPressed => (buttons0 & (1 << 7)) != 0;

        public bool IsSelectPressed => (buttons1 & (1 << 2)) != 0;
        public bool IsStartPressed => (buttons1 & (1 << 3)) != 0;
        public bool IsHOMEPressed => (buttons1 & (1 << 4)) != 0;
        public bool IsLSPressed => (buttons1 & (1 << 5)) != 0;
        public bool IsRSPressed => (buttons1 & (1 << 6)) != 0;

        public bool IsM4Pressed => (buttons2 & (1 << 0)) != 0;
        public bool IsM3Pressed => (buttons2 & (1 << 1)) != 0;
        public bool IsM2Pressed => (buttons0 & (1 << 5)) != 0;
        public bool IsM1Pressed => (buttons0 & (1 << 2)) != 0;

        public bool IsCPressed => false;
        public bool IsZPressed => false;
        public bool IsFNPressed => false;

        public bool IsLTPressed => (buttons1 & (1 << 0)) != 0;
        public bool IsRTPressed => (buttons1 & (1 << 1)) != 0;

        public short AccelYRaw => BitConverter.ToInt16(rawReport.Span[15..17]);
        public short AccelXRaw => BitConverter.ToInt16(rawReport.Span[17..19]);
        public short AccelZRaw => BitConverter.ToInt16(rawReport.Span[19..21]);

        public short RollRaw => BitConverter.ToInt16(rawReport.Span[21..23]);
        public short PitchRaw => BitConverter.ToInt16(rawReport.Span[23..25]);
        public short YawRaw => BitConverter.ToInt16(rawReport.Span[25..27]);

        // F_GYRO_DPS_SCALE in U2W is 14.2824f. DS4 Standard is 16.384f. We scale Gyro.
        public short YawCalibrated => (short)Math.Clamp(YawRaw * (16.384f / 14.2824f), short.MinValue, short.MaxValue);
        public short PitchCalibrated => (short)Math.Clamp(PitchRaw * (16.384f / 14.2824f), short.MinValue, short.MaxValue);
        public short RollCalibrated => (short)Math.Clamp(RollRaw * (16.384f / 14.2824f), short.MinValue, short.MaxValue);

        // F_ACC_RES_PER_G in U2W is 4096. DS4 Standard is 8192. We scale Accel.
        public short AccelXCalibrated => (short)Math.Clamp(AccelXRaw * 2, short.MinValue, short.MaxValue);
        public short AccelYCalibrated => (short)Math.Clamp(AccelYRaw * 2, short.MinValue, short.MaxValue);
        public short AccelZCalibrated => (short)Math.Clamp(AccelZRaw * 2, short.MinValue, short.MaxValue);

        public bool IsAirMouseActive => false;
    }
}
